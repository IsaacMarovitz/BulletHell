using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 5;
    public float moveSpeed = 5;
    public Rigidbody rb;
    public GameObject bulletPrefab;
    public float gunCooldown = 0.1f;
    public Transform bulletSpawn;
    public Transform bulletParent;

    public float cooldownLeft;
    public float dist;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        if (cooldownLeft <= 0 && Input.GetMouseButton(0)) {
            cooldownLeft = gunCooldown;
            GameObject instantiatedeBullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.position, transform.rotation);
            instantiatedeBullet.transform.parent = bulletParent;
            instantiatedeBullet.GetComponent<Bullet>().startingVelocity = rb.velocity;
        } else {
            cooldownLeft -= Time.deltaTime;
        }
        if (this.transform.position.x > 200 || this.transform.position.x < -200) {
            this.transform.position = new Vector3(0, 0, this.transform.position.z);
        }
        if (this.transform.position.z > 200 || this.transform.position.z < -200) {
            this.transform.position = new Vector3(this.transform.position.x, 0, 0);
        }
    }

    void FixedUpdate() {   
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 20;
        Vector3 objectPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        mousePosition.x = mousePosition.x - objectPosition.x;
        mousePosition.y = mousePosition.y - objectPosition.y;
        dist = Mathf.Clamp01((Mathf.Sqrt((mousePosition.x * mousePosition.x) + (mousePosition.y * mousePosition.y)) - 20) / 100);
        float angle = -Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, angle, 0)), Time.deltaTime * speed * dist);
        rb.velocity = transform.right * moveSpeed * dist;
    }
}
