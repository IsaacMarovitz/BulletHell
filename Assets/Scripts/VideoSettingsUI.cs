using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoSettingsUI : MonoBehaviour {

    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle vsyncToggle;
    public Toggle fullscreenToggle;
    public Animator videoSettingsAnimator;
    public Animator previousMenuAnimator;
    public Button backButton;

    Resolution[] resolutions;
    List<Resolution> dropdownResolutions = new List<Resolution>();

    public void Start() {
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
        resolutionDropdown.value = options.Count-1;
        SetResolution(options.Count-1);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.onValueChanged.AddListener(SetQuality);
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

    public void Back() {
        videoSettingsAnimator.Play("Fade & Slide Out");
        previousMenuAnimator.Play("Fade & Slide In");
    }
}
