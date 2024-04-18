using UnityEngine;

public class Interaction : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider collision)
    {
        InteractionController.Instance.AddInteraction(this);
    }
    protected virtual void OnTriggerExit(Collider collision)
    {
        RemoveInteraction();
    }

    //inherited methods
    public void RemoveInteraction()
    {
        InteractionController.Instance.RemoveInteraction(this);
    }
}
