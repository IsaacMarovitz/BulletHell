using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : UIMenuBase {

    public Button videoSettingsButton;
    public Button audioSettingsButton;
    public Button backButton;
    public VideoSettingsUI videoSettingsUI;
    public AudioSettingsUI audioSettingsUI;
    public MenuUI menuUI;
    [HideInInspector]
    public CurrentSettingsMenu currentSettingsMenu;

    public override void Start() {
        base.Start();
        videoSettingsButton.onClick.AddListener(() => OpenMenu(videoSettingsUI, CurrentSettingsMenu.Video));
        audioSettingsButton.onClick.AddListener(() => OpenMenu(audioSettingsUI, CurrentSettingsMenu.Audio));
        backButton.onClick.AddListener(Back);
        currentSettingsMenu = CurrentSettingsMenu.Main;
    }

    public override void OpenMenu() {
        base.OpenMenu();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(videoSettingsButton.gameObject);
    }

    public void CloseCurrentMenu() {
        if (currentSettingsMenu == CurrentSettingsMenu.Main) {
            animator.Play("Fade Out");
        } else if (currentSettingsMenu == CurrentSettingsMenu.Video) {
            videoSettingsUI.animator.Play("Fade Out");
        } else {
            audioSettingsUI.animator.Play("Fade Out");
        }
    }

    void OpenMenu(UIMenuBase menu, CurrentSettingsMenu newCurrentMenu) {
        animator.Play("Fade & Slide Out");
        menu.OpenMenu();
        currentSettingsMenu = newCurrentMenu;
    }

    public void Back() {
        animator.Play("Fade & Slide Out");
        menuUI.animator.Play("Fade & Slide In");
        menuUI.currentMenu = CurrentMenu.Main;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(menuUI.settingsButton.gameObject);
    }
}

public enum CurrentSettingsMenu { Main, Video, Audio }