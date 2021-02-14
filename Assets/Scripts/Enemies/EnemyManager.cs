using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    const int threadGroupSize = 64;

    public EnemySettings droneEnemySettings;
    public Enemy droneEnemyPrefab;
    public EnemySettings cannonEnemySettings;
    public Enemy cannonEnemyPrefab;
    public EnemySettings fighterEnemySettings;
    public Enemy fighterEnemyPrefab;
    public ComputeShader compute;
    public int numEnemiesToSpawn;
    public BoxCollider cage;
    public GameObject player;
    public GameObject bulletPrefab;
    public Transform bulletParent;
    
    List<Enemy> enemies = new List<Enemy>();

    public void Start() {
        Spawn();
        cage.size = new Vector3(droneEnemySettings.size.x, 10, droneEnemySettings.size.y);
    }

    public void Update() {
        if (enemies.Count > 0) {
            int numEnemies = enemies.Count;
            EnemyData[] enemyData = new EnemyData[numEnemies];

            for (int i = 0; i < numEnemies; i++) {
                enemyData[i].position = enemies[i].position;
                enemyData[i].velocity = enemies[i].velocity;
            }

            ComputeBuffer enemyBuffer = new ComputeBuffer(numEnemies, EnemyData.Size);
            enemyBuffer.SetData(enemyData);

            compute.SetBuffer(0, "enemies", enemyBuffer);
            compute.SetInt("numEnemies", numEnemies);
            compute.SetFloat("viewDistance", droneEnemySettings.viewDistance);
            compute.SetFloat("viewAngle", droneEnemySettings.viewAngle);

            int threadGroups = Mathf.CeilToInt(numEnemies / (float)threadGroupSize);
            compute.Dispatch(0, threadGroups, 1, 1);

            enemyBuffer.GetData(enemyData);

            for (int i = 0; i < numEnemies; i++) {
                enemies[i].seperationVector = enemyData[i].seperationVector;
                enemies[i].alignmentVector = enemyData[i].alignmentVector;
                enemies[i].cohesionVector = enemyData[i].cohesionVector;
                enemies[i].numPerceivedEnemies = enemyData[i].numPerceivedEnemies;

                enemies[i].UpdateEnemy();
            }

            enemyBuffer.Release();
        }
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if (droneEnemySettings != null) {
            Gizmos.DrawWireCube(this.transform.position, new Vector3(droneEnemySettings.size.x * 2, 10, droneEnemySettings.size.y * 2));
        }
    }

    public void Spawn() {
        for (int i = 0; i < numEnemiesToSpawn; i++) {
            Vector3 position = new Vector3(Random.Range(-droneEnemySettings.size.x, droneEnemySettings.size.x), 0, Random.Range(-droneEnemySettings.size.y, droneEnemySettings.size.y));
            Quaternion rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
            GameObject instantiatedEnemy = GameObject.Instantiate(droneEnemyPrefab.gameObject, position, rotation);
            instantiatedEnemy.transform.parent = this.transform;
            instantiatedEnemy.name = $"Enemy {i}";

            Enemy enemy = instantiatedEnemy.GetComponent<Enemy>();
            enemy.enemySettings = droneEnemySettings;
            enemy.enemyManager = this;
            enemy.player = player;
            enemy.bulletPrefab = bulletPrefab;
            enemy.bulletParent = bulletParent;
            enemies.Add(enemy);
        }
    }
    
    public void RemoveEnemy(Enemy enemy) {
        enemies.Remove(enemy);
    }

    public struct EnemyData {
        public Vector2 position;
        public Vector2 velocity;

        public Vector2 seperationVector;
        public Vector2 alignmentVector;
        public Vector2 cohesionVector;
        public int numPerceivedEnemies;

        public static int Size {
            get {
                return sizeof (float) * 2 * 5 + sizeof (int);
            }
        }
    }
}