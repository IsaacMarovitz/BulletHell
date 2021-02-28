using UnityEngine;

public class Missile : MonoBehaviour {
    
    public Transform player;
    public float missileSpeed = 20;
    public float turnSpeed = 50;
    public float timeToDetonate = 5;
    public int damage = 5;
    public int explosionRadius = 5;
    public float startingSpeed;

    void Update() {
        Vector3 posOffset = this.transform.right * (missileSpeed + startingSpeed) * Time.deltaTime;
        Vector3 direction = player.position - this.transform.position;
        float rotationAmount = Vector3.Cross(direction.normalized, this.transform.right).y;
        float rotation = -rotationAmount * turnSpeed * Time.deltaTime;
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y + rotation, this.transform.eulerAngles.z);

        if (float.IsNaN(posOffset.x) || float.IsNaN(posOffset.y) || float.IsNaN(posOffset.z)) {
            GameObject.Destroy(this.gameObject);
        } else {
            this.transform.position += posOffset;
        }

        timeToDetonate -= Time.deltaTime;
        if (timeToDetonate <= 0) {
            Detonate();
        }

        Collider[] colliders = Physics.OverlapSphere(this.transform.position, explosionRadius);

        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].transform.tag == "Player") {
                colliders[i].transform.gameObject.GetComponentInParent<PlayerHealth>()?.TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag == "Player") {
            //Debug.Log("Player was hit!");
            Detonate();
        }
    }

    void Detonate() {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, explosionRadius);

        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].transform.tag == "Player") {
                colliders[i].transform.gameObject.GetComponentInParent<PlayerHealth>()?.TakeDamage(damage);
            }
        }

        Destroy(this.gameObject);
    }
}