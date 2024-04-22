using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using static UnityEngine.InputSystem.PlayerInput;

public class UIOptionText : UIOption, IPointerClickHandler
{

    //local 
    Button _button;

    TextOption[] _options;

    int _curOptionI;

    //props
    int optionI { get { return _curOptionI; } 
        set { _curOptionI = value;
            SetOptionLine(_options[_curOptionI].Line); } }

    protected override void Awake()
    {
        base.Awake();

        _button = GetComponent<Button>();
    }

    //outside methods
    public void AssignOption(string name, TextOption[] options)
    {
        AssignOption(name);

        _options = options;
    }

    public void SetOptionI(int i)
    {
        optionI = i; InvokeEvent();
    }

    public int GetOptionI()
    {
        return optionI;
    }

    //actions
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (optionI > _options.Length) optionI = 0;
            else optionI++;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_curOptionI == 0) optionI = _options.Length;
            else optionI--;
        }

        InvokeEvent();
    }

    //other methods
    void InvokeEvent() => _options[_curOptionI].OptionEvent.Invoke(_options[_curOptionI].Line);
}
