using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ActionsDictionary
{
    [SerializeField] public EnumsActions ObserverEnum;
    [SerializeField] public UnityEvent Event;
}
