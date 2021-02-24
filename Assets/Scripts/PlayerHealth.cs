using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    
    public float maxHealth;
    public float health;
    public float regenHealth;
    public float timeToStartRegen;
    public MenuUI menuUI;

    public bool hasDied = false;

    float timeSinceLastDamage;

    public void TakeDamage(float damage) {
        health -= damage;
        timeSinceLastDamage = 0;
    }

    public void Start() {
        health = maxHealth;
    }

    public void Update() {
        if (!hasDied) {
            if (health <= 0) {
                menuUI.Die();
                hasDied = true;
                //Debug.Log("Player Died!");
            }
            if (health < maxHealth) {
                if (timeSinceLastDamage >= timeToStartRegen) {
                    health += regenHealth * Time.deltaTime;
                } else {
                    timeSinceLastDamage += Time.deltaTime;
                }
            }
            if (health > maxHealth) {
                health = maxHealth;
            }
        }
    }
}