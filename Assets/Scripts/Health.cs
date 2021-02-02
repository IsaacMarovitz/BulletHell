using UnityEngine;

public class Health : MonoBehaviour {

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
        Destroy(this.gameObject);
    }
}