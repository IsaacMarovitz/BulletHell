using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour {

    public Animator creditAnimator;
    public MenuUI menuUI;
    public Button backButton;

    void Start() {
        backButton.onClick.AddListener(Back);
    }

    void Back() {
        creditAnimator.Play("Fade & Slide Out");
        menuUI.animator.Play("Fade & Slide In");
        menuUI.currentMenu = CurrentMenu.Main;
    }
}
