using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class DeathUI : MonoBehaviour {

    public MenuUI menuUI;
    public GameUI gameUI;
    public TMP_Text scoreText;
    public Button mainMenu;
    public Button quit;
    public GameObject deathMenu;
    public GameObject loadingMenu;
    public Slider loadingSlider;
    
    void Start() {
        mainMenu.onClick.AddListener(MainMenu);
        quit.onClick.AddListener(Quit);
    }

    void Update() {
       scoreText.text = gameUI.score.ToString(); 
    }

    void MainMenu() {
        deathMenu.SetActive(false);
        loadingMenu.SetActive(true);
        StartCoroutine(LoadAysnc(SceneManager.GetActiveScene().buildIndex));
    }

    void Quit() {
        Application.Quit();
    }

    IEnumerator LoadAysnc(int sceneIndex) {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

        while (!loadScene.isDone) {
            float progress = Mathf.Clamp01(loadScene.progress / 0.9f);
            loadingSlider.value = progress;

            yield return null;
        }
    }
}
