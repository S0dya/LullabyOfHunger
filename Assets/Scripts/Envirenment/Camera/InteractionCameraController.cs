using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCameraController : SingletonSubject<InteractionCameraController>
{

    //local
    Camera _interactionCam;

    protected override void Awake()
    {
        base.Awake();

        _interactionCam = GetComponent<Camera>();

        AddAction(EnumsActions.OnSwitchToInteraction, StartRendering);
        AddAction(EnumsActions.OnReload, StartRendering);

        AddAction(EnumsActions.OnSwitchToFirstPerson, StopRendering);
        AddAction(EnumsActions.OnSwitchToIsometric, StopRendering);
    }

    public void SetCameraTransform(Transform newTransformParent)
    {
        transform.SetParent(newTransformParent);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    void StartRendering()
    {
        ToggleRendering(true);
    }

    void StopRendering()
    {
        ToggleRendering(false);
    }

    void ToggleRendering(bool toggle)
    {
        _interactionCam.enabled = toggle;
    }
}
