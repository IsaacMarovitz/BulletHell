using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoSettingsUI : UIMenuBase {

    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle vsyncToggle;
    public Toggle fullscreenToggle;
    public SettingsUI settingsUI;
    public Button backButton;

    Resolution[] resolutions;
    List<Resolution> dropdownResolutions = new List<Resolution>();

    public override void Start() {
        base.Start();
        #if UNITY_IOS
            Application.targetFrameRate = 60;
            resolutionDropdown.transform.parent.gameObject.SetActive(false);
            vsyncToggle.transform.parent.gameObject.SetActive(false);
            fullscreenToggle.transform.parent.gameObject.SetActive(false);
        #endif
        resolutions = Screen.resolutions;
        backButton.onClick.AddListener(Back);

        if (QualitySettings.vSyncCount > 0) {
            vsyncToggle.isOn = true;
        } else {
            vsyncToggle.isOn = false;
        }
        vsyncToggle.onValueChanged.AddListener(VSync);

        fullscreenToggle.isOn = Screen.fullScreen;
        fullscreenToggle.onValueChanged.AddListener(Fullscreen);

        resolutionDropdown.ClearOptions();
        int maxRefreshRate = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            if (resolutions[i].refreshRate > maxRefreshRate) {
                maxRefreshRate = resolutions[i].refreshRate;
            }
        }
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++) {
            if (resolutions[i].refreshRate == maxRefreshRate) {
                dropdownResolutions.Add(resolutions[i]);
                options.Add(ResolutionToString(resolutions[i]));
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.onValueChanged.AddListener(SetQuality);

        SaveData saveData = SaveSystem.Load();
        if (saveData != null) {
            int index = resolutionDropdown.options.FindIndex((i) => { return i.text.Equals(SaveResolutionToString(saveData.resolutionDim)); });
            if (index >= 0) {
                SetResolution(index);
                resolutionDropdown.value = index;
            } else {
                Debug.Log("<b>Video Settings:</b> Saved resolution not found!");
            }
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, Screen.fullScreen, saveData.refreshRate);
            qualityDropdown.value = saveData.qualityIndex;
            QualitySettings.SetQualityLevel(saveData.qualityIndex);
            vsyncToggle.isOn = saveData.vsyncEnabled;
            QualitySettings.vSyncCount = saveData.vsyncEnabled ? 1 : 0;
            fullscreenToggle.isOn = saveData.isFullscreen;
            Screen.fullScreen = saveData.isFullscreen;
        } else {
            SetResolution(options.Count-1);
            resolutionDropdown.value = options.Count-1;
        }
    }

    public override void OpenMenu() {
        base.OpenMenu();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(resolutionDropdown.gameObject);
    }

    public void VSync(bool value) {
        if (value) {
            QualitySettings.vSyncCount = 1;
        } else {
            QualitySettings.vSyncCount = 0;
        }
    }

    public void Fullscreen(bool value) {
        Screen.fullScreen = value;
    }

    public void SetResolution(int i) {
        Screen.SetResolution(dropdownResolutions[i].width, dropdownResolutions[i].height, Screen.fullScreen);
    }

    public void SetQuality(int i) {
        QualitySettings.SetQualityLevel(i);
    }

    string ResolutionToString(Resolution resolution) {
        return  resolution.width + " x " + resolution.height;
    }

    string SaveResolutionToString(int[] resolution) {
        return resolution[0] + " x " + resolution[1];
    }

    public void Back() {
        animator.Play("Fade & Slide Out");
        settingsUI.animator.Play("Fade & Slide In");
        settingsUI.currentSettingsMenu = CurrentSettingsMenu.Main;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(settingsUI.videoSettingsButton.gameObject);
        SaveSystem.SaveResolution();
    }
}
