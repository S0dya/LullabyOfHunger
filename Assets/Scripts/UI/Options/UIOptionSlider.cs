using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIOptionSlider : UIOption
{

    //local 
    Slider _slider;

    float _curVal;

    new void Awake()
    {
        _optionText = GetComponentInChildren<TextMeshProUGUI>();
        _slider = GetComponentInChildren<Slider>();

        _slider.onValueChanged.AddListener(OnValueChanged);
    }

    //outside methods
    public override void AssignOption(string name)
    {
        base.AssignOption(name); SetOptionLine("");
    }

    public void SetSliderVal(float val)
    {
        _slider.value = val;
    }

    public float GetSliderVal()
    {
        return _curVal;
    }

    //actions
    public void OnValueChanged(float val)
    {
        _curVal = val;
    }
}
