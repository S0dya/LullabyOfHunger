using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{
    [SerializeField] ActionsDictionary[] ActionsDictionaries;

    List<IObserver> _observers = new List<IObserver>();

    //local
    Dictionary<EnumsActions, UnityEvent> _actionDictionary = new Dictionary<EnumsActions, UnityEvent>();

    protected virtual void Awake()
    {
        foreach (ActionsDictionary kvp in ActionsDictionaries)
        {
            _actionDictionary.Add(kvp.SubjectEnum, kvp.Action);
        }
    }

    public void AddObserver(IObserver observer) => _observers.Add(observer);
    public void RemoveObserver(IObserver observer) => _observers.Remove(observer);
    public void NotifyObservers(EnumsActions actionEnum)
    {
        if (_actionDictionary.ContainsKey(actionEnum)) _actionDictionary[actionEnum].Invoke();
    }
}

[System.Serializable]
public class ActionsDictionary
{
    [SerializeField] public EnumsActions SubjectEnum;
    [SerializeField] public UnityEvent Action;
}
