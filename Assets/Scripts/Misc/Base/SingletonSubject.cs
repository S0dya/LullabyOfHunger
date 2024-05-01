using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonSubject<T> : Subject where T : MonoBehaviour
{
    static T instance;
    public static T Instance { get { return instance; } }

    public void CreateInstance()
    {
        if (instance == null) instance = this as T;
        else Debug.Log(gameObject.transform + " duplicated");
    }
}
