using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCamera : Subject
{

    //local
    Camera _interactionCam;

    protected override void Awake()
    {
        base.Awake();

        _interactionCam = GetComponent<Camera>();

        AddAction(EnumsActions.OnStartInteractionView, StartInteractionView);
        AddAction(EnumsActions.OnStopInteractionView, StopInteractionView);
    }

    public void SetCameraTransform(Transform newTransformParent)
    {
        transform.SetParent(newTransformParent);
    }

    void StartInteractionView()
    {
        ToggleInteractionCam(true);
    }

    void StopInteractionView()
    {
        ToggleInteractionCam(false);
    }

    void ToggleInteractionCam(bool toggle)
    {
        _interactionCam.enabled = toggle;
    }
}
