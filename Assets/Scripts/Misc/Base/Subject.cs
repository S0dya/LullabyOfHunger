using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Subject : MonoBehaviour
{
    [SerializeField] ActionsDictionary[] ActionsDictionaries;

    //local
    Dictionary<EnumsActions, Action> _actionDictionary = new Dictionary<EnumsActions, Action>();

    public void AddAction(EnumsActions enumAction, Action action) => _actionDictionary.Add(enumAction, action);

    protected virtual void Awake()
    {
        foreach (ActionsDictionary kvp in ActionsDictionaries)
        {
            EventToAction wrapper = new EventToAction();
            wrapper.WrapEvent(kvp.Event);

            _actionDictionary.Add(kvp.ObserverEnum, wrapper.InvokeEvent);
        }
    }

    protected virtual void OnEnable()
    {
        Observer.Instance.AddObserver(this);
    }
    protected virtual void OnDisable()
    {
        Observer.Instance.RemoveObserver(this);
    }

    public virtual void PerformAction(EnumsActions actionEnum)
    {
        if (_actionDictionary.ContainsKey(actionEnum)) _actionDictionary[actionEnum].Invoke();
    }

    public void NotifyObserver(EnumsActions actionEnum)
    {
        Observer.Instance.NotifyObservers(actionEnum);
    }
}
