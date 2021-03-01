using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour {

    [SerializeField]
    int health;
    [SerializeField]
    EnemyType enemyType;
    bool isDead = false;
    Enemy enemy;
    EnemyManager enemyManager;
    public GameObject deathParticles;

    public void Start() {
        enemy = GetComponent<Enemy>();
        enemyManager = enemy.enemyManager;
    }

    public void TakeDamage(int damage, GameUI gameUI) {
        health -= damage;

        if (health <= 0 && !isDead) {
            //LogMessage.Send(this.gameObject, $"Took {damage} damage and died!");
            gameUI.AddScore(enemyType);
            Die();
        }
    }

    public void Die() {
        GameObject.Instantiate(deathParticles, this.transform.position, Quaternion.identity);
        isDead = true;
        //LogMessage.Send(this.gameObject, "Died!");
        enemyManager.RemoveEnemy(enemy);
        Destroy(this.gameObject);
    }
}