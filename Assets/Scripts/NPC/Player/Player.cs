using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player: Subject
{
    [Header("main settings")]
    public float WalkSpeed = 2;
    public float RunSpeed = 4;

    public float Durability = 4;
    public float AimDelayDuration = 0.5f;

    public float SensitivityInReloading = 0.5f;

    [Header("smoothness of animation, 0 - performed, 1 - canceled")]
    public Vector2 MoveSmoothness;
    public Vector2 RotateSmoothness;

    [Header("FOV")]
    public float[] LookRotationOffset = new float[4];
    public float[] ReloadingOffset = new float[4];

    public float ReloadingZOffset;

    [Header("targets")]
    public Transform LookingTargetTransform;
    public Transform ReloadingTargetTransform;

    [Header("interaction camera")]
    [SerializeField] Transform ReloadingTrasf;

    [Header("Other")]
    [SerializeField] Transform FirstPersonCameraTranform;
    [SerializeField] Camera FirstPersonCamera;

    //local
    RiggingController _riggingController;
    InteractionCameraController _interactionCameraController;
    CharacterController _cc;
    Animator _animator;
    
    Inputs _input;
    InputActionMap _firstPersonInput;
    InputActionMap _isometricInput;

    //general 
    //float _curDurability = 2;

    //animator
    int _animIDMotionSpeed;

    //gravity
    Vector3 _gravity;
    float _gravityForce = 9.81f;

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

    float[] _curOffsetMouseInput;

    //input
    [HideInInspector] public Vector2 _lookDirection;

    //actions
    Action<Vector2> HandleMouseDeltaInput;

    //other
    float _deltaTime;
    
    bool _isLooking;
    bool _canLook;

    bool _isAiming;
    bool _canShoot = true;

    bool _isInteracting;
    bool _isReloading;

    //serialization
    protected override void Awake()
    {
        base.Awake();

        _riggingController = GetComponent<RiggingController>();
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _interactionCameraController = GameObject.FindGameObjectWithTag("InteractionCameraController").GetComponent<InteractionCameraController>();

        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        HandleMouseDeltaInput = OnLook;


        AddAction(EnumsActions.OnFire, Shoot);
        AddAction(EnumsActions.OnReload, Reload);

        AddAction(EnumsActions.OnSwitchToFirstPerson, ToFirstPersonView);
        AddAction(EnumsActions.OnSwitchToIsometric, ToIsometricView);
        AddAction(EnumsActions.OnSwitchToInteraction, ToInteractionView);

        AssignInput();
    }
    void AssignInput()
    {
        _input = new Inputs();

        _isometricInput = _input.IsometricInput;
        _firstPersonInput = _input.FirstPersonInput;

        _input.Input.LookAt.performed += ctx => OnRightMousePerformed();
        _input.Input.LookAt.canceled += ctx => OnRightMouseCanceled();

        _input.Input.Reload.performed += ctx => OnStartReloading();

        _input.IsometricInput.Move.performed += ctx => OnMove(ctx.ReadValue<float>(), MoveSmoothness[0]);
        _input.IsometricInput.Move.canceled += ctx => OnMove(0, MoveSmoothness[1]);

        _input.IsometricInput.Rotation.performed += ctx => OnRotate(ctx.ReadValue<float>(), RotateSmoothness[0]);
        _input.IsometricInput.Rotation.canceled += ctx => OnRotate(0, RotateSmoothness[1]);

        _input.IsometricInput.Run.performed += ctx => OnRun(RunSpeed);
        _input.IsometricInput.Run.canceled += ctx => OnRun(WalkSpeed);

        _input.FirstPersonInput.Look.performed += ctx => HandleMouseDeltaInput.Invoke(ctx.ReadValue<Vector2>());

        _input.FirstPersonInput.Fire.performed += ctx => OnFire();
    }

    void Start()
    {
        _movementSpeed = WalkSpeed;
        _firstPersonInput.Disable();

        StartCoroutine(DelayAimCor());
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

    void OnRightMousePerformed()
    {
        if (_isReloading)
        {
            Observer.Instance.NotifyObservers(EnumsActions.OnInteractionGrab);
        }
        else
        {
            if (_canLook)
            {
                Observer.Instance.NotifyObservers(EnumsActions.OnSwitchToFirstPerson);

                _canLook = false;
            }
        }
    }
    void OnRightMouseCanceled()
    {
        if (_isReloading)
        {
            Observer.Instance.NotifyObservers(EnumsActions.OnInteractionRelease);
        }
        else
        {
            Observer.Instance.NotifyObservers(EnumsActions.OnSwitchToIsometric);
        }
    }

    void OnLook(Vector2 direction)
    {
        _lookDirection = new Vector2(Mathf.Clamp(_lookDirection.x + direction.x, -LookRotationOffset[0], LookRotationOffset[1]), 
            Mathf.Clamp(_lookDirection.y + direction.y, -LookRotationOffset[2], LookRotationOffset[3]));
    }
    void OnReloadLook(Vector2 direction)
    {
        _lookDirection = new Vector2(Mathf.Clamp(_lookDirection.x + direction.x * SensitivityInReloading, -ReloadingOffset[0], ReloadingOffset[1]),
            Mathf.Clamp(_lookDirection.y + direction.y * SensitivityInReloading, -ReloadingOffset[2], ReloadingOffset[3]));
    }

    void OnFire()
    {
        if (_isReloading) return;
        
        if (_isAiming)
        {
            if (_canShoot) Observer.Instance.NotifyObservers(EnumsActions.OnFire);
        }
        else
        {
            _riggingController.StartAiming();

            _isAiming = true;
        }
    }

    void OnStartReloading()
    {
        if (!_isReloading) Observer.Instance.NotifyObservers(EnumsActions.OnReload);
    }

    //update
    void Update()
    {
        _deltaTime = Time.deltaTime;

        Debug.Log(transform.forward);

        Gravity();
        MoveRotate();
        HandleInteraction();

        _animator.SetFloat(_animIDMotionSpeed, _curMovementSpeed);
    }
    void Gravity()
    {
        if (_cc.isGrounded) _gravity.y = 0;
        else _gravity.y -= _gravityForce * _deltaTime;
    }
    void MoveRotate()
    {
        _curMovementSpeed = GetLerpVal(_curMovementSpeed, _targetSpeed, 0.1f, _curMoveSmoothness);
        _curRotationDirection = GetLerpVal(_curRotationDirection, _targetRotation, 0.05f, _curRotateSmoothness);

        _cc.Move((transform.forward.normalized * (_curMovementSpeed) + _gravity) * _deltaTime);
        transform.rotation = Quaternion.Euler(0.0f, transform.eulerAngles.y + _curRotationDirection * (1 - Mathf.Clamp01(GetAbs(_curMovementSpeed) / RunSpeed)), 0.0f);
    }
    void HandleInteraction()
    {
        if (_isLooking)
        {
            FirstPersonCameraTranform.localRotation = Quaternion.Euler(-_lookDirection.y, _lookDirection.x, 0.0f);

            //if (Physics.Raycast(FirstPersonCamera.ScreenPointToRay(_lookDirection + _cameraAimingOffset), out RaycastHit hit)) LookingTargetTransform.position = hit.point;
            LookingTargetTransform.position = FirstPersonCameraTranform.position + FirstPersonCameraTranform.forward * 2f;//myb remove later

            //AimingHandTargetTransform.position = (GetPos(FirstPersonCameraTranform) + (GetPos(LookingTargetTransform) - GetPos(FirstPersonCameraTranform)).normalized * 2);
            //AimingHandTargetTransform.rotation = Quaternion.LookRotation(AimingHandTargetTransform.position - FirstPersonCameraTranform.position);
        }
        else if (_isReloading)
        {
            ReloadingTargetTransform.localPosition = new Vector3(-_lookDirection.x, _lookDirection.y, ReloadingZOffset);
        }
    }

    //cors
    IEnumerator DelayAimCor()
    {
        yield return new WaitForSeconds(AimDelayDuration);

        _canLook = true;
    }

    //actions
    void ToFirstPersonView()
    {
        ToggleInput(_firstPersonInput, _isometricInput);
        ToggleRendering(true);

        _isLooking = true;
    }
    void ToIsometricView()
    {
        ToggleInput(_isometricInput, _firstPersonInput);
        ToggleRendering(false);

        StartCoroutine(DelayAimCor());
        ToggleLooking(false);
    }
    void ToInteractionView()
    {
        ToggleInput(_firstPersonInput, _isometricInput);
        ToggleRendering(false);
        ToggleLooking(false);
    }

    void Shoot()
    {

    }

    void Reload()
    {
        _isReloading = true;



        HandleMouseDeltaInput = OnReloadLook;
        _curOffsetMouseInput = ReloadingOffset;
        _interactionCameraController.SetCameraTransform(ReloadingTrasf);
        Observer.Instance.NotifyObservers(EnumsActions.OnSwitchToInteraction);
    }

    //other methods
    void ToggleRendering(bool toggle)
    {
        FirstPersonCamera.enabled = toggle;
    }

    void ToggleLooking(bool toggle) => _isAiming = _isLooking = toggle;

    void ToggleInput(InputActionMap toEnabled, InputActionMap toDisabled)
    {
        toEnabled.Enable();
        toDisabled.Disable();
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
