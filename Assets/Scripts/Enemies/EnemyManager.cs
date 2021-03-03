using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    const int threadGroupSize = 64;

    public EnemySettings droneEnemySettings;
    public Drone droneEnemyPrefab;
    public GameObject bulletPrefab;
    public Transform bulletParent;
    public int numOfDronesToSpawn;
    public EnemySettings cannonEnemySettings;
    public Cannon cannonEnemyPrefab;
    public int numOfCannonsToSpawn;
    public EnemySettings fighterEnemySettings;
    public Fighter fighterEnemyPrefab;
    public GameObject missilePrefab;
    public Transform missileParent;
    public int numOfFightersToSpawn;
    public ComputeShader compute;
    public GameObject player;
    public MenuUI menuUI;
    
    bool enemiesHaveBeenSpawned = false;
    bool hasWon = false;
    List<Enemy> enemies = new List<Enemy>();

    /*
    This is likely the most complex script in the game, and it 
    is in charge of spawning the enenmeis on start, dispatching
    the compute shader, retreaving the values from the shader, 
    and then updating every enemy, every frame.
    */

    // I'm using the first frame of LateUpdate() here instead of 
    // Start() as using start caused the enemies to not move when 
    // reloading the scene.
    public void LateUpdate() {
        if (!enemiesHaveBeenSpawned) {
            enemiesHaveBeenSpawned = true;
            Spawn();
        }
    }

    public void Update() {
        // Don't control enemy behaviour if they haven't spawned yet
        if (enemiesHaveBeenSpawned) {
            // If there are enemies still alive, update them, otherwise, the player has won
            if (enemies.Count > 0) {
                int numEnemies = enemies.Count;
                // Create a new enemyData array with the size of nunEnemies
                EnemyData[] enemyData = new EnemyData[numEnemies];

                // Add the information of each enemy to the array
                for (int i = 0; i < numEnemies; i++) {
                    enemyData[i].position = enemies[i].position;
                    enemyData[i].velocity = enemies[i].velocity;
                }

                // Calculate the exact size in memory of EnemyData and then make a new 
                // ComputeBuffer. The ComputeBuffer allows us to pass struct data back
                // and forth easily.
                int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(EnemyData));
                ComputeBuffer enemyBuffer = new ComputeBuffer(numEnemies, size);
                enemyBuffer.SetData(enemyData);

                // Set the variables inside the compute shader
                compute.SetBuffer(0, "enemies", enemyBuffer);
                compute.SetInt("numEnemies", numEnemies);
                compute.SetFloat("viewDistance", droneEnemySettings.viewDistance);
                compute.SetFloat("viewAngle", droneEnemySettings.viewAngle);

                // Dispatch an appropiate amount of threads
                int threadGroups = Mathf.CeilToInt(numEnemies / (float)threadGroupSize);
                compute.Dispatch(0, threadGroups, 1, 1);

                // Get the data back from the compute shader
                enemyBuffer.GetData(enemyData);

                // Give the enemies the newly computed data
                for (int i = 0; i < numEnemies; i++) {
                    enemies[i].seperationVector = enemyData[i].seperationVector;
                    enemies[i].alignmentVector = enemyData[i].alignmentVector;
                    enemies[i].cohesionVector = enemyData[i].cohesionVector;
                    enemies[i].numPerceivedEnemies = enemyData[i].numPerceivedEnemies;

                    // Call the update function on the enemy now it has the new values
                    enemies[i].UpdateEnemy();
                }

                // Release the memory of the buffer
                enemyBuffer.Release();
            } else {
                if (!hasWon) {
                    menuUI.Win();
                    hasWon = true;
                }
            }
        }
    }

    // Spawn each enemy type, each for loop is almost the same except for a few minor details
    // Each for loop spawns the appropriate number of enemies and places it at a random positon and rotation
    public void Spawn() {
        for (int i = 0; i < numOfDronesToSpawn; i++) {
            Vector3 position = new Vector3(Random.Range(-droneEnemySettings.size.x, droneEnemySettings.size.x), 0, Random.Range(-droneEnemySettings.size.y, droneEnemySettings.size.y));
            Quaternion rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
            GameObject instantiatedEnemy = GameObject.Instantiate(droneEnemyPrefab.gameObject, position, rotation);
            instantiatedEnemy.transform.position = position;
            instantiatedEnemy.transform.rotation = rotation;
            instantiatedEnemy.transform.parent = this.transform;
            instantiatedEnemy.name = $"Drone {i}";

            Drone enemy = instantiatedEnemy.GetComponent<Drone>();
            enemy.enemySettings = droneEnemySettings;
            enemy.enemyManager = this;
            enemy.player = player;
            enemy.bulletPrefab = bulletPrefab;
            enemy.bulletParent = bulletParent;
            enemies.Add(enemy);
        }
        for (int i = 0; i < numOfCannonsToSpawn; i++) {
            Vector3 position = new Vector3(Random.Range(-cannonEnemySettings.size.x, cannonEnemySettings.size.x), 0, Random.Range(-cannonEnemySettings.size.y, cannonEnemySettings.size.y));
            Quaternion rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
            GameObject instantiatedEnemy = GameObject.Instantiate(cannonEnemyPrefab.gameObject, position, rotation);
            instantiatedEnemy.transform.position = position;
            instantiatedEnemy.transform.rotation = rotation;
            instantiatedEnemy.transform.parent = this.transform;
            instantiatedEnemy.name = $"Cannon {i}";

            Cannon enemy = instantiatedEnemy.GetComponent<Cannon>();
            enemy.enemySettings = cannonEnemySettings;
            enemy.enemyManager = this;
            enemy.player = player;
            enemies.Add(enemy);
        }
        for (int i = 0; i < numOfFightersToSpawn; i++) {
            Vector3 position = new Vector3(Random.Range(-fighterEnemySettings.size.x, fighterEnemySettings.size.x), 0, Random.Range(-fighterEnemySettings.size.y, fighterEnemySettings.size.y));
            Quaternion rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
            GameObject instantiatedEnemy = GameObject.Instantiate(fighterEnemyPrefab.gameObject, position, rotation);
            instantiatedEnemy.transform.position = position;
            instantiatedEnemy.transform.rotation = rotation;
            instantiatedEnemy.transform.parent = this.transform;
            instantiatedEnemy.name = $"Fighter {i}";

            Fighter enemy = instantiatedEnemy.GetComponent<Fighter>();
            enemy.enemySettings = fighterEnemySettings;
            enemy.enemyManager = this;
            enemy.player = player;
            enemy.missilePrefab = missilePrefab;
            enemy.missileParent = missileParent;
            enemies.Add(enemy);
        }
    }
    
    public void RemoveEnemy(Enemy enemy) {
        enemies.Remove(enemy);
    }

    // This struct is used for passing data to and from the compute shader
    public struct EnemyData {
        public Vector2 position;
        public Vector2 velocity;

        public Vector2 seperationVector;
        public Vector2 alignmentVector;
        public Vector2 cohesionVector;
        public int numPerceivedEnemies;
    }
}