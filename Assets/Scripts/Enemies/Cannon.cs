using UnityEngine;

public class Cannon : Enemy {

    public LineRenderer lineRenderer;
    public float minCooldown;
    public float maxCooldown;
    public float maxshootTime;
    public AudioSource audioSource;
    public AudioClip laserSFX;
    public float aimVariation;
    public float moveSpeed;

    float cooldownLeft;
    float shootTime;
    Vector3 directionVector;

    void Start() {
        cooldownLeft = Random.Range(minCooldown, maxCooldown);
    }

    void Update() {
        if (cooldownLeft <= 0) {
            if (shootTime > 0) {
                shootTime -= Time.deltaTime;
                directionVector = (player.transform.position - this.transform.position).normalized;
                lineRenderer.SetPosition(1, directionVector * 100);
            } else {
                shootTime = maxshootTime;
                cooldownLeft  = Random.Range(minCooldown, maxCooldown);
            }
        } else {
            cooldownLeft -= Time.deltaTime;
        }
    }
    
}