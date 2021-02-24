using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public Button settingsButton;
    public Button scoreboardButton;
    public Button creditsButton;
    public Button mainMenuButton;
    public Button quitButton;
    public PlayerMovement playerMovement;
    public Animator animator;
    public GameObject mainMenu;
    public GameObject loadingMenu;
    public Slider loadingSlider;
    public GameUI gameUI;
    public SettingsUI settingsUI;
    public Animator settingsAnimator;
    public Animator scoreboardAnimator;
    public Animator creditsAnimator;
    public Animator textCreditsAnimator;
    public Animator deathAnimator;
    [HideInInspector]
    public CurrentMenu currentMenu;
    public int pauseFadeDuration;
    public AnimationCurve timeCurve;

    float targetWeight;
    float startTime;
    float pauseStartTime;
    float pauseTime;
    bool gameStarted = false;
    bool gamePaused = false;
    bool gameEnded = false;

    void Start() {
        newGameButton.onClick.AddListener(NewGame);
        settingsButton.onClick.AddListener(Settings);
        scoreboardButton.onClick.AddListener(Scoreboard);
        creditsButton.onClick.AddListener(Credits);
        mainMenuButton.onClick.AddListener(MainMenu);
        quitButton.onClick.AddListener(Quit);
        mainMenuButton.gameObject.SetActive(false);
        targetWeight = enemySettings.targetWeight;
        enemySettings.targetWeight = 0;
        enemySettings.shootingEnabled = false;
        settingsAnimator.Play("Closed");
        scoreboardAnimator.Play("Closed");
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
                pauseTime += (Time.realtimeSinceStartup - pauseStartTime);
                musicManager.Resume();
                StartCoroutine(FadeTime(1, pauseFadeDuration));
            } else {
                gamePaused = true;
                pauseStartTime = Time.realtimeSinceStartup;
                uiCamera.Priority = 20;
                animator.Play("Fade In");
                gameUI.gameObject.SetActive(false);
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
            pauseTime += (Time.realtimeSinceStartup - pauseStartTime);
            musicManager.Resume();
            StartCoroutine(FadeTime(1, pauseFadeDuration));
        } else {
            Resume();
            startTime = Time.realtimeSinceStartup;
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
        } else if (currentMenu == CurrentMenu.Settings) {
            settingsUI.CloseCurrentMenu();
        } 
        currentMenu = CurrentMenu.Main;
        gameUI.gameObject.SetActive(true);
        playerMovement.move = true;
        Cursor.visible = false;
    }

    public void Die() {
        gamePaused = true;
        gameEnded = true;
        uiCamera.Priority = 20;
        deathAnimator.Play("Fade In");
        gameUI.gameObject.SetActive(false);
        playerMovement.move = false;
        musicManager.Pause();
        Cursor.visible = true;
        print($"<b>Save System:</b> Saving run: {gameUI.score}, {(Time.realtimeSinceStartup - startTime) - pauseTime}");
        SaveSystem.SaveRun(new Run(gameUI.score, (Time.realtimeSinceStartup - startTime) - pauseTime));
        StartCoroutine(FadeTime(0, pauseFadeDuration));
    }

    public void ChangeToPauseMenu() {
        menuTitle.text = "PAUSED";
        newGameButtonText.text = "RESUME";
        creditsButton.gameObject.SetActive(false);
        scoreboardButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(true);
    }

    public void Settings() {
        animator.Play("Fade & Slide Out");
        settingsAnimator.Play("Fade & Slide In");
        currentMenu = CurrentMenu.Settings;
    }

    public void Scoreboard() {
        animator.Play("Fade & Slide Out");
        scoreboardAnimator.Play("Fade & Slide In");
        currentMenu = CurrentMenu.Scoreboard;
    }

    public void Credits() {
        animator.Play("Fade & Slide Out");
        creditsAnimator.Play("Fade & Slide In");
        textCreditsAnimator.Play("Scroll", -1, 0f);
        currentMenu = CurrentMenu.Credits;
    }

    public void MainMenu() {
        mainMenu.SetActive(false);
        loadingMenu.SetActive(true);
        StartCoroutine(LoadAsync(SceneManager.GetActiveScene().buildIndex));
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

    IEnumerator LoadAsync(int sceneIndex) {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

        while(!loadScene.isDone) {
            float progress = Mathf.Clamp01(loadScene.progress / 0.9f);
            loadingSlider.value = progress;

            yield return null;
        }
    }
}

public enum CurrentMenu { Main, Settings, Scoreboard, Credits };