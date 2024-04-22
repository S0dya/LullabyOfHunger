using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIOption : MonoBehaviour
{
    //local
    [HideInInspector] public TextMeshProUGUI _optionText;

    //threshold
    [HideInInspector] public string _optionName;


    protected virtual void Awake()
    {
        _optionText = GetComponent<TextMeshProUGUI>();
    }

    //outside methods
    public virtual void AssignOption(string name)
    {
        _optionName = name;
    }

    public void SetOptionLine(string secondPart) => _optionText.text = _optionName + " : " + secondPart;
}
