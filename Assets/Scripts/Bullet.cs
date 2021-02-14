using UnityEngine;

public class Bullet : MonoBehaviour {

    public float bulletSpeed = 15;
    public float decayTime = 5;
    public int damage = 5;
    public float startingSpeed;
    public GameUI gameUI;

    void Update() {
        Vector3 posOffset = this.transform.right * (bulletSpeed + startingSpeed) * Time.deltaTime;
        if (float.IsNaN(posOffset.x) || float.IsNaN(posOffset.y) || float.IsNaN(posOffset.z)) {
            GameObject.Destroy(this.gameObject);
        } else {
            this.transform.position += posOffset;
        }
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
