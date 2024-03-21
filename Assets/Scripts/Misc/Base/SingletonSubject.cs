using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SingletonSubject<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;
    public static T Instance { get { return instance; } }

    [SerializeField] ActionsDictionary[] ActionsDictionaries;

    //local
    Dictionary<EnumsActions, Action> _actionDictionary = new Dictionary<EnumsActions, Action>();

    public void CreateInstance()
    {
        if (instance == null) instance = this as T;
        else Debug.Log(gameObject.transform + " duplicated");
    }

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
        Observer.OnNotifyObservers += PerformAction;
    }
    protected virtual void OnDisable()
    {
        Observer.OnNotifyObservers -= PerformAction;
    }

    public void AddAction(EnumsActions enumAction, Action action) => _actionDictionary.Add(enumAction, action);
    
    public virtual void PerformAction(EnumsActions actionEnum)
    {
        if (_actionDictionary.ContainsKey(actionEnum)) _actionDictionary[actionEnum].Invoke();
    }

    public void NotifyObserver(EnumsActions actionEnum)
    {
        Observer.Instance.NotifyObservers(actionEnum);
    }
}
