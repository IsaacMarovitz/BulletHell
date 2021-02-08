using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour {

    [SerializeField]
    int health;

    public void TakeDamage(int damage, GameUI gameUI) {
        health -= damage;

        if (health <= 0) {
            //LogMessage.Send(this.gameObject, $"Took {damage} damage and died!");
            gameUI.AddScore(EnemyType.Drone);
            Die();
        } else {
        }
    }

    public void Die() {
        //LogMessage.Send(this.gameObject, "Died!");
        FindObjectOfType<EnemyManager>().RemoveEnemy(GetComponent<Enemy>());
        Destroy(this.gameObject);
    }
}