using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettingsUI : MonoBehaviour {

    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;
    public Animator audioSettingsAnimator;
    public SettingsUI settingsUI;
    public Button backButton;

    void Start() {
        masterSlider.onValueChanged.AddListener(SetMaster);
        sfxSlider.onValueChanged.AddListener(SetSFX);
        musicSlider.onValueChanged.AddListener(SetMusic);
        backButton.onClick.AddListener(Back);
    }

    public void SetMaster(float value) {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetSFX(float value) {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);

    }

    public void SetMusic(float value) {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void Back() {
        audioSettingsAnimator.Play("Fade & Slide Out");
        settingsUI.animator.Play("Fade & Slide In");
        settingsUI.currentSettingsMenu = CurrentSettingsMenu.Main;
    }
}
