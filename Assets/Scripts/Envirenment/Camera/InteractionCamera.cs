using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCamera : SingletonCamera<InteractionCamera>, ICamera
{
    public void SetCameraTransform(Transform newTransformParent)
    {
        transform.position = newTransformParent.position;
        transform.rotation = newTransformParent.rotation;
    }
}
