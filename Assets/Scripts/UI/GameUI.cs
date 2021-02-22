using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour {

    public TMP_Text scoreText;
    public TMP_Text newScoreText;
    public Animator newScoreTextAnimator;
    public Animator tutorialAnimator;
    public int score;
    public Slider healthSlider;
    public PlayerHealth playerHealth;

    int[] scores = { 10 };
    string[] names = { "DRONE" };
    int newScoreTextScore = 0;

    public void Start() {
        tutorialAnimator.Play("Tutorial");
        healthSlider.maxValue = playerHealth.maxHealth;
    }

    public void Update() {
        healthSlider.value = playerHealth.health;
    }

    public void AddScore(EnemyType enemyType) {
        this.score += scores[(int)enemyType];
        scoreText.text = this.score.ToString();
        if (newScoreTextAnimator.GetCurrentAnimatorClipInfo(0).Length > 0) {
            newScoreTextScore += scores[(int)enemyType];
            newScoreText.text = $"KILLED {names[(int)enemyType]} +{newScoreTextScore}";
            if (newScoreTextAnimator.gameObject.activeInHierarchy) {
                newScoreTextAnimator.Play("Bounce Text");
            }
        } else {
            newScoreTextScore = scores[(int)enemyType];
            newScoreText.text = $"KILLED {names[(int)enemyType]} +{newScoreTextScore}";
            if (newScoreTextAnimator.gameObject.activeInHierarchy) {
                newScoreTextAnimator.Play("Bounce In");
            }
        }
    }

}

public enum EnemyType{ Drone };