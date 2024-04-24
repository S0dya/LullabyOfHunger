using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIOptionToggle : UIOption
{
    public bool DefaultVal;

    //local 
    Button _button;

    bool _curToggle;

    //props 
    bool Toggle { get { return _curToggle; } set { _curToggle = value; SetOptionLine(_curToggle ? "On" : "Off" ); } }

    protected override void Awake()
    {
        base.Awake();

        _button = GetComponent<Button>();

        _button.onClick.AddListener(OnClick);
        Toggle = DefaultVal;
    }

    public void SetToggle(bool toggle)
    {
        Toggle = toggle;
    }

    public bool GetToggle()
    {
        return Toggle;
    }

    //actions 
    void OnClick()
    {
        SetToggle(!Toggle);
    }
}
