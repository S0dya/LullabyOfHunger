using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    [SerializeField] float TimeBeforeDestroy = 2;

    void Start()
    {
        Invoke("DestroyGO", TimeBeforeDestroy);
    }

    void DestroyGO() => Destroy(gameObject);
}
