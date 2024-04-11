using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonCamera<T> : MonoBehaviour where T : MonoBehaviour 
{
    public Camera Camera;

    //singleton
    static T instance;
    public static T Instance { get { return instance; } }
    
    //local

    protected virtual void Awake()
    {
        if (instance == null) instance = this as T;
        else Debug.Log(gameObject.transform + " duplicated");

        if (Camera == null) Camera = GetComponent<Camera>();
    }

    //outside methods
    public virtual void ToggleCam(bool toggle)
    {
        Camera.enabled = toggle;
    }
}

interface ICamera
{
    void ToggleCam(bool toggle);
}