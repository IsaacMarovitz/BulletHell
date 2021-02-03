using UnityEngine;

public class Bullet : MonoBehaviour {

    public float bulletSpeed = 10;
    public float decayTime = 5;
    public int damage;
    public Vector3 startingVelocity;
    public Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        rb.velocity = transform.right * bulletSpeed + startingVelocity;
        decayTime -= Time.deltaTime;
        if (decayTime <= 0) {
            GameObject.Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag == "Enemy") {
            collider.transform.gameObject.GetComponentInParent<EnemyHealth>()?.TakeDamage(damage);
            Destroy(this.gameObject);
        } 
    }
}
