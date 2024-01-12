using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{
    [SerializeField] ActionsDictionary[] ActionsDictionaries;

    List<IObserver> _observers = new List<IObserver>();

    //local
    Dictionary<SubjectActions, UnityEvent> _actionDictionary = new Dictionary<SubjectActions, UnityEvent>();

    protected virtual void Awake()
    {
        foreach (ActionsDictionary kvp in ActionsDictionaries)
        {
            _actionDictionary.Add(kvp.SubjectEnum, kvp.Action);
        }
    }

    public void AddObserver(IObserver observer) => _observers.Add(observer);
    public void RemoveObserver(IObserver observer) => _observers.Remove(observer);
    public void NotifyObservers(SubjectActions actionEnum)
    {
        if (_actionDictionary.ContainsKey(actionEnum)) _actionDictionary[actionEnum].Invoke();
    }
}

[System.Serializable]
class ActionsDictionary
{
    [SerializeField] public SubjectActions SubjectEnum;
    [SerializeField] public UnityEvent Action;
}

public enum SubjectActions
{
    InputLookAt,
}
