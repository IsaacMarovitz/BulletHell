using System.Collections.Generic;

[System.Serializable]
public class SaveData {
    public List<Run> runs = new List<Run>();
    public int highScoreIndex;
    public float totalPlayTime;
    public int[] resolutionDim = new int[2];
    public int refreshRate;
    public int qualityIndex;
    public bool vsyncEnabled;
    public bool isFullscreen;
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
}

[System.Serializable]
public struct Run {
    public int score;
    public float playTime;

    public Run(int _score, float _playTime) {
        score = _score;
        playTime = _playTime;
    }
}