using UnityEngine;

public class Drone : Enemy {

    public GameObject bulletPrefab;
    public bool shoot = false;
    public float gunCooldown = 0.1f;
    public Transform bulletSpawn;
    public Transform bulletParent;
    public AudioSource audioSource;
    public AudioClip gunSFX;
    public float aimVariation;

    float cooldownLeft;
    Vector3 lastPosition;
    Vector3 positionDifference;
    float speed;
    float angle;

    void Update() {
        positionDifference = player.transform.position - this.transform.position;
        angle = (Mathf.Atan2(positionDifference.z, positionDifference.x) * Mathf.Rad2Deg) + this.transform.rotation.eulerAngles.y;
        if (Mathf.Abs(angle) < enemySettings.shootAngle || Mathf.Abs(360-angle) < enemySettings.shootAngle) {
            if (positionDifference.magnitude < enemySettings.shootDistance) {
                shoot = true;
            } else {
                shoot = false;
            }
        } else {
            shoot = false;
        }

        if (shoot && cooldownLeft <= 0 && enemySettings.shootingEnabled) {
            cooldownLeft = gunCooldown;
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.y += Random.Range(-aimVariation, aimVariation);
            GameObject instantiatedBullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.Euler(rotation));
            instantiatedBullet.transform.parent = bulletParent;
            EnemyBullet bullet = instantiatedBullet.GetComponent<EnemyBullet>();
            bullet.startingSpeed = speed;
            audioSource.PlayOneShot(gunSFX);
        } else {
            cooldownLeft -= Time.deltaTime;
        }

        speed = (this.transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = this.transform.position;
    }
}