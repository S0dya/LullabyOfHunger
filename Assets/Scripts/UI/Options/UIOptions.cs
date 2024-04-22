using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIOptions : UISingletonMonobehaviour<UIOptions>
{

    [SerializeField] OptionSettingText[] OptionSettingsTexts;
    [SerializeField] OptionSettingToggle[] OptionSettingsToggles;
    [SerializeField] OptionSettingSlider[] OptionSettingsSliders;

    //local
    Dictionary<string, OptionSettingText> _optionsSettingsTextsDict = new Dictionary<string, OptionSettingText>();
    Dictionary<string, OptionSettingToggle> _optionsSettingsTogglesDict = new Dictionary<string, OptionSettingToggle>();
    Dictionary<string, OptionSettingSlider> _optionsSettingsSlidersDict = new Dictionary<string, OptionSettingSlider>();

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

            option.UiOption.AssignOption(option.Name, option.OptionEvent);
        }
        foreach (var option in OptionSettingsSliders)
        {
            _optionsSettingsSlidersDict.Add(option.Name, option);

            option.UiOption.AssignOption(option.Name, option.OptionEvent);
        }
    }

    //outside methods
    //actions
    public void SetResolution(string str)
    {
        string[] resolutionParts = str.Split(" x ");
        if (resolutionParts.Length == 2 && int.TryParse(resolutionParts[0], out int width) && int.TryParse(resolutionParts[1], out int height))
            Screen.SetResolution(width, height, Screen.fullScreen);
    }

    //save
    public void SaveGameSettings()
    {
        foreach (var kvp in _optionsSettingsTextsDict) SaveInt(kvp.Key, kvp.Value.UiOption.GetOptionI());
        foreach (var kvp in _optionsSettingsTogglesDict) SaveBool(kvp.Key, kvp.Value.UiOption.GetToggle());
        foreach (var kvp in _optionsSettingsSlidersDict) SaveFloat(kvp.Key, kvp.Value.UiOption.GetSliderVal());

    }

    void LoadGameSettings()
    {
        foreach (var kvp in _optionsSettingsTextsDict) kvp.Value.UiOption.SetOptionI(LoadInt(kvp.Key));
        foreach (var kvp in _optionsSettingsTogglesDict) kvp.Value.UiOption.SetToggle(LoadBool(kvp.Key));
        foreach (var kvp in _optionsSettingsSlidersDict) kvp.Value.UiOption.SetSliderVal(LoadFloat(kvp.Key));

    }

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