using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIOptionSlider : UIOption
{

    //local 
    Slider _slider;

    UnityEvent<float> _optionEvent;

    float _curVal;

    void Awake()
    {
        //_optionName = 

        _slider = GetComponent<Slider>();

        _slider.onValueChanged.AddListener(OnValueChanged);
    }

    //outside methods
    public void AssignOption(string name, UnityEvent<float> optionEvent)
    {
        AssignOption(name); SetOptionLine("");

        _optionEvent = optionEvent;
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

        InvokeEvent();
    }

    //other methods
    void InvokeEvent() => _optionEvent.Invoke(_curVal);
}
