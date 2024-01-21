using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

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
    public Vector2 LookRotationLimit;


    [Header("rigging")]
    [SerializeField] Rig LookingRig;
    [SerializeField] Rig AimingRig;

    [SerializeField] Transform LookingTargetTransform;

    [SerializeField] Transform AimingHandTargetTransform;
    [SerializeField] Transform AimingHandgunOriginTransform;

    [Header("Other")]
    [SerializeField] Transform FirstPersonCameraTranform;
    [SerializeField] Camera FirstPersonCamera;

    //local
    CharacterController Cc;
    Animator Animator;
    
    Inputs _input;
    InputActionMap _isometricInput;
    InputActionMap _firstPersonInput;   

    //gravity
    Vector3 _gravity;
    float _gravityForce = 9.81f;

    //general 
    float _curDurability = 2; 

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

    Vector2 _lookDirection;
    float _curLookDirectionDistance;

    //hand&aiming
    float _curHandAimingDistance;
    float _curAimingRigWeight;

    Vector3 _randomAimingOffsetPos;

    //other
    float _deltaTime;
    
    bool _isAiming;
    bool _canAim;


    //cons
    Vector2 LookDirection
    {
        get { return _lookDirection; }
        set
        {
            _lookDirection = value;

            ChangeLooking();
        }
    }

    //cors
    Coroutine _moveCor;
    Coroutine _rotateCor;

    Coroutine _returnLookDirectionCor;
    Coroutine _smoothAimingHandCor;

    Coroutine _increaseWeightOfHandCor;
    Coroutine _dereaseWeightOfHandCor;

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

    void OnStartAiming()
    {
        if (_canAim)
        {
            Observer.Instance.NotifyObservers(EnumsActions.OnStartAiming);

            _canAim = false;
        }
    }

    void OnStopAiming()
    {
        Observer.Instance.NotifyObservers(EnumsActions.OnStopAiming);
    }

    void OnAim(Vector2 direction)
    {
        LookDirection = new Vector2(Mathf.Clamp(LookDirection.x + direction.x, -LookRotationLimit.x, LookRotationLimit.x), 
            Mathf.Clamp(LookDirection.y + direction.y, -LookRotationLimit.y, LookRotationLimit.y));
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

        if (_isAiming) FirstPersonCameraTranform.localRotation = Quaternion.Euler(-LookDirection.y, LookDirection.x, 0.0f);
    }

    //rigging
    void ChangeLooking()
    {
        LookingTargetTransform.position = FirstPersonCameraTranform.position + FirstPersonCameraTranform.forward * 3;
    }

    //cors
    IEnumerator SmoothReturnLookDirectionCor()
    {
        _curLookDirectionDistance = GetDistance(LookDirection, Vector2.zero);
        float firstDistance = _curLookDirectionDistance;

        while (_curLookDirectionDistance > 0.1f)
        {
            LookDirection = Vector2.Lerp(LookDirection, Vector2.zero, 1.5f * _deltaTime);
            LookingRig.weight = Mathf.Clamp01(_curLookDirectionDistance / firstDistance);

            _curLookDirectionDistance = GetDistance(LookDirection, Vector2.zero);

            yield return null;
        }

        LookDirection = Vector2.zero; LookingRig.weight = 0;
    }

    IEnumerator SmoothAimingHandCor()
    {
        while (true)
        {
            _randomAimingOffsetPos = LookingTargetTransform.position + GetRandomPos(0.1f);
            _curHandAimingDistance = GetDistance(AimingHandTargetTransform.position, _randomAimingOffsetPos);

            while (_curHandAimingDistance > 0.1f && _curHandAimingDistance < 0.2f)//change later
            {
                AimingHandTargetTransform.position = Vector3.Lerp(AimingHandTargetTransform.position, _randomAimingOffsetPos, 1.5f * _deltaTime);
                _curHandAimingDistance = GetDistance(AimingHandTargetTransform.position, _randomAimingOffsetPos);

                yield return null;
            }

            yield return null;
        }
    }

    IEnumerator IncreaseWeightOfHandCor()
    {
        while (AimingRig.weight < 1f)
        {
            AimingRig.weight = Mathf.Lerp(AimingRig.weight, 1.05f, _curDurability * _deltaTime);

            yield return null;
        }

        AimingRig.weight = 1;
    }
    IEnumerator DecreaseWeightOfHandCor()
    {
        while (AimingRig.weight > 0)
        {
            AimingRig.weight = Mathf.Lerp(AimingRig.weight, -0.05f, Durability * _deltaTime);

            yield return null;
        }

        AimingRig.weight = 0;
    }

    IEnumerator DelayAimCor()
    {
        yield return new WaitForSeconds(AimDelayDuration);

        _canAim = true;
    }

    //actions
    public void StartAiming()
    {
        StopCor(_returnLookDirectionCor);

        StopCor(_smoothAimingHandCor);
        _smoothAimingHandCor = StartCoroutine(SmoothAimingHandCor());

        StopCor(_dereaseWeightOfHandCor);
        _increaseWeightOfHandCor = StartCoroutine(IncreaseWeightOfHandCor());

        LookingRig.weight = 1.0f;
        ToggleAiming(true);

        _isometricInput.Disable();
        _firstPersonInput.Enable();
    }
    public void StopAiming()
    {
        StopCor(_returnLookDirectionCor);
        _returnLookDirectionCor = StartCoroutine(SmoothReturnLookDirectionCor());
        StopCor(_smoothAimingHandCor);

        StopCor(_increaseWeightOfHandCor);
        _dereaseWeightOfHandCor = StartCoroutine(DecreaseWeightOfHandCor());

        StartCoroutine(DelayAimCor());

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

    float GetDistance(Vector2 distance0, Vector2 distance1)
    {
        return Vector2.Distance(distance0, distance1);
    }

    Vector3 GetRandomPos(float offset)
    {
        return new Vector3(GetRandomFloat(offset), GetRandomFloat(offset), GetRandomFloat(offset));
    }

    float GetRandomFloat(float offset)
    {
        return Random.Range(-offset, offset);
    }

    void StopCor(Coroutine cor) 
    {
        if (cor != null) StopCoroutine(cor);
    }

    void ChangeTargetSpeed() => _targetSpeed = _curMovementDirection * _movementSpeed;
}
