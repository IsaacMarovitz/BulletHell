using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class MenuUI : MonoBehaviour {
    
    public EnemySettings enemySettings;
    public CinemachineVirtualCamera uiCamera;
    public Button newGameButton;
    public Button videoSettingsButton;
    public Button audioSettingsButton;
    public Button quitButton;
    public PlayerMovement playerMovement;
    public Animator animator;
    public GameObject gameUI;
    public Animator videoSettingsAnimator;
    public Animator audioSettingsAnimator;

    float targetWeight;

    void Start() {
        newGameButton.onClick.AddListener(NewGame);
        videoSettingsButton.onClick.AddListener(VideoSettings);
        audioSettingsButton.onClick.AddListener(AudioSettings);
        quitButton.onClick.AddListener(Quit);
        targetWeight = enemySettings.targetWeight;
        enemySettings.targetWeight = 0;
        videoSettingsAnimator.Play("Closed");
        audioSettingsAnimator.Play("Closed");
    }

    public void NewGame() {
        uiCamera.Priority = 0;
        animator.Play("Fade Out");
        playerMovement.move = true;
        enemySettings.targetWeight = targetWeight;
        gameUI.SetActive(true);
    }

    public void VideoSettings() {
        animator.Play("Fade & Slide Out");
        videoSettingsAnimator.Play("Fade & Slide In");
    }

    public void AudioSettings() {
        animator.Play("Fade & Slide Out");
        audioSettingsAnimator.Play("Fade & Slide In");
    }

    public void Quit() {
        Application.Quit();
    }
}
