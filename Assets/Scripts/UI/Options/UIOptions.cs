using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class UIOptions : UISingletonMonobehaviour<UIOptions>
{
    [Header("UI panels")]
    [SerializeField] GameObject[] PanelsObjs;

    [SerializeField] Image[] PanelButtons;
    [SerializeField] Color HighlightedColor;
    [SerializeField] Color NormalColor;

    [Header("Options")]
    [SerializeField] OptionSettingText[] OptionSettingsTexts;
    [SerializeField] OptionSettingToggle[] OptionSettingsToggles;
    [SerializeField] OptionSettingSlider[] OptionSettingsSliders;

    //local
    Dictionary<string, OptionSettingText> _optionsSettingsTextsDict = new Dictionary<string, OptionSettingText>();
    Dictionary<string, OptionSettingToggle> _optionsSettingsTogglesDict = new Dictionary<string, OptionSettingToggle>();
    Dictionary<string, OptionSettingSlider> _optionsSettingsSlidersDict = new Dictionary<string, OptionSettingSlider>();

    PostProcessVolume _postProcessVol;

    protected override void Awake()
    {
        base.Awake();

        foreach (var option in OptionSettingsTexts)
        {
            _optionsSettingsTextsDict.Add(option.Name, option);

            option.UiOption.AssignOption(option.Name, option.Options);
        }
        foreach (var option in OptionSettingsToggles)
        {
            _optionsSettingsTogglesDict.Add(option.Name, option);

            option.UiOption.AssignOption(option.Name);
        }
        foreach (var option in OptionSettingsSliders)
        {
            _optionsSettingsSlidersDict.Add(option.Name, option);

            option.UiOption.AssignOption(option.Name);
        }

        _postProcessVol = FindObjectOfType<PostProcessVolume>();
    }

    void Start()
    {
        LoadGameSettings();
    }

    //outside methods
    public void OpenOptions()
    {
        ButtonOpenTab(0); LoadGameSettings();
        SwitchCG(1);
    }

    public void CloseOptions()
    {
        SwitchCG(0);
    }

    //buttons 
    public void ButtonApplyChanges()
    {
        SaveGameSettings(); CloseOptions();
    }

    public void ButtonOpenTab(int i)
    {
        SetCurPanel(i);
    }

    //actions 
    //text
    public void SetResolution(string str)
    {
        string[] resolutionParts = str.Split(" x ");
        if (resolutionParts.Length == 2 && int.TryParse(resolutionParts[0], out int width) && int.TryParse(resolutionParts[1], out int height))
        {
            Screen.SetResolution(width, height, Screen.fullScreen);
        }
    }

    public void SetWindowMode(string str)
    {
        Screen.fullScreen = str == "Windowed";
    }

    public void SetQuality(string str)
    {
        QualitySettings.SetQualityLevel(str == "Low" ? 0 : str == "Medium" ? 1 : 2);
    }

    //toggle
    public void ShowBlood(bool toggle)
    {
        Settings.showBlood = toggle;
    }

    //slider
    public void ChangeAmbientVol(float val) => ChangeVol(0, val);
    public void ChangeMusicVol(float val) =>ChangeVol(1, val);
    public void ChangeSFXVol(float val) => ChangeVol(2, val);
    public void ChangeFov(float val)
    {
        Settings.camFov = val;

        if (LevelManager.Instance != null) LevelManager.Instance.SetCamsFov();
    }
    public void ChangeGamma(float val)
    {
        GetColorGrading().gamma.value = new Vector4(val, val, val, val);
    }
    public void ChangeContrast(float val)
    {
        GetColorGrading().contrast.value = val;
    }
    public void ChangeBrightness(float val)
    {
        GetColorGrading().postExposure.value = val;
    }

    public void ChangeFPSensitivity(float val)
    {
        if (Player.Instance != null) Player.Instance.SensitivityFirstPerson = val;
    }
    public void ChangeReloadingSensitivity(float val)
    {
        if (Player.Instance != null) Player.Instance.SensitivityInReloading = val;
    }

    //other
    void SetCurPanel(int indexToTrue)
    {
        for (int i = 0; i < PanelsObjs.Length; i++)
        {
            PanelsObjs[i].SetActive(i == indexToTrue);
            PanelButtons[i].color = i == indexToTrue ? HighlightedColor : NormalColor;
        }
    }

    void ChangeVol(int i, float val) => AudioManager.Instance.SetVolume(i, val);

    ColorGrading GetColorGrading()
    {
        ColorGrading colorGrading;
        _postProcessVol.profile.TryGetSettings(out colorGrading);

        return colorGrading;
    }

    //save
    public void SaveGameSettings()
    {
        foreach (var kvp in _optionsSettingsTextsDict)
        {
            var val = kvp.Value.UiOption.GetOptionI(); SaveInt(kvp.Key, val);

            kvp.Value.Options[val].OptionEvent.Invoke(kvp.Value.Options[val].Line);
        }
        foreach (var kvp in _optionsSettingsTogglesDict)
        {
            var val = kvp.Value.UiOption.GetToggle(); SaveBool(kvp.Key, val);

            kvp.Value.OptionEvent.Invoke(val);
        }
        foreach (var kvp in _optionsSettingsSlidersDict)
        {
            var val = kvp.Value.UiOption.GetSliderVal(); SaveFloat(kvp.Key, val);

            kvp.Value.OptionEvent.Invoke(val);
        }
    }

    void LoadGameSettings()
    {
        foreach (var kvp in _optionsSettingsTextsDict) kvp.Value.UiOption.SetOptionI(LoadInt(kvp.Key));
        foreach (var kvp in _optionsSettingsTogglesDict) kvp.Value.UiOption.SetToggle(LoadBool(kvp.Key));
        foreach (var kvp in _optionsSettingsSlidersDict) kvp.Value.UiOption.SetSliderVal(LoadFloat(kvp.Key));

    }

    //other outisde methods
    public void PlayHeaderButtonSound() => AudioManager.Instance.PlayOneShot("OptionsHeaderButtonClick");

    //other 
    void SaveInt(string name, int val) => PlayerPrefs.SetInt(name, val);
    void SaveString(string name, string val) => PlayerPrefs.SetString(name, val);
    void SaveFloat(string name, float val) => PlayerPrefs.SetFloat(name, val);
    void SaveBool(string name, bool val) => PlayerPrefs.SetInt(name, val ? 0 : 1);

    int LoadInt(string name)
    {
        return PlayerPrefs.GetInt(name);
    }
    string LoadString(string name)
    {
        return PlayerPrefs.GetString(name);
    }
    float LoadFloat(string name)
    {
        return PlayerPrefs.GetFloat(name);
    }
    bool LoadBool(string name)
    {
        return PlayerPrefs.GetInt(name) == 0;
    }
}

[System.Serializable]
public class OptionSettingToggle : OptionSetting
{
    [SerializeField] public UIOptionToggle UiOption;
    [SerializeField] public UnityEvent<bool> OptionEvent;
}

[System.Serializable]
public class OptionSettingSlider : OptionSetting
{
    [SerializeField] public UIOptionSlider UiOption;
    [SerializeField] public UnityEvent<float> OptionEvent;
}

[System.Serializable]
public class OptionSettingText : OptionSetting
{
    [SerializeField] public UIOptionText UiOption;
    [SerializeField] public TextOption[] Options;
}
[System.Serializable]
public class TextOption
{
    [SerializeField] public string Line;
    [SerializeField] public UnityEvent<string> OptionEvent;
}

public class OptionSetting
{
    [SerializeField] public string Name;
}