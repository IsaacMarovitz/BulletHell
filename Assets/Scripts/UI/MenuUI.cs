using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;
using System.Collections;

public class MenuUI : MonoBehaviour {
    
    public EnemySettings droneSettings;
    public EnemySettings cannonSettings;
    public EnemySettings fighterSettings;
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
    public ScoreboardUI scoreboardUI;
    public CreditsUI creditsUI;
    public DeathUI deathUI;
    public DeathUI winUI;
    public int pauseFadeDuration;
    public AnimationCurve timeCurve;
    public PlayerInput playerInput;
    public CurrentMenu currentMenu;

    float droneTargetWeight;
    float cannonTargetWeight;
    float fighterTargetWeight;
    float startTime;
    float pauseStartTime;
    float pauseTime;
    bool gameStarted = false;
    bool gameEnded = false;

    void Start() {
        newGameButton.onClick.AddListener(NewGame);
        settingsButton.onClick.AddListener(() => OpenMenu(settingsUI, CurrentMenu.Settings));
        scoreboardButton.onClick.AddListener(() => OpenMenu(scoreboardUI, CurrentMenu.Scoreboard));
        creditsButton.onClick.AddListener(() => OpenMenu(creditsUI, CurrentMenu.Credits));
        mainMenuButton.onClick.AddListener(MainMenu);
        quitButton.onClick.AddListener(Quit);
        mainMenuButton.gameObject.SetActive(false);
        droneTargetWeight = droneSettings.targetWeight;
        droneSettings.targetWeight = 0;
        droneSettings.shootingEnabled = false;
        cannonTargetWeight = cannonSettings.targetWeight;
        cannonSettings.targetWeight = 0;
        cannonSettings.shootingEnabled = false;
        fighterTargetWeight = fighterSettings.targetWeight;
        fighterSettings.targetWeight = 0;
        fighterSettings.shootingEnabled = false;
        currentMenu = CurrentMenu.Main;
        Cursor.visible = true;
        Time.timeScale = 1;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);
        Application.runInBackground = true;
    }

    void OnApplicationFocus(bool hasFocus) {
        if (!hasFocus && gameStarted && !gameEnded) {
            Pause();
        }
    }

    void OnApplicationPause(bool hasPaused) {
        if (hasPaused && gameStarted && !gameEnded) {
            Pause();
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
            droneSettings.targetWeight = droneTargetWeight;
            droneSettings.shootingEnabled = true;
            cannonSettings.targetWeight = cannonTargetWeight;
            cannonSettings.shootingEnabled = true;
            fighterSettings.targetWeight = fighterTargetWeight;
            fighterSettings.shootingEnabled = true;
            musicManager.ChangeAudioState(musicManager.levelOneMusic);
            gameStarted = true;
        }
    }

    public void Resume() {
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
        playerInput.SwitchCurrentActionMap("Movement");
    }

    public void Pause() {
        pauseStartTime = Time.realtimeSinceStartup;
        uiCamera.Priority = 20;
        animator.Play("Fade In");
        gameUI.gameObject.SetActive(false);
        playerMovement.move = false;
        musicManager.Pause();
        Cursor.visible = true;
        playerInput.SwitchCurrentActionMap("Menu Controls");
        StartCoroutine(FadeTime(0, pauseFadeDuration));
    }

    public void Die() {
        gameEnded = true;
        uiCamera.Priority = 20;
        deathUI.OpenMenu();
        gameUI.gameObject.SetActive(false);
        playerMovement.move = false;
        musicManager.Pause();
        Cursor.visible = true;
        print($"<b>Save System:</b> Saving run: {gameUI.score}, {(Time.realtimeSinceStartup - startTime) - pauseTime}");
        SaveSystem.SaveRun(new Run(gameUI.score, (Time.realtimeSinceStartup - startTime) - pauseTime));
        playerInput.SwitchCurrentActionMap("Menu Controls");
        StartCoroutine(FadeTime(0, pauseFadeDuration));
    }
    
    public void Win() {
        gameEnded = true;
        uiCamera.Priority = 20;
        winUI.OpenMenu();
        gameUI.gameObject.SetActive(false);
        playerMovement.move = false;
        musicManager.Pause();
        Cursor.visible = true;
        print($"<b>Save System:</b> Saving run: {gameUI.score}, {(Time.realtimeSinceStartup - startTime) - pauseTime}");
        SaveSystem.SaveRun(new Run(gameUI.score, (Time.realtimeSinceStartup - startTime) - pauseTime));
        playerInput.SwitchCurrentActionMap("Menu Controls");
        StartCoroutine(FadeTime(0, pauseFadeDuration));
    }

    public void ChangeToPauseMenu() {
        menuTitle.text = "PAUSED";
        newGameButtonText.text = "RESUME";
        creditsButton.gameObject.SetActive(false);
        scoreboardButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(true);
    }

    void OpenMenu(UIMenuBase menu, CurrentMenu newCurrentMenu) {
        animator.Play("Fade & Slide Out");
        menu.OpenMenu();
        currentMenu = newCurrentMenu;
    }

    public void MainMenu() {
        mainMenu.SetActive(false);
        loadingMenu.SetActive(true);
        StartCoroutine(LoadAsync(SceneManager.GetActiveScene().buildIndex));
    }

    public void Quit() {
        Application.Quit();
    }

    public void OnPause(InputAction.CallbackContext value) {
        if (value.started && gameStarted && !gameEnded) {
            Pause();
        }
    }

    public void OnResume(InputAction.CallbackContext value) {
        if (value.started && gameStarted && !gameEnded) {
            Resume();
            pauseTime += (Time.realtimeSinceStartup - pauseStartTime);
            musicManager.Resume();
            StartCoroutine(FadeTime(1, pauseFadeDuration));
        }
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