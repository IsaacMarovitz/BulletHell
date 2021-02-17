using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour {

    public TMP_Text scoreText;
    public TMP_Text newScoreText;
    public Animator newScoreTextAnimator;
    public Animator tutorialAnimator;
    public int score;

    int[] scores = { 10 };
    string[] names = { "DRONE" };
    int newScoreTextScore = 0;

    public void Start() {
        tutorialAnimator.Play("Tutorial");
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