using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettingsUI : UIMenuBase {

    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;
    public SettingsUI settingsUI;
    public Button backButton;

    // Set up slider delagate and load in saved audio values
    public override void Start() {
        base.Start();
        masterSlider.onValueChanged.AddListener(SetMaster);
        sfxSlider.onValueChanged.AddListener(SetSFX);
        musicSlider.onValueChanged.AddListener(SetMusic);
        backButton.onClick.AddListener(Back);

        SaveData saveData = SaveSystem.Load();
        if (saveData != null) {
            masterSlider.value = saveData.masterVolume;
            sfxSlider.value = saveData.sfxVolume;
            musicSlider.value = saveData.musicVolume;

            SetMaster(saveData.masterVolume);
            SetSFX(saveData.sfxVolume);
            SetMusic(saveData.musicVolume);
        }
    }

    public override void OpenMenu() {
        base.OpenMenu();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(masterSlider.gameObject);
    }

    // Set the value of the master volume converting linear slider to logarithmic decibels
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
        animator.Play("Fade & Slide Out");
        settingsUI.animator.Play("Fade & Slide In");
        settingsUI.currentSettingsMenu = CurrentSettingsMenu.Main;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(settingsUI.audioSettingsButton.gameObject);
        SaveSystem.SaveAudioSettings(masterSlider.value, sfxSlider.value, musicSlider.value);
    }
}
