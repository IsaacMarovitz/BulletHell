using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public EnemySettings enemySettings;
    public Enemy enemyPrefab;
    public int numEnemiesToSpawn;

    public void Start() {
        Spawn();
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position, new Vector3(enemySettings.size.x * 2, 10, enemySettings.size.y * 2));
    }

    public void Spawn() {
        for (int i = 0; i < numEnemiesToSpawn; i++) {
            Vector3 position = new Vector3(Random.Range(-enemySettings.size.x, enemySettings.size.x), 0, Random.Range(-enemySettings.size.y, enemySettings.size.y));
            Quaternion rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
            GameObject instantiatedEnemy = GameObject.Instantiate(enemyPrefab.gameObject, position, rotation);
            instantiatedEnemy.transform.parent = this.transform;

            Enemy enemy = instantiatedEnemy.GetComponent<Enemy>();
            enemy.enemySettings = enemySettings;
        }
    }
}