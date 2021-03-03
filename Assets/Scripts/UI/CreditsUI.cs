using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : UIMenuBase {

    public Animator textAnimator;
    public MenuUI menuUI;
    public Button backButton;

    public override void Start() {
        base.Start();
        backButton.onClick.AddListener(Back);
    }

    // Open menu and re-start text scroll animation
    public override void OpenMenu() {
        base.OpenMenu();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(backButton.gameObject);
        textAnimator.Play("Scroll", -1, 0f);
    }

    void Back() {
        animator.Play("Fade & Slide Out");
        menuUI.animator.Play("Fade & Slide In");
        menuUI.currentMenu = CurrentMenu.Main;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(menuUI.creditsButton.gameObject);
    }
}
