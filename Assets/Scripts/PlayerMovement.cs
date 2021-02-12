using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public bool move = false;
    public float turnSpeed = 5;
    public float moveSpeed = 10;
    public GameObject bulletPrefab;
    public float gunCooldown = 0.1f;
    public Transform bulletSpawn;
    public Transform bulletParent;
    public GameUI gameUI;

    float cooldownLeft;
    Vector3 lastPosition;
    float speed;

    void Update() {
        if (!move) {
            return;
        }
        if (cooldownLeft <= 0 && Input.GetMouseButton(0)) {
            cooldownLeft = gunCooldown;
            GameObject instantiatedeBullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.position, transform.rotation);
            instantiatedeBullet.transform.parent = bulletParent;
            Bullet bullet = instantiatedeBullet.GetComponent<Bullet>();
            bullet.startingSpeed = speed;
            bullet.gameUI = gameUI;
        } else {
            cooldownLeft -= Time.deltaTime;
        }
        if (this.transform.position.x > 200 || this.transform.position.x < -200) {
            this.transform.position = new Vector3(0, 0, this.transform.position.z);
        }
        if (this.transform.position.z > 200 || this.transform.position.z < -200) {
            this.transform.position = new Vector3(this.transform.position.x, 0, 0);
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 20;
        Vector3 objectPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        mousePosition.x = mousePosition.x - objectPosition.x;
        mousePosition.y = mousePosition.y - objectPosition.y;
        float dist = Mathf.Clamp01((Mathf.Sqrt((mousePosition.x * mousePosition.x) + (mousePosition.y * mousePosition.y)) - 20) / 100);
        float angle = -Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, angle, 0)), Time.deltaTime * turnSpeed * dist);
        this.transform.position += this.transform.right * moveSpeed * dist * Time.deltaTime;
        speed = (this.transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = this.transform.position;
    }
}
