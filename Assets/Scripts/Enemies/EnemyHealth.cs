using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour {

    [SerializeField]
    int health;

    public void TakeDamage(int damage) {
        health -= damage;

        if (health <= 0) {
            LogMessage.Send(this.gameObject, $"Took {damage} damage and died!");
            Die();
        }
    }

    public void Die() {
        LogMessage.Send(this.gameObject, "Died!");
        FindObjectOfType<EnemyManager>().RemoveEnemy(GetComponent<Enemy>());
        Destroy(this.gameObject);
    }
}