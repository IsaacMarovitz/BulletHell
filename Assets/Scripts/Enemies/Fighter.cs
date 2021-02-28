using UnityEngine;

public class Fighter : Enemy {
    
    public GameObject missilePrefab;
    public bool shoot = false;
    public float gunCooldown = 0.1f;
    public Transform leftMissileSpawn;
    public Transform rightMissileSpawn;
    public Transform missileParent;
    public AudioSource audioSource;
    public AudioClip missileSFX;
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
            GameObject instantiatedMissile;
            Missile missile;

            instantiatedMissile = GameObject.Instantiate(missilePrefab, leftMissileSpawn.position, Quaternion.Euler(rotation));
            instantiatedMissile.transform.parent = missileParent;
            missile = instantiatedMissile.GetComponent<Missile>();
            missile.player = player.transform;
            missile.startingSpeed = speed;

            instantiatedMissile = GameObject.Instantiate(missilePrefab, rightMissileSpawn.position, Quaternion.Euler(rotation));
            instantiatedMissile.transform.parent = missileParent;
            missile = instantiatedMissile.GetComponent<Missile>();
            missile.player = player.transform;
            missile.startingSpeed = speed;

            audioSource.PlayOneShot(missileSFX);
        } else {
            cooldownLeft -= Time.deltaTime;
        }

        speed = (this.transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = this.transform.position;
    }

}