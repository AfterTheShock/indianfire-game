
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class DataToSave : MonoBehaviour
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

    [SerializeField] bool deletePlayerDataButton = false;

    [Header("OtherNecessities")]
    private bool dataLoaded = false;
    [SerializeField] AudioMixer masterMixer;

    #region singletonPatern
    private static DataToSave _instance;
    public static DataToSave Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindFirstObjectByType<DataToSave>();
            }
            return _instance;
        }
    }
    #endregion

    public void LoadData()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data == null) return;

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

        dataLoaded = true;
    }

    private void Update()
    {
        if (deletePlayerDataButton)
        {
            deletePlayerDataButton = false;
            DeletePlayerData();
        }
    }

    private void Awake()
    {
        LoadData();

    }
    private void Start()
    {
        if (!dataLoaded) LoadData();

        SetVolumesWithCurrentData();

        InicializePlayerSettings();
    }

    private void InicializePlayerSettings()
    {
        OptionsMenuManager.Instance.ToggleVsync(vsync);
        OptionsMenuManager.Instance.SetResolution(resolutionIndex);
        OptionsMenuManager.Instance.ToggleFullScreen(fullscreen);
        OptionsMenuManager.Instance.SetQualityLevel(qualityIndex);
    }

    public void SaveData()
    {
        SaveSystem.SaveData(this);
        SetVolumesWithCurrentData();
    }

    private void SetVolumesWithCurrentData()
    {
        masterMixer.SetFloat("MasterVolume", masterVolume);
        masterMixer.SetFloat("MusicVolume", musicVolume);
        masterMixer.SetFloat("SFXVolume", sfxVolume);
        masterMixer.SetFloat("AmbianceVolume", ambianceVolume);
    }

    public void DeletePlayerData()
    {
        SaveSystem.DeletePlayerData();
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
