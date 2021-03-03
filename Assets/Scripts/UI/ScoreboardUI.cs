using UnityEngine;
using UnityEngine.UI;

public class ScoreboardUI : UIMenuBase {

    public MenuUI menuUI;
    public Button backButton;
    public Transform scoreboardEntryParent;
    public GameObject scoreboardEntryPrefab;
    public Color primaryColor;
    public Color secondaryColor;

    // Load run adata and populate the scoreboard table with the runs
    public override void Start() {
        base.Start();
        backButton.onClick.AddListener(Back);

        SaveData saveData = SaveSystem.Load();
        if (saveData != null) {
            for (int i = 0; i < saveData.runs.Count; i++) {
                GameObject instantiatedEntry = GameObject.Instantiate(scoreboardEntryPrefab, Vector3.zero, Quaternion.identity);
                instantiatedEntry.transform.SetParent(scoreboardEntryParent, false);

                ScoreboardEntry scoreboardEntry = instantiatedEntry.GetComponent<ScoreboardEntry>();
                scoreboardEntry.rankText.text = (i + 1) + ".";
                scoreboardEntry.scoreText.text = saveData.runs[i].score.ToString();
                scoreboardEntry.timeText.text = SecondsToString(saveData.runs[i].playTime);
                scoreboardEntry.background.color = (i % 2 == 0) ? primaryColor : secondaryColor;
            }
        }
    }

    public override void OpenMenu() {
        base.OpenMenu();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(backButton.gameObject);
    }

    void Back() {
        animator.Play("Fade & Slide Out");
        menuUI.animator.Play("Fade & Slide In");
        menuUI.currentMenu = CurrentMenu.Main;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(menuUI.scoreboardButton.gameObject);
    }

    // Helper function to convert strings to a 02:30 style string of minutes and seconds
    string SecondsToString(float seconds) {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int remainingSeconds = Mathf.RoundToInt(seconds) - minutes * 60;
        string secondsText;
        if (remainingSeconds < 10) {
            secondsText = "0" + remainingSeconds;
        } else {
            secondsText = remainingSeconds.ToString();
        }
        return minutes + ":" + secondsText;
    }
}
