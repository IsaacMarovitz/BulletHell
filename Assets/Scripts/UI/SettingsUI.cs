using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : UIMenuBase {

    public Button videoSettingsButton;
    public Button audioSettingsButton;
    public Button backButton;
    public Animator videoSettingsAnimator;
    public Animator audioSettingsAnimator;
    public MenuUI menuUI;
    [HideInInspector]
    public CurrentSettingsMenu currentSettingsMenu;

    public override void Start() {
        base.Start();
        videoSettingsButton.onClick.AddListener(VideoSettings);
        audioSettingsButton.onClick.AddListener(AudioSettings);
        backButton.onClick.AddListener(Back);
        videoSettingsAnimator.Play("Closed");
        audioSettingsAnimator.Play("Closed");
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
            videoSettingsAnimator.Play("Fade Out");
        } else {
            audioSettingsAnimator.Play("Fade Out");
        }
    }

    public void VideoSettings() {
        animator.Play("Fade & Slide Out");
        videoSettingsAnimator.Play("Fade & Slide In");
        currentSettingsMenu = CurrentSettingsMenu.Video;
    }

    public void AudioSettings() {
        animator.Play("Fade & Slide Out");
        audioSettingsAnimator.Play("Fade & Slide In");
        currentSettingsMenu = CurrentSettingsMenu.Audio;
    }

    public void Back() {
        animator.Play("Fade & Slide Out");
        menuUI.animator.Play("Fade & Slide In");
        menuUI.currentMenu = CurrentMenu.Main;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(menuUI.settingsButton.gameObject);
    }
}

public enum CurrentSettingsMenu { Main, Video, Audio }