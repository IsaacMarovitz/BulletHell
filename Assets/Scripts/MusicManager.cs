using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public AudioClip[] mainMenuMusic;
    public AudioClip[] levelOneMusic;
    public AudioClip[] currentMusic;
    public AudioSource musicPlayer;
    public AudioMixer audioMixer;
    public float timeToStartFade;
    public float fadeDuration;
    public AnimationCurve fadeOutCurve;
    public AnimationCurve fadeInCurve;
    public float lowpassFadeDiraton;
    public AnimationCurve fadeLowpassOutCurve;
    public AnimationCurve fadeLowpassInCurve;

    float currentClipPlaybackTime;
    bool nextSongStarted = false;

    void Start() {
        mainMenuMusic = Resources.LoadAll<AudioClip>("Music");
        levelOneMusic = Resources.LoadAll<AudioClip>("Level 1");
        currentMusic = mainMenuMusic;
        int randomIndex = Random.Range(0, currentMusic.Length);
        musicPlayer.clip = currentMusic[randomIndex];
        musicPlayer.Play(); 
        musicPlayer.volume = 0;
        currentClipPlaybackTime = currentMusic[randomIndex].length;
        StartCoroutine(FadeIn());
        audioMixer.SetFloat("MusicLowpassCutoff", 22000);
    }

    void Update() {
        if (musicPlayer.time >= (currentClipPlaybackTime - timeToStartFade) && !nextSongStarted) {
            nextSongStarted = true;
            StartCoroutine(StartNextSong());
        }
    }

    public void ChangeAudioState(AudioClip[] newMusic) {
        currentMusic = newMusic;
        StartCoroutine(StartNextSong());
    }

    public void Pause() {
        StartCoroutine(FadeLowpassOut());
    }

    public void Resume() {
        StartCoroutine(FadeLowpassIn());
    }

    public IEnumerator StartNextSong() {
        float currentTime = 0;
        while (currentTime < fadeDuration) {
            currentTime += Time.unscaledDeltaTime;
            float pointAlongCurve = Mathf.Lerp(0, 1, currentTime / fadeDuration);
            musicPlayer.volume = fadeOutCurve.Evaluate(pointAlongCurve);
            yield return null;
        }
        int randomIndex = Random.Range(0, currentMusic.Length);
        musicPlayer.clip = currentMusic[randomIndex];
        musicPlayer.Play(); 
        currentClipPlaybackTime = currentMusic[randomIndex].length;
        nextSongStarted = false;
        yield return StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn() {
        float currentTime = 0;
        while (currentTime < fadeDuration) {
            currentTime += Time.fixedUnscaledDeltaTime;
            float pointAlongCurve = Mathf.Lerp(0, 1, currentTime / fadeDuration);
            musicPlayer.volume = fadeInCurve.Evaluate(pointAlongCurve);
            yield return null;
        }
        yield break;
    }

    public IEnumerator FadeLowpassIn() {
        float currentTime = 0;
        while (currentTime < lowpassFadeDiraton) {
            currentTime += Time.fixedUnscaledDeltaTime;
            float pointAlongCurve = Mathf.Lerp(0, 1, currentTime / lowpassFadeDiraton);
            audioMixer.SetFloat("MusicLowpassCutoff", fadeLowpassInCurve.Evaluate(pointAlongCurve) * 21700 + 300);
            yield return null;
        }
        yield break;
    }

    public IEnumerator FadeLowpassOut() {
        float currentTime = 0;
        while (currentTime < lowpassFadeDiraton) {
            currentTime += Time.fixedUnscaledDeltaTime;
            float pointAlongCurve = Mathf.Lerp(0, 1, currentTime / lowpassFadeDiraton);
            audioMixer.SetFloat("MusicLowpassCutoff", fadeLowpassOutCurve.Evaluate(pointAlongCurve) * 21700 + 300);
            yield return null;
        }
        yield break;
    }
}
