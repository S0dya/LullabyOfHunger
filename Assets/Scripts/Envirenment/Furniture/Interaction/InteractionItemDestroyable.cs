using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionItemDestroyable : InteractionItem
{
    //local
    SphereCollider _collider;
    LightInteraction _lightInteraction;


    void Awake()
    {
        _lightInteraction = GetComponentInChildren<LightInteraction>();
        _collider = GetComponent<SphereCollider>();
    }


    public void DestroyObj()
    {
         _collider.enabled = _lightInteraction.enabled = false;

        RemoveInteraction();
    }
}
