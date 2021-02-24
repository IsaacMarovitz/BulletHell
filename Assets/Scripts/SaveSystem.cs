using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {

    public static void SaveResolution() {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        SaveData oldData = Load();
        FileStream stream = new FileStream(GetPath(), FileMode.Create);

        SaveData data = new SaveData();

        if (oldData != null) {
            data = oldData;
        }

        data.resolutionDim[0] = Screen.currentResolution.width;
        data.resolutionDim[1] = Screen.currentResolution.height;
        data.refreshRate = Screen.currentResolution.refreshRate;
        data.qualityIndex = QualitySettings.GetQualityLevel();
        data.vsyncEnabled = QualitySettings.vSyncCount > 0 ? true : false;
        data.isFullscreen = Screen.fullScreen;
        
        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveAudioSettings(float masterVolume, float sfxVolume, float musicVolume) {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        SaveData oldData = Load();
        FileStream stream = new FileStream(GetPath(), FileMode.Create);

        SaveData data = new SaveData();

        if (oldData != null) {
            data = oldData;
        }

        data.masterVolume = masterVolume;
        data.sfxVolume = sfxVolume;
        data.musicVolume = musicVolume;
        
        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveRun(Run run) {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        SaveData oldData = Load();
        FileStream stream = new FileStream(GetPath(), FileMode.Create);

        SaveData data = new SaveData();

        if (oldData != null) {
            data = oldData;
        }

        data.runs.Add(run);
        int highScoreIndex = 0;
        for (int i = 0; i < data.runs.Count; i++) {
            if (data.runs[i].score > data.runs[highScoreIndex].score) {
                highScoreIndex = i;
            }
        }
        data.highScoreIndex = highScoreIndex;
        
        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveTime() {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        SaveData oldData = Load();
        FileStream stream = new FileStream(GetPath(), FileMode.Create);

        SaveData data = new SaveData();

        if (oldData != null) {
            data = oldData;
        }

        data.totalPlayTime += Time.realtimeSinceStartup;
        
        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData Load() {
        if (File.Exists(GetPath())) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(GetPath(), FileMode.Open);

            SaveData data = (SaveData)binaryFormatter.Deserialize(stream);
            stream.Close();
            return data;
        } else {
            //Debug.Log("<b>Save System:</b> No save data found!");
            return null;
        }
    }

    static string GetPath() {
        return Path.Combine(Application.persistentDataPath, "SaveData.bh");
    }
}