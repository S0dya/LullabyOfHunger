using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIOptionToggle : UIOption
{
    //local 
    Button _button;

    UnityEvent<bool> _optionEvent;

    bool _curToggle;

    //props 
    bool Toggle { get { return _curToggle; } set { _curToggle = value; SetOptionLine(_curToggle ? "On" : "Off" ); } }

    protected override void Awake()
    {
        base.Awake();

        _button = GetComponent<Button>();

        _button.onClick.AddListener(OnClick);
    }

    //outside methods
    public void AssignOption(string name, UnityEvent<bool> optionEvent)
    {
        AssignOption(name);

        _optionEvent = optionEvent;
    }

    public void SetToggle(bool toggle)
    {
        Toggle = toggle;

        InvokeEvent();
    }

    public bool GetToggle()
    {
        return Toggle;
    }

    //actions 
    void OnClick()
    {
        Toggle = !Toggle;

        InvokeEvent();
    }

    //other methods
    void InvokeEvent() => _optionEvent.Invoke(Toggle);
}
