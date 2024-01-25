using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCameraController : Subject
{
    [SerializeField] Camera IsometricCamera;

    protected override void Awake()
    {
        base.Awake();

        AddAction(EnumsActions.OnStartLooking, StartLooking);
        AddAction(EnumsActions.OnStopLooking, StopLooking);
    }

    void Start()
    {
        
    }

    //actions
    public void StartLooking()
    {
        ToggleLooking(false);
    }

    public void StopLooking()
    {
        ToggleLooking(true);
    }

    //other methods
    void ToggleLooking(bool toggle)
    {
        IsometricCamera.enabled = toggle;
    }

}
