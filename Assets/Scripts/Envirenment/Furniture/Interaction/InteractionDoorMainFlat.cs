using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDoorMainFlat : InteractionDoor
{
    BoxCollider _collider;

    void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    void Start()
    {
        _collider.enabled = false;
    }

    //outside methods
    public void EnableDoor()
    {
        _collider.enabled = true;
    }
}
