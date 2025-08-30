
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class OptionsMenuManager : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider ambianceSlider;
    [SerializeField] Slider masterSlider;

    [SerializeField] GameObject firstSelectedButtonOnStart;

    [SerializeField] GameObject[] allOptionsBrackgrounds = new GameObject[0];

    [Header("GraphicsStuff")]
    private Resolution[] resolutions;
    [SerializeField] Toggle vsyncToggle;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] TMP_Dropdown qualityDropdown;

    #region singletonPatern
    private static OptionsMenuManager _instance;
    public static OptionsMenuManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindFirstObjectByType<OptionsMenuManager>();
            }
            return _instance;
        }
    }
    #endregion

    private void Start()
    {
        InicializeResolutionParameters();

        SetSlidersValuesToSavedData();
    }

    private void InicializeResolutionParameters()
    {
        //Get resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        //Reversing resolutions so they are ordered properly
        Resolution[] tempArray = new Resolution[resolutions.Length];
        for (int i = 0; i < resolutions.Length; i++)
        {
            tempArray[resolutions.Length - 1 - i] = resolutions[i];
        }
        resolutions = tempArray;

        //Set the dropdown with the possible resolutions
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        //Add the resolutions to the dropdown
        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);

        //Save data
        DataToSave.Instance.resolutionIndex = resolutionIndex;
        DataToSave.Instance.SaveData();

        //Update drowdown
        resolutionDropdown.value = resolutionIndex;
    }

    public void ToggleVsync(bool vsyncEnabled)
    {   
        QualitySettings.vSyncCount = (vsyncEnabled ? 1 : 0);

        //Save data
        DataToSave.Instance.vsync = vsyncEnabled;
        DataToSave.Instance.SaveData();

        //Update toggle
        vsyncToggle.isOn = vsyncEnabled;
    }

    public void SetQualityLevel(int qualityIndex)
    {
        if (QualitySettings.count <= qualityIndex) return;

        QualitySettings.SetQualityLevel(qualityIndex);

        //Save data
        DataToSave.Instance.qualityIndex = qualityIndex;
        DataToSave.Instance.SaveData();

        //Update drowdown
        qualityDropdown.value = qualityIndex;
    }

    public void ToggleFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        //Save data
        DataToSave.Instance.fullscreen = isFullScreen;
        DataToSave.Instance.SaveData();

        //Update toggle
        fullscreenToggle.isOn = isFullScreen;
    }

    private void OnEnable()
    {
        //Initialize a selected button to navegate with arrows
        EventSystem.current.SetSelectedGameObject(firstSelectedButtonOnStart);

        SetSlidersValuesToSavedData();
    }

    private void Update()
    {
        //Always have a selected ui element so you can navegate with the arrows or joistick
        if (EventSystem.current.currentSelectedGameObject == null) EventSystem.current.SetSelectedGameObject(firstSelectedButtonOnStart);
    }

    public void OnChangeMusicSlider()
    {

        float volume = ConvertDromDBtoLinearValue(musicSlider.value); // Convert from dB to linear for slider

        masterMixer.SetFloat("MusicVolume", volume);

        DataToSave.Instance.musicVolume = volume;
        ChangedVolume();
    }

    public void OnChangeSFXSlider()
    {

        float volume = ConvertDromDBtoLinearValue(SFXSlider.value); // Convert from dB to linear for slider

        masterMixer.SetFloat("SFXVolume", volume);

        DataToSave.Instance.sfxVolume = volume;
        ChangedVolume();
    }

    public void OnChangeAmbianceSlider()
    {

        float volume = ConvertDromDBtoLinearValue(ambianceSlider.value); // Convert from dB to linear for slider

        masterMixer.SetFloat("AmbianceVolume", volume);

        DataToSave.Instance.ambianceVolume = volume;
        ChangedVolume();
    }

    public void OnChangeMasterSlider()
    {

        float volume = ConvertDromDBtoLinearValue(masterSlider.value); // Convert from dB to linear for slider

        masterMixer.SetFloat("MasterVolume", volume);

        DataToSave.Instance.masterVolume = volume;
        ChangedVolume();
    }

    private void ChangedVolume()
    {
        DataToSave.Instance.SaveData();
    }

    private float ConvertDromDBtoLinearValue(float valueInDB)
    {
        return Mathf.Log10(valueInDB) * 20;
    }

    private void SetSlidersValuesToSavedData()
    {
        masterSlider.value = Mathf.Pow(10, DataToSave.Instance.masterVolume / 20);
        musicSlider.value = Mathf.Pow(10, DataToSave.Instance.musicVolume / 20);
        SFXSlider.value = Mathf.Pow(10, DataToSave.Instance.sfxVolume / 20);
        ambianceSlider.value = Mathf.Pow(10, DataToSave.Instance.ambianceVolume / 20);
    }

    public void TurnOffAllOptionsBackgrounds()
    {
        foreach(GameObject o in allOptionsBrackgrounds)
        {
            o.SetActive(false);
        }
    }
}
