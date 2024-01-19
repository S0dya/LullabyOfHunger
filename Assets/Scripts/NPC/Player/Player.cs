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

    public Vector2 LookRotationLimit;

    

    [Header("Other")]
    [SerializeField] GameObject FirstPersonCameraObj;
    [SerializeField] Camera FirstPersonCamera;

    //local
    CharacterController Cc;
    Animator Animator;
    
    Inputs _input;
    InputActionMap _isometricInput;
    InputActionMap _firstPersonInput;   

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

    bool _isAiming;

    Vector2 _lookDirection;

    //cors
    Coroutine _moveCor;
    Coroutine _rotateCor;

    Coroutine _returnLookDirectionCor;

    //serialization
    protected override void Awake()
    {
        base.Awake();

        Cc = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();

        AddAction(EnumsActions.OnStartAiming, StartAiming);
        AddAction(EnumsActions.OnStopAiming, StopAiming);

        AssignInput();
    }
    void AssignInput()
    {
        _input = new Inputs();

        _isometricInput = _input.IsometricInput;
        _firstPersonInput = _input.FirstPersonInput;
        _firstPersonInput.Disable();

        _input.Input.LookAt.performed += ctx => OnStartAiming();
        _input.Input.LookAt.canceled += ctx => OnStopAiming();

        _input.IsometricInput.Move.performed += ctx => OnMove(ctx.ReadValue<float>(), MoveSmoothness[0]);
        _input.IsometricInput.Move.canceled += ctx => OnMove(0, MoveSmoothness[1]);

        _input.IsometricInput.Rotation.performed += ctx => OnRotate(ctx.ReadValue<float>(), RotateSmoothness[0]);
        _input.IsometricInput.Rotation.canceled += ctx => OnRotate(0, RotateSmoothness[1]);

        _input.IsometricInput.Run.performed += ctx => OnRun(RunSpeed);
        _input.IsometricInput.Run.canceled += ctx => OnRun(WalkSpeed);

        _input.FirstPersonInput.Look.performed += ctx => OnAim(ctx.ReadValue<Vector2>());
    }

    void Start()
    {
        _movementSpeed = WalkSpeed;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
     
        _input.Enable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
     
        _input.Disable();
    }

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
        Observer.Instance.NotifyObservers(EnumsActions.OnStartAiming);
    }

    void OnStopAiming()
    {
        Observer.Instance.NotifyObservers(EnumsActions.OnStopAiming);
    }

    void OnAim(Vector2 direction)
    {
        _lookDirection = new Vector2(Mathf.Clamp(_lookDirection.x + direction.x, -LookRotationLimit.x, LookRotationLimit.x), 
            Mathf.Clamp(_lookDirection.y + direction.y, -LookRotationLimit.y, LookRotationLimit.y));
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

        if (_isAiming) FirstPersonCameraObj.transform.localRotation = Quaternion.Euler(-_lookDirection.y, _lookDirection.x, 0.0f);
    }

    //cors
    IEnumerator SmoothReturnLookDirectionCor()
    {
        while (Vector2.Distance(_lookDirection, Vector2.zero) > 0.1f)
        {
            _lookDirection = Vector2.Lerp(_lookDirection, Vector2.zero, 1.5f * _deltaTime) ;

            yield return null;
        }

        _lookDirection = Vector2.zero;
    }

    //actions
    public void StartAiming()
    {
        if (_returnLookDirectionCor != null) StopCoroutine(_returnLookDirectionCor);

        ToggleAiming(true);

        _isometricInput.Disable();
        _firstPersonInput.Enable();
    }
    public void StopAiming()
    {

        if (_returnLookDirectionCor != null) StopCoroutine(_returnLookDirectionCor);
        _returnLookDirectionCor = StartCoroutine(SmoothReturnLookDirectionCor());

        ToggleAiming(false);

        _isometricInput.Enable();
        _firstPersonInput.Disable();
    }

    //other methods
    void ToggleAiming(bool toggle)
    {
        _isAiming = toggle;
        FirstPersonCamera.enabled = toggle;
    }

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
