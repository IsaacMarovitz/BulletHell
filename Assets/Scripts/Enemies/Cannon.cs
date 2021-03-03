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

    /*
    This is the main script for an unimplemented third enemy type,
    a cannon. I didn't end up having enough time to complete this 
    enemy, but it would've been a laser-based enemy that would fire 
    a laser that deals damage over time across the map aimed at the player.
    */

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