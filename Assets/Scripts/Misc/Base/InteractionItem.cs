using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionItem : Interaction
{
    [SerializeField] public InteractionItemEnum ItemEnum;

    [SerializeField] Transform InteractionCamTransf;

    //outside methods
    public virtual void Interact()
    {
        InteractionCamera.Instance.SetCameraTransform(InteractionCamTransf);
    }
}
