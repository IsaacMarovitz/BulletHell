using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class MenuUI : MonoBehaviour {
    
    public EnemySettings enemySettings;
    public CinemachineVirtualCamera uiCamera;
    public TMP_Text menuTitle;
    public TMP_Text newGameButtonText;
    public Button newGameButton;
    public Button videoSettingsButton;
    public Button audioSettingsButton;
    public Button quitButton;
    public PlayerMovement playerMovement;
    public Animator animator;
    public GameObject gameUI;
    public Animator videoSettingsAnimator;
    public Animator audioSettingsAnimator;
    [HideInInspector]
    public CurrentMenu currentMenu;

    float targetWeight;
    bool gameStarted = false;
    bool gamePaused = false;

    void Start() {
        newGameButton.onClick.AddListener(NewGame);
        videoSettingsButton.onClick.AddListener(VideoSettings);
        audioSettingsButton.onClick.AddListener(AudioSettings);
        quitButton.onClick.AddListener(Quit);
        targetWeight = enemySettings.targetWeight;
        enemySettings.targetWeight = 0;
        videoSettingsAnimator.Play("Closed");
        audioSettingsAnimator.Play("Closed");
        menuTitle.text = "BULLET HELL";
        currentMenu = CurrentMenu.Main;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && gameStarted) {
            if (gamePaused) {
                gamePaused = false;
                uiCamera.Priority = 0;
                if (currentMenu == CurrentMenu.Main) {
                    animator.Play("Fade Out");
                } else if (currentMenu == CurrentMenu.Video) {
                    videoSettingsAnimator.Play("Fade Out");
                } else if (currentMenu == CurrentMenu.Audio) {
                    audioSettingsAnimator.Play("Fade Out");
                }
                gameUI.SetActive(true);
                playerMovement.move = true;
                Time.timeScale = 1f;
            } else {
                gamePaused = true;
                uiCamera.Priority = 20;
                animator.Play("Fade In");
                gameUI.SetActive(false);
                playerMovement.move = false;
                Time.timeScale = 0f;
            }
        }
    }

    public void NewGame() {
        if (gameStarted) {
            gamePaused = false;
            uiCamera.Priority = 0;
            animator.Play("Fade Out");
            gameUI.SetActive(true);
            playerMovement.move = true;
            Time.timeScale = 1f;
        } else {
            uiCamera.Priority = 0;
            animator.Play("Fade Out");
            playerMovement.move = true;
            enemySettings.targetWeight = targetWeight;
            gameUI.SetActive(true);
            gameStarted = true;
            menuTitle.text = "PAUSED";
            newGameButtonText.text = "RESUME";
        }
    }

    public void VideoSettings() {
        animator.Play("Fade & Slide Out");
        videoSettingsAnimator.Play("Fade & Slide In");
        currentMenu = CurrentMenu.Video;
    }

    public void AudioSettings() {
        animator.Play("Fade & Slide Out");
        audioSettingsAnimator.Play("Fade & Slide In");
        currentMenu = CurrentMenu.Audio;
    }

    public void Quit() {
        Application.Quit();
    }
}

public enum CurrentMenu { Main, Video, Audio };