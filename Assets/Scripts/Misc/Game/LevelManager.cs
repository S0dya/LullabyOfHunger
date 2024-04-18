using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class LevelManager : SingletonSubject<LevelManager>
{
    [SerializeField] IsometricCamera _isometricCam;
    [SerializeField] FirstPersonCamera _fPCam;
    [SerializeField] ReloadCamera _reloadCam;
    [SerializeField] InteractionCamera _interactionCam;

    //local

    ICamera[] _cameras;
    //InteractionCamera _interactionCam;

    protected override void Awake()
    {
        base.Awake(); CreateInstance();

        _cameras = new ICamera[4] { _isometricCam, _fPCam, _reloadCam, _interactionCam };

        AddAction(EnumsActions.OnSwitchToIsometric, SwitchToIsometric);
        AddAction(EnumsActions.OnSwitchToFirstPerson, SwitchToFP);
        AddAction(EnumsActions.OnReload, SwitchToReload);
        AddAction(EnumsActions.OnSwitchToInteraction, SwitchToInteraction);
    }

    //actions
    void SwitchToIsometric()
    {
        DisableCameras(); _isometricCam.ToggleCam(true);
    }
    void SwitchToFP()
    {
        DisableCameras(); _fPCam.ToggleCam(true);
    }
    void SwitchToReload()
    {
        DisableCameras(); _reloadCam.ToggleCam(true);
    }
    void SwitchToInteraction()
    {
        DisableCameras(); _interactionCam.ToggleCam(true);
    }

    //other methods
    void DisableCameras()
    {
        foreach (var cam in _cameras) cam.ToggleCam(false);
    }
}
