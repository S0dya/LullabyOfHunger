using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Observer : SingletonMonobehaviour<Observer>
{
    List<Subject> _subjects = new List<Subject>();

    public void AddObserver(Subject subject) => _subjects.Add(subject);
    public void RemoveObserver(Subject subject) => _subjects.Remove(subject);
    public void NotifyObservers(EnumsActions actionEnum)
    {
        foreach (Subject subject in _subjects) subject.PerformAction(actionEnum);
    }
}