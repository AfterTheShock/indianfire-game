using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [Header("PlayerSettings")]
    public bool vsync = true;
    public int resolutionIndex = 0;
    public bool fullscreen = true;
    public int qualityIndex = 1;

    [Header("PlayerSoundSetting")]
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public float ambianceVolume = 1f;



    public PlayerData(DataToSave data)
    {
        //PlayerSettings
        vsync = data.vsync;
        resolutionIndex = data.resolutionIndex;
        fullscreen = data.fullscreen;
        qualityIndex = data.qualityIndex;

        //PlayerSoundSetting
        masterVolume = data.masterVolume;
        musicVolume = data.musicVolume;
        sfxVolume = data.sfxVolume;
        ambianceVolume = data.ambianceVolume;
    }

}
