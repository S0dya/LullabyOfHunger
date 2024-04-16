using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Observer : SingletonMonobehaviour<Observer>
{
    public delegate void EventHandler(EnumsActions actionEnum);
    public static event EventHandler OnNotifyObservers;

    public void NotifyObservers(EnumsActions enumAction)
    {
        OnNotifyObservers?.Invoke(enumAction);
    }
}