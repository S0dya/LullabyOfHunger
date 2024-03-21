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

    List<InputActionMap> _actionMapsList = new List<InputActionMap>();

    protected override void Awake()
    {
        base.Awake();

        AddAction(EnumsActions.OnSwitchToFirstPerson, ToFirstPersonView);
        AddAction(EnumsActions.OnSwitchToIsometric, ToIsometricView);
        AddAction(EnumsActions.OnSwitchToInteraction, ToInteractionView);
    }

    void Start()
    {
        _actionMapsList.Add(_firstPersonInput);
        _actionMapsList.Add(_isometricInput);

        _firstPersonInput.Disable(); //myb remove later
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _input = new Inputs();

        _isometricInput = _input.IsometricInput;
        _firstPersonInput = _input.FirstPersonInput;

        _input.Input.LookAt.performed += ctx => Player.Instance.OnRightMousePerformed();
        _input.Input.LookAt.canceled += ctx => Player.Instance.OnRightMouseCanceled();

        _input.Input.Reload.performed += ctx => Player.Instance.OnReload();

        _input.IsometricInput.Move.performed += ctx => Player.Instance.OnMove(ctx.ReadValue<float>());
        _input.IsometricInput.Move.canceled += ctx => Player.Instance.OnMoveStop(0);

        _input.IsometricInput.Rotation.performed += ctx => Player.Instance.OnRotate(ctx.ReadValue<float>());
        _input.IsometricInput.Rotation.canceled += ctx => Player.Instance.OnRotateStop(0);

        _input.IsometricInput.Run.performed += ctx => Player.Instance.OnRun();
        _input.IsometricInput.Run.canceled += ctx => Player.Instance.OnRunStop();

        _input.IsometricInput.Interact.performed += ctx => Player.Instance.OnInteracte();

        _input.FirstPersonInput.Look.performed += ctx => Player.Instance.HandleMouseDeltaInput.Invoke(ctx.ReadValue<Vector2>());

        _input.FirstPersonInput.Fire.performed += ctx => Player.Instance.OnFire();

        _input.Enable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();

        _input.Input.LookAt.performed -= ctx => Player.Instance.OnRightMousePerformed();
        _input.Input.LookAt.canceled -= ctx => Player.Instance.OnRightMouseCanceled();

        _input.Input.Reload.performed -= ctx => Player.Instance.OnReload();

        _input.IsometricInput.Move.performed -= ctx => Player.Instance.OnMove(ctx.ReadValue<float>());
        _input.IsometricInput.Move.canceled -= ctx => Player.Instance.OnMoveStop(0);

        _input.IsometricInput.Rotation.performed -= ctx => Player.Instance.OnRotate(ctx.ReadValue<float>());
        _input.IsometricInput.Rotation.canceled -= ctx => Player.Instance.OnRotateStop(0);

        _input.IsometricInput.Run.performed -= ctx => Player.Instance.OnRun();
        _input.IsometricInput.Run.canceled -= ctx => Player.Instance.OnRunStop();

        _input.FirstPersonInput.Look.performed -= ctx => Player.Instance.HandleMouseDeltaInput.Invoke(ctx.ReadValue<Vector2>());

        _input.FirstPersonInput.Fire.performed -= ctx => Player.Instance.OnFire();

        _input.Disable();
    }

    //actions
    void ToFirstPersonView() => EnableActionMap(_firstPersonInput);
    void ToIsometricView() => EnableActionMap(_isometricInput);
    void ToInteractionView() => EnableActionMap(_firstPersonInput);

    //other methods
    void EnableActionMap(InputActionMap map)
    {
        DisableInput();

        map.Enable();
    }

    void DisableInput()
    {
        foreach (var map in _actionMapsList) map.Disable();
    }
    
}
