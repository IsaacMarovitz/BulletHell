using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    public bool move = false;
    public float turnSpeed = 5;
    public float moveSpeed = 10;
    [Range(0, 180)]
    public float maxTurnAngle = 150;
    public GameObject bulletPrefab;
    public float gunCooldown = 0.1f;
    public Image cooldownImage;
    public Transform bulletSpawn;
    public Transform bulletParent;
    public GameUI gameUI;
    public AudioSource audioSource;
    public AudioClip gunSFX;
    public GameObject rotationIndicatorParent;
    public Image rotationIndicator;
    public PlayerInput playerInput;

    float cooldownLeft;
    Vector3 lastPosition;
    Vector3 moveDir;
    float speed;
    bool shoot;

    void Update() {
        if (!move) 
            return;

        // Update cooldown image
        cooldownImage.fillAmount = 1 - (cooldownLeft / gunCooldown);
        if (this.transform.position.x > 400 || this.transform.position.x < -400) {
            this.transform.position = new Vector3(0, 0, this.transform.position.z);
        }
        if (this.transform.position.z > 400 || this.transform.position.z < -400) {
            this.transform.position = new Vector3(this.transform.position.x, 0, 0);
        }
        
        // If can shoot, shoot
        if (shoot) {
            if (cooldownLeft <= 0) {
                cooldownLeft = gunCooldown;
                GameObject instantiatedBullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.position, transform.rotation);
                instantiatedBullet.transform.parent = bulletParent;
                Bullet bullet = instantiatedBullet.GetComponent<Bullet>();
                bullet.startingSpeed = speed;
                bullet.gameUI = gameUI;
                audioSource.PlayOneShot(gunSFX);
            } else {
                cooldownLeft -= Time.deltaTime;
            }
        }

        if (playerInput.currentControlScheme == "Keyboard and Mouse") {
            moveDir = Mouse.current.position.ReadValue();
        }

        // Turns and moves the player towards the current mouse position relative to the player
        // and updates the rotationIndicator to show the new rotation without any lerping
        moveDir.z = 20;
        Vector3 objectPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        moveDir.x = moveDir.x - objectPosition.x;
        moveDir.y = moveDir.y - objectPosition.y;
        float dist = Mathf.Clamp01((Mathf.Sqrt((moveDir.x * moveDir.x) + (moveDir.y * moveDir.y)) - 20) / 100);
        float angle = -Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg + 90;
        if (angle > 180) {
            angle = angle - 360;
        }
        if (angle > maxTurnAngle) {
            angle = maxTurnAngle;
        } else if (angle < -maxTurnAngle) {
            angle = -maxTurnAngle;
        }
        rotationIndicatorParent.transform.rotation = Quaternion.Euler(-90, -90, angle + 90 + this.transform.rotation.eulerAngles.y);
        rotationIndicator.rectTransform.localPosition = new Vector3(2 + 3 * dist, 0, 0);
        rotationIndicator.color = new Color(1, 1, 1, 1 * dist);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, angle, 0)), Time.deltaTime * turnSpeed * dist);
        this.transform.position += this.transform.right * moveSpeed * dist * Time.deltaTime;
        speed = (this.transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = this.transform.position;
    }

    // Sets up Input System callback event for when the 'Shoot' Input Action is triggered
    public void OnShoot(InputAction.CallbackContext value) {
        if (value.performed) {
            shoot = true;
            return;
        } else if (value.canceled) {
            shoot = false;
            return;
        }
        shoot = false;
    }
}
