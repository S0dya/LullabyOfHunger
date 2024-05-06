using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionItemDestroyable : InteractionItem
{
    [Header("Additional")] [SerializeField] UnityEvent AdditionalEvent;

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
        if (AdditionalEvent != null) AdditionalEvent.Invoke();

         _collider.enabled = _lightInteraction.enabled = false;

        RemoveInteraction();
    }
}
