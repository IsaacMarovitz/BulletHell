using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    public float bulletSpeed = 15;
    public float decayTime = 5;
    public int damage = 5;
    public float startingSpeed;
    
    // This moves a bullet at a constant bullet and start speed.
    // If it runs out of decayTime or hits an object it'll be 
    // destroyed.
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

    // Check to see if the bullet has hit the player, if so, make
    // the player take damage and destory the bullet.
    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag == "Player") {
            //Debug.Log("Player was hit!");
            collider.transform.gameObject.GetComponentInParent<PlayerHealth>()?.TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}