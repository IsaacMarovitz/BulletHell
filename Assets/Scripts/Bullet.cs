using UnityEngine;

public class Bullet : MonoBehaviour {

    public float bulletSpeed = 10;
    public float decayTime = 5;
    public int damage;
    public Vector3 startingVelocity;
    public GameUI gameUI;

    void Update() {
        this.transform.position += this.transform.right * bulletSpeed + startingVelocity;
        decayTime -= Time.deltaTime;
        if (decayTime <= 0) {
            GameObject.Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag == "Enemy") {
            collider.transform.gameObject.GetComponentInParent<EnemyHealth>()?.TakeDamage(damage, gameUI);
            Destroy(this.gameObject);
        } 
    }
}
