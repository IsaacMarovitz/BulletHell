using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using System.Collections;

public class MenuUI : MonoBehaviour {
    
    public EnemySettings enemySettings;
    public MusicManager musicManager;
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
    public int pauseFadeDuration;
    public AnimationCurve timeCurve;

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
        enemySettings.shootingEnabled = false;
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
                currentMenu = CurrentMenu.Main;
                gameUI.SetActive(true);
                playerMovement.move = true;
                musicManager.Resume();
                StartCoroutine(FadeTime(1, pauseFadeDuration));
            } else {
                gamePaused = true;
                uiCamera.Priority = 20;
                animator.Play("Fade In");
                gameUI.SetActive(false);
                playerMovement.move = false;
                musicManager.Pause();
                StartCoroutine(FadeTime(0, pauseFadeDuration));
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
            musicManager.Resume();
            StartCoroutine(FadeTime(1, pauseFadeDuration));
        } else {
            uiCamera.Priority = 0;
            animator.Play("Fade Out");
            enemySettings.targetWeight = targetWeight;
            gameUI.SetActive(true);
            playerMovement.move = true;
            enemySettings.shootingEnabled = true;
            musicManager.ChangeAudioState(musicManager.levelOneMusic);
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

    public IEnumerator FadeTime(float end, float duration) {
        float currentTime = 0;
        float start = Time.timeScale;
        while (currentTime < duration) {
            currentTime += Time.unscaledDeltaTime;
            float pointAlongCurve = Mathf.Lerp(start, end, currentTime / duration);
            Time.timeScale = timeCurve.Evaluate(pointAlongCurve);
            yield return null;
        }
        yield break;
    }
}

public enum CurrentMenu { Main, Video, Audio };