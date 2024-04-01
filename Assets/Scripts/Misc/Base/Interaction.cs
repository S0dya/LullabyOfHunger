using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider collision)
    {
        InteractionController.Instance.AddInteraction(this);
    }
    protected virtual void OnTriggerExit(Collider collision)
    {
        InteractionController.Instance.RemoveInteraction(this);
    }
}
