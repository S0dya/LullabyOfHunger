using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionItemDestroyable : InteractionItem
{
    [SerializeField] GameObject ObjectToDisable;

    //local
    SphereCollider _collider;
    LightInteraction _lightInteraction;


    void Awake()
    {
        _lightInteraction = GetComponentInChildren<LightInteraction>();
        _collider = GetComponent<SphereCollider>();
    }

    public override void Interact()
    {
        base.Interact();

        UIInteraction.Instance.StartInteraction(this, ItemEnum);
    }


    public void DisableObj()
    {
        ObjectToDisable.SetActive(false); _collider.enabled = _lightInteraction.enabled = false;

        RemoveInteraction();
    }
}
