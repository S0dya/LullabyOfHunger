using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionItem : Interaction
{
    [SerializeField] InteractionItemEnum ItenEnum;

    //outside methods
    public void Interact()
    {
        UIInteraction.Instance.StartInteraction(ItenEnum);
    }
}
