using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class MenuUI : MonoBehaviour {
    
    public EnemySettings enemySettings;
    public CinemachineVirtualCamera uiCamera;
    public Button newGameButton;
    public Button quitButton;
    public PlayerMovement playerMovement;
    public Animator animator;
    public GameObject gameUI;

    float targetWeight;

    void Start() {
        newGameButton.onClick.AddListener(NewGame);
        quitButton.onClick.AddListener(Quit);
        targetWeight = enemySettings.targetWeight;
        enemySettings.targetWeight = 0;
    }

    public void NewGame() {
        uiCamera.Priority = 0;
        animator.Play("Fade Out");
        playerMovement.move = true;
        enemySettings.targetWeight = targetWeight;
        gameUI.SetActive(true);
    }

    public void Quit() {
        Application.Quit();
    }
}
