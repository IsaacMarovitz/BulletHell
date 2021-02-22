using UnityEngine;
using UnityEngine.UI;

public class ScoreboardUI : MonoBehaviour {

    public Animator scoreboardAnimator;
    public MenuUI menuUI;
    public Button backButton;

    void Start() {
        backButton.onClick.AddListener(Back);
    }

    void Back() {
        scoreboardAnimator.Play("Fade & Slide Out");
        menuUI.animator.Play("Fade & Slide In");
        menuUI.currentMenu = CurrentMenu.Main;
    }
}
