using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EventToAction : MonoBehaviour
{
    UnityEvent _event;
    public void WrapEvent(UnityEvent eventToWrap) => _event = eventToWrap;
    public void InvokeEvent() => _event.Invoke();
}
