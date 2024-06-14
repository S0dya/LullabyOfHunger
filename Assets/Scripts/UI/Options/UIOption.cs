using System.Collections;
using UnityEngine;
using TMPro;


public class UIOption : MonoBehaviour
{
    [Header("option")]
    [SerializeField] bool SetsKey;

    //local
    [HideInInspector] public TextMeshProUGUI _optionText;

    //threshold
    [HideInInspector] public string _optionName;
    
    string _settingName;

    protected virtual void Awake()
    {
        _optionText = GetComponent<TextMeshProUGUI>();
    }

    //outside methods
    public virtual void AssignOption(string name)
    {
        _optionName = name;
    }

    public void SetOptionLine(string secondPart)
    {
        _settingName = secondPart; SetOptionLine();
    }

    public void SetOptionLine() => _optionText.text = GameManager.Instance.GetLocalizedString(_optionName) + " : " 
        + (SetsKey ? GameManager.Instance.GetLocalizedString(_settingName) : _settingName);
}
