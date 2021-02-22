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
    public Button creditsButton;
    public Button quitButton;
    public PlayerMovement playerMovement;
    public Animator animator;
    public GameObject gameUI;
    public Animator videoSettingsAnimator;
    public Animator audioSettingsAnimator;
    public Animator creditsAnimator;
    public Animator textCreditsAnimator;
    public Animator deathAnimator;
    [HideInInspector]
    public CurrentMenu currentMenu;
    public int pauseFadeDuration;
    public AnimationCurve timeCurve;

    float targetWeight;
    bool gameStarted = false;
    bool gamePaused = false;
    bool gameEnded = false;

    void Start() {
        newGameButton.onClick.AddListener(NewGame);
        videoSettingsButton.onClick.AddListener(VideoSettings);
        audioSettingsButton.onClick.AddListener(AudioSettings);
        creditsButton.onClick.AddListener(Credits);
        quitButton.onClick.AddListener(Quit);
        targetWeight = enemySettings.targetWeight;
        enemySettings.targetWeight = 0;
        enemySettings.shootingEnabled = false;
        videoSettingsAnimator.Play("Closed");
        audioSettingsAnimator.Play("Closed");
        creditsAnimator.Play("Closed");
        deathAnimator.Play("Closed");
        currentMenu = CurrentMenu.Main;
        Cursor.visible = true;
        Time.timeScale = 1;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && gameStarted && !gameEnded) {
            if (gamePaused) {
                Resume();
                musicManager.Resume();
                StartCoroutine(FadeTime(1, pauseFadeDuration));
            } else {
                gamePaused = true;
                uiCamera.Priority = 20;
                animator.Play("Fade In");
                gameUI.SetActive(false);
                playerMovement.move = false;
                musicManager.Pause();
                Cursor.visible = true;
                StartCoroutine(FadeTime(0, pauseFadeDuration));
            }
        }
    }

    public void NewGame() {
        if (gameStarted) {
            Resume();
            musicManager.Resume();
            StartCoroutine(FadeTime(1, pauseFadeDuration));
        } else {
            Resume();
            enemySettings.targetWeight = targetWeight;
            enemySettings.shootingEnabled = true;
            musicManager.ChangeAudioState(musicManager.levelOneMusic);
            gameStarted = true;
        }
    }

    public void Resume() {
        gamePaused = false;
        gameEnded = false;
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
        Cursor.visible = false;
    }

    public void Die() {
        gamePaused = true;
        gameEnded = true;
        uiCamera.Priority = 20;
        deathAnimator.Play("Fade In");
        gameUI.SetActive(false);
        playerMovement.move = false;
        musicManager.Pause();
        Cursor.visible = true;
        StartCoroutine(FadeTime(0, pauseFadeDuration));
    }

    public void ChangeToPauseMenu() {
        menuTitle.text = "PAUSED";
        newGameButtonText.text = "RESUME";
        creditsButton.gameObject.SetActive(false);
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

    public void Credits() {
        animator.Play("Fade & Slide Out");
        creditsAnimator.Play("Fade & Slide In");
        textCreditsAnimator.Play("Scroll", -1, 0f);
        currentMenu = CurrentMenu.Credits;
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

public enum CurrentMenu { Main, Video, Audio, Credits };