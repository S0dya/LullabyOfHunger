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
    TextOption[] _options;

    int _optionsN;
    int _curOptionI;

    //props
    int optionI { get { return _curOptionI; } 
        set { _curOptionI = value;
            SetOptionLine(_options[_curOptionI].Line); } }

    //outside methods
    public void AssignOption(string name, TextOption[] options)
    {
        AssignOption(name);

        _options = options; _optionsN = _options.Length - 1;
    }

    public void SetOptionI(int i)
    {
        optionI = i;
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
            if (optionI == _optionsN) optionI = 0;
            else optionI++;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_curOptionI == 0) optionI = _optionsN;
            else optionI--;
        }
    }
}
