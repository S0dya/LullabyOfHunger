using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : SingletonSubject<InputManager>
{
    //local

    //inputs
    Inputs _input;

    InputActionMap _firstPersonInput;
    InputActionMap _isometricInput;
    InputActionMap _reloadInput;

    List<InputActionMap> _actionMapsList = new List<InputActionMap>();

    protected override void Awake()
    {
        base.Awake();

        AddAction(EnumsActions.OnSwitchToFirstPerson, ToFirstPersonView);
        AddAction(EnumsActions.OnSwitchToIsometric, ToIsometricView);
        AddAction(EnumsActions.OnSwitchToInteraction, ToInteractionView);
        AddAction(EnumsActions.OnReload, ToReloadView);
    }

    void Start()
    {
        _actionMapsList.Add(_firstPersonInput);
        _actionMapsList.Add(_isometricInput);
        _actionMapsList.Add(_reloadInput);

        ToIsometricView();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _input = new Inputs();

        _isometricInput = _input.IsometricInput;
        _firstPersonInput = _input.FirstPersonInput;
        _reloadInput = _input.ReloadInput;

        //isometric
        _input.IsometricInput.LookAt.performed += ctx => Player.Instance.IsometricLook();

        _input.IsometricInput.Move.performed += ctx => Player.Instance.OnMove(ctx.ReadValue<float>());
        _input.IsometricInput.Move.canceled += ctx => Player.Instance.OnMoveStop(0);

        _input.IsometricInput.Rotation.performed += ctx => Player.Instance.OnRotate(ctx.ReadValue<float>());
        _input.IsometricInput.Rotation.canceled += ctx => Player.Instance.OnRotateStop(0);

        _input.IsometricInput.Run.performed += ctx => Player.Instance.OnRun();
        _input.IsometricInput.Run.canceled += ctx => Player.Instance.OnRunStop();

        _input.IsometricInput.Interact.performed += ctx => Player.Instance.IsometricInteracte();

        _input.IsometricInput.Reload.performed += ctx => Player.Instance.IsometricReload();

        //first person
        _input.FirstPersonInput.Look.performed += ctx => Player.Instance.OnMouseDelta(ctx.ReadValue<Vector2>());
        _input.FirstPersonInput.LookAt.canceled += ctx => Player.Instance.FirstPersonStopLooking();

        _input.FirstPersonInput.Fire.performed += ctx => Player.Instance.OnFire();

        //reload
        _input.ReloadInput.MoveHand.performed += ctx => Player.Instance.OnMouseDelta(ctx.ReadValue<Vector2>());

        _input.ReloadInput.Grab.performed += ctx => Player.Instance.ReloadGrab();
        _input.ReloadInput.Grab.canceled += ctx => Player.Instance.ReloadRelease();

        _input.ReloadInput.ExitReload.canceled += ctx => Player.Instance.ReloadStopReloading();

        _input.Enable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();

        //isometric
        _input.IsometricInput.LookAt.performed -= ctx => Player.Instance.IsometricLook();

        _input.IsometricInput.Move.performed -= ctx => Player.Instance.OnMove(ctx.ReadValue<float>());
        _input.IsometricInput.Move.canceled -= ctx => Player.Instance.OnMoveStop(0);

        _input.IsometricInput.Rotation.performed -= ctx => Player.Instance.OnRotate(ctx.ReadValue<float>());
        _input.IsometricInput.Rotation.canceled -= ctx => Player.Instance.OnRotateStop(0);

        _input.IsometricInput.Run.performed -= ctx => Player.Instance.OnRun();
        _input.IsometricInput.Run.canceled -= ctx => Player.Instance.OnRunStop();

        _input.IsometricInput.Interact.performed -= ctx => Player.Instance.IsometricInteracte();

        _input.IsometricInput.Reload.performed -= ctx => Player.Instance.IsometricReload();

        //first person
        _input.FirstPersonInput.Look.performed -= ctx => Player.Instance.OnMouseDelta(ctx.ReadValue<Vector2>());
        _input.FirstPersonInput.LookAt.canceled -= ctx => Player.Instance.FirstPersonStopLooking();

        _input.FirstPersonInput.Fire.performed -= ctx => Player.Instance.OnFire();

        //reload
        _input.ReloadInput.MoveHand.performed -= ctx => Player.Instance.OnMouseDelta(ctx.ReadValue<Vector2>());

        _input.ReloadInput.Grab.performed -= ctx => Player.Instance.ReloadGrab();
        _input.ReloadInput.Grab.canceled -= ctx => Player.Instance.ReloadRelease();

        _input.ReloadInput.ExitReload.canceled -= ctx => Player.Instance.ReloadStopReloading();

        _input.Disable();
    }

    //actions
    void ToFirstPersonView() => EnableActionMap(_firstPersonInput);
    void ToIsometricView() => EnableActionMap(_isometricInput);
    void ToInteractionView() => Debug.Log("to interaction");//EnableActionMap(_firstPersonInput);
    void ToReloadView() => EnableActionMap(_reloadInput);

    //other methods
    void EnableActionMap(InputActionMap mapToEnable)
    {
        DisableMaps();

        mapToEnable.Enable();
    }
    
    void DisableMaps()
    {
        foreach (InputActionMap map in _actionMapsList) map.Disable();
    }
}
