using UnityEngine;
using UnityEngine.Audio;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    public static void Save(Run run, AudioMixer audioMixer) {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream stream = new FileStream(GetPath(), FileMode.Create);

        SaveData oldData = Load();
        SaveData data = new SaveData();
        if (oldData != null) {
            data.runs = oldData.runs;
            data.runs.Add(run);
            data.totalPlayTime = oldData.totalPlayTime + Time.realtimeSinceStartup;
        } else {
            data.runs = new List<Run>();
            data.runs.Add(run);
            data.totalPlayTime = Time.realtimeSinceStartup;
        }
        int highScoreIndex = 0;
        for (int i = 0; i < data.runs.Count; i++) {
            if (data.runs[i].score > data.runs[highScoreIndex].score) {
                highScoreIndex = i;
            }
        }
        data.highScoreIndex = highScoreIndex;
        Resolution currentResolution = Screen.currentResolution;
        data.resolutionDim[0] = currentResolution.width;
        data.resolutionDim[1] = currentResolution.height;
        data.refreshRate = currentResolution.refreshRate;
        data.isFullscreen = Screen.fullScreen;

        audioMixer.GetFloat("MasterVolume", out data.masterVolume);
        audioMixer.GetFloat("MusicVolume", out data.musicVolume);
        audioMixer.GetFloat("SFXVolume", out data.sfxVolume);

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
            Debug.Log("<b>Save System:</b> No save data found!");
            return null;
        }
    }

    static string GetPath() {
        return Application.persistentDataPath + "SaveData.bh";
    }
}