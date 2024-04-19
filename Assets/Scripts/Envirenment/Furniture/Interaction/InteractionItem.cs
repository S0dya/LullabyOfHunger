using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionItem : Interaction
{
    [SerializeField] GameObject ObjectToDisable;
    [SerializeField] public InteractionItemEnum ItemEnum;
    [SerializeField] Transform InteractionCamTransf;

    //outside methods
    public virtual void Interact()
    {
        InteractionCamera.Instance.SetCameraTransform(InteractionCamTransf);

        UIInteraction.Instance.StartInteraction(this, ItemEnum);
    }

    public void ToggleObj(bool toggle)
    {
        ObjectToDisable.SetActive(toggle);
    }
}
