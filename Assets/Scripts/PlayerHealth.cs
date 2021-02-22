using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    
    public float maxHealth;
    public float health;
    public float regenHealth;
    public MenuUI menuUI;

    public bool hasDied = false;

    public void TakeDamage(float damage) {
        health -= damage;
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
                health += regenHealth * Time.deltaTime;
            }
        }
    }
}