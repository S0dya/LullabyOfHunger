using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Subject
{
    [SerializeField] Camera IsometricCamera;

    protected override void Awake()
    {
        base.Awake();

        AddAction(EnumsActions.OnStartAiming, StartAiming);
        AddAction(EnumsActions.OnStopAiming, StopAiming);
    }

    void Start()
    {
        
    }

    //actions
    public void StartAiming()
    {
        ToggleAiming(false);
    }

    public void StopAiming()
    {
        ToggleAiming(true);
    }

    //other methods
    void ToggleAiming(bool toggle)
    {
        IsometricCamera.enabled = toggle;
    }

}
