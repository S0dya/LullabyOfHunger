using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player: Subject
{
    [Header("Settings")]
    public float WalkSpeed = 2;
    public float RunSpeed = 4;

    public Vector2 MoveSmoothness;

    public Vector2 RotateSmoothness;

    [Header("Other")]
    [SerializeField] CharacterController Cc;
    [SerializeField] Animator Animator;

    //local
    Inputs _input;

    //gravity
    Vector3 _gravity;
    float _gravityForce = 10;

    //movement
    float _targetSpeed;
    float _curMovementSpeed;
    float _movementSpeed;

    float _curMovementDirection;

    float _curMoveSmoothness;

    //rotation
    float _targetRotation;
    float _curRotationDirection;

    float _curRotateSmoothness;

    //other
    float _deltaTime;

    //cors
    Coroutine _moveCor;
    Coroutine _rotateCor;

    //serialization
    protected override void Awake()
    {
        base.Awake();

        AssignInput();
    }
    void AssignInput()
    {
        _input = new Inputs();

        _input.Input.Move.performed += ctx => OnMove(ctx.ReadValue<float>(), MoveSmoothness[0]);
        _input.Input.Move.canceled += ctx => OnMove(0, MoveSmoothness[1]);

        _input.Input.Rotation.performed += ctx => OnRotate(ctx.ReadValue<float>(), RotateSmoothness[0]);
        _input.Input.Rotation.canceled += ctx => OnRotate(0, RotateSmoothness[1]);

        _input.Input.Run.performed += ctx => OnRun(RunSpeed);
        _input.Input.Run.canceled += ctx => OnRun(WalkSpeed);

        _input.Input.LookAt.performed += ctx => OnStartAiming();
        _input.Input.LookAt.canceled += ctx => OnStopAiming();
    }

    void Start()
    {
        _movementSpeed = WalkSpeed;
    }

    void OnEnable() => _input.Enable();
    void OnDisable() => _input.Disable();

    //input
    void OnMove(float direction, float smoothSpeed)
    {
        _curMovementDirection = direction;
        ChangeTargetSpeed();

        _curMoveSmoothness = smoothSpeed;
    }

    void OnRotate(float direction, float smoothSpeed)
    {
        _targetRotation = direction;

        _curRotateSmoothness = smoothSpeed;
    }

    void OnRun(float speed)
    {
        _movementSpeed = speed;

        ChangeTargetSpeed();
    }

    void OnStartAiming()
    {

    }

    void OnStopAiming()
    {

    }

    //update
    void Update()
    {
        _deltaTime = Time.deltaTime;

        Gravity();
        MoveRotate();

        Animator.SetFloat("MotionSpeed", _curMovementSpeed);
    }
    void Gravity()
    {
        if (Cc.isGrounded) _gravity.y = 0;
        else _gravity.y -= _gravityForce * _deltaTime;
    }
    void MoveRotate()
    {
        _curMovementSpeed = GetLerpVal(_curMovementSpeed, _targetSpeed, 0.1f, _curMoveSmoothness);
        _curRotationDirection = GetLerpVal(_curRotationDirection, _targetRotation, 0.05f, _curRotateSmoothness);

        transform.rotation = Quaternion.Euler(0.0f, transform.eulerAngles.y + _curRotationDirection * (1 - Mathf.Clamp01(GetAbs(_curMovementSpeed) / RunSpeed)), 0.0f);

        Cc.Move((transform.forward.normalized * (_curMovementSpeed) + _gravity) * _deltaTime);
    }

    //cors

    //other methods
    float GetLerpVal(float curValue, float targetValue, float threshold, float smoothTime)
    {
        return GetAbs(targetValue - curValue) > threshold ? Mathf.Lerp(curValue, targetValue, smoothTime * _deltaTime) : targetValue;
    }

    float GetAbs(float val)
    {
        return Mathf.Abs(val);
    }

    void ChangeTargetSpeed() => _targetSpeed = _curMovementDirection * _movementSpeed;
}
