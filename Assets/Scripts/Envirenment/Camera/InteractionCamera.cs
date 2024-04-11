using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCamera : SingletonCamera<InteractionCamera>, ICamera
{

    public void SetCameraTransform(Transform newTransformParent)
    {
        Debug.Log("123");

        transform.SetParent(newTransformParent);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
