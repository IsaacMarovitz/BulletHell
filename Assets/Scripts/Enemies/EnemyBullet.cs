using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    public float bulletSpeed = 15;
    public float decayTime = 5;
    public int damage = 5;
    public float startingSpeed;
    
    void Update() {
        this.transform.position += this.transform.right * (bulletSpeed + startingSpeed) * Time.deltaTime;
        decayTime -= Time.deltaTime;
        if (decayTime <= 0) {
            GameObject.Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag == "Player") {
            Debug.Log("Player was hit!");
            //collider.transform.gameObject.GetComponentInParent<PlayerHealth>()?.TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}