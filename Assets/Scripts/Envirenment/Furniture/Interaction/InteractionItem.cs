using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionItem : Interaction
{
    [SerializeField] InteractionItemEnum ItemEnum;

    [SerializeField] Transform InteractionCamTransf;

    [SerializeField] bool DestroyesOnInteracted;

    //outside methods
    public void Interact()
    {
        InteractionCamera.Instance.SetCameraTransform(InteractionCamTransf);

        UIInteraction.Instance.StartInteraction(ItemEnum);

        if (DestroyesOnInteracted)
        {
            GetComponent<SphereCollider>().enabled = false;

            Invoke("DestroyObj", 0.001f);
        }
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }
}
