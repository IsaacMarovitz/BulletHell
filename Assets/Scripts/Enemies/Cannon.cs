using UnityEngine;
using System.Collections;

public class Cannon : Enemy {

    public LineRenderer lineRenderer;
    public float minCooldown;
    public float maxCooldown;
    public float shootTimeLength;
    public float timeToFire;
    public float length = 100;
    public AudioSource audioSource;
    public AudioClip laserSFX;
    public float aimVariation;
    public float moveSpeed;

    float cooldownLeft;
    float shootTime;
    bool shootComplete;
    bool shootStarted;
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
            directionVector = (player.transform.position - this.transform.position).normalized;

            if (!shootStarted) {
                shootStarted = true;
                StartCoroutine(LerpLaserLength());
            }

            if (shootComplete) {
                if (shootTime > 0) {
                    shootTime -= Time.deltaTime;
                } else {
                    shootTime = shootTimeLength;
                    cooldownLeft = Random.Range(minCooldown, maxCooldown);
                    shootStarted = false;
                }
            }
        } else {
            cooldownLeft -= Time.deltaTime;
        }
    }

    

    public IEnumerator LerpLaserLength() {
        float currentTime = 0;
        shootComplete = false;
        while (currentTime < timeToFire) {
            currentTime += Time.deltaTime;
            float distance = Mathf.Lerp(0, length, currentTime / timeToFire);
            lineRenderer.SetPosition(1, directionVector * distance);
            yield return null;
        }
        shootComplete = true;
    }
    
}