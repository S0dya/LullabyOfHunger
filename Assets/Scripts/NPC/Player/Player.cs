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

    [Header("smoothness of animation, 0 - performed, 1 - canceled")]
    public Vector2 MoveSmoothness;
    public Vector2 RotateSmoothness;

    [Header("FOV")]
    public int[] LookRotationOffsetX = new int[2];
    public int[] LookRotationOffsetY = new int[2];

    [Header("Targets")]
    public Transform LookingTargetTransform;

    [Header("Other")]
    [SerializeField] Transform FirstPersonCameraTranform;
    [SerializeField] Camera FirstPersonCamera;

    //local
    RiggingController _riggingController;
    InteractionCamera _interactionCamera;
    CharacterController _cc;
    Animator _animator;
    
    Inputs _input;
    InputActionMap _isometricInput;
    InputActionMap _firstPersonInput;   

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
    
    //input
    [HideInInspector] public Vector2 _lookDirection;

    //other
    float _deltaTime;
    
    bool _isLooking;
    bool _canLook;

    bool _isAiming;
    bool _canShoot = true;

    //serialization
    protected override void Awake()
    {
        base.Awake();

        _riggingController = GetComponent<RiggingController>();
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _interactionCamera = GameObject.FindGameObjectWithTag("InteractionCamera").GetComponent<InteractionCamera>();

        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");


        AddAction(EnumsActions.OnStartLooking, StartLooking);
        AddAction(EnumsActions.OnStopLooking, StopLooking);
        AddAction(EnumsActions.OnFire, Shoot);

        AssignInput();
    }
    void AssignInput()
    {
        _input = new Inputs();

        _isometricInput = _input.IsometricInput;
        _firstPersonInput = _input.FirstPersonInput;
        _firstPersonInput.Disable();

        _input.Input.LookAt.performed += ctx => OnStartLooking();
        _input.Input.LookAt.canceled += ctx => Observer.Instance.NotifyObservers(EnumsActions.OnStopLooking);

        _input.IsometricInput.Move.performed += ctx => OnMove(ctx.ReadValue<float>(), MoveSmoothness[0]);
        _input.IsometricInput.Move.canceled += ctx => OnMove(0, MoveSmoothness[1]);

        _input.IsometricInput.Rotation.performed += ctx => OnRotate(ctx.ReadValue<float>(), RotateSmoothness[0]);
        _input.IsometricInput.Rotation.canceled += ctx => OnRotate(0, RotateSmoothness[1]);

        _input.IsometricInput.Run.performed += ctx => OnRun(RunSpeed);
        _input.IsometricInput.Run.canceled += ctx => OnRun(WalkSpeed);

        _input.FirstPersonInput.Look.performed += ctx => OnLook(ctx.ReadValue<Vector2>());

        _input.FirstPersonInput.Fire.performed += ctx => OnFire();
    }

    void Start()
    {
        _movementSpeed = WalkSpeed;

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

    void OnStartLooking()
    {
        if (_canLook)
        {
            Observer.Instance.NotifyObservers(EnumsActions.OnStartLooking);

            _canLook = false;
        }
    }

    void OnLook(Vector2 direction)
    {
        _lookDirection = new Vector2(Mathf.Clamp(_lookDirection.x + direction.x, -LookRotationOffsetX[0], LookRotationOffsetX[1]), 
            Mathf.Clamp(_lookDirection.y + direction.y, -LookRotationOffsetY[0], LookRotationOffsetY[1]));
    }

    void OnFire()
    {
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

    //update
    void Update()
    {
        _deltaTime = Time.deltaTime;

        Gravity();
        MoveRotate();

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

        if (_isLooking)
        {
            FirstPersonCameraTranform.localRotation = Quaternion.Euler(-_lookDirection.y, _lookDirection.x, 0.0f);
        
            //if (Physics.Raycast(FirstPersonCamera.ScreenPointToRay(_lookDirection + _cameraAimingOffset), out RaycastHit hit)) LookingTargetTransform.position = hit.point;
            LookingTargetTransform.position = FirstPersonCameraTranform.position + FirstPersonCameraTranform.forward * 2f;//myb remove later

            //AimingHandTargetTransform.position = (GetPos(FirstPersonCameraTranform) + (GetPos(LookingTargetTransform) - GetPos(FirstPersonCameraTranform)).normalized * 2);
            //AimingHandTargetTransform.rotation = Quaternion.LookRotation(AimingHandTargetTransform.position - FirstPersonCameraTranform.position);
        }
    }

    //gun
    void Shoot()
    {
    }

    //cors
    IEnumerator DelayAimCor()
    {
        yield return new WaitForSeconds(AimDelayDuration);

        _canLook = true;
    }

    //actions
    public void StartLooking()
    {
        ToggleLooking(true);

        _isometricInput.Disable();
        _firstPersonInput.Enable();
    }
    public void StopLooking()
    {
        StartCoroutine(DelayAimCor());

        ToggleLooking(false);
        _isAiming = false;

        _isometricInput.Enable();
        _firstPersonInput.Disable();
    }

    //other methods
    void ToggleLooking(bool toggle)
    {
        _isLooking = toggle;
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
