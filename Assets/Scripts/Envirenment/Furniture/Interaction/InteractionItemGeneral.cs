using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionItemGeneral : InteractionItem
{
    public override void Interact()
    {
        base.Interact();

        UIInteraction.Instance.StartInteraction(ItemEnum);
    }
}
