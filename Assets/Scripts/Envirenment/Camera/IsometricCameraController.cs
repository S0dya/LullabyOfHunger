using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCameraController : SingletonSubject<IsometricCameraController>
{
    //local
    Camera IsometricCamera;

    protected override void Awake()
    {
        base.Awake();

        IsometricCamera = GetComponent<Camera>();

        AddAction(EnumsActions.OnSwitchToIsometric, StartRendering);
        AddAction(EnumsActions.OnSwitchToFirstPerson, StopRendering);
        AddAction(EnumsActions.OnSwitchToInteraction, StopRendering);
    }

    void Start()
    {
        
    }

    //actions
    public void StartRendering()
    {
        ToggleRendering(true);
    }

    public void StopRendering()
    {
        ToggleRendering(false);
    }

    //other methods
    void ToggleRendering(bool toggle)
    {
        IsometricCamera.enabled = toggle;
    }

}
