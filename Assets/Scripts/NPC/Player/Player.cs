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
    public int[] LookRotationOffsetX = new int[2];
    public int[] LookRotationOffsetY = new int[2];

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

    //general 
    float _curDurability = 2;

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

    Vector2 _lookDirection;
    float _curLookDirectionDistance;

    //hand&aiming
    float _curHandAimingDistance;
    float _curAimingRigWeight;

    Vector3 _randomAimingOffsetPos;
    Vector2 _cameraAimingOffset;

    //other
    float _deltaTime;
    
    bool _isLooking;
    bool _canLook;

    bool _isAiming;
    bool _canShoot = true;

    //cons
    /*
    Vector2 LookDirection
    {
        get { return _lookDirection; }
        set
        {
            _lookDirection = value;

            ChangeLooking();
        }
    }
    */

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
        _cameraAimingOffset = new Vector2(Screen.width / 2, Screen.height / 2);

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
            StopCor(_smoothAimingHandCor);
            _smoothAimingHandCor = StartCoroutine(SmoothAimingHandCor());

            StopCor(_dereaseWeightOfHandCor);
            _increaseWeightOfHandCor = StartCoroutine(IncreaseWeightOfHandCor());

            _isAiming = true;
        }
    }

    //update
    void Update()
    {
        _deltaTime = Time.deltaTime;

        Gravity();
        MoveRotate();

        Animator.SetFloat(_animIDMotionSpeed, _curMovementSpeed);
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

        if (_isLooking)
        {
            FirstPersonCameraTranform.localRotation = Quaternion.Euler(-_lookDirection.y, _lookDirection.x, 0.0f);
        
            if (Physics.Raycast(FirstPersonCamera.ScreenPointToRay(_lookDirection + _cameraAimingOffset), out RaycastHit hit)) LookingTargetTransform.position = hit.point;
            else LookingTargetTransform.position = FirstPersonCameraTranform.position + FirstPersonCameraTranform.forward * 2;//myb remove later

            //AimingHandTargetTransform.rotation = Quaternion.LookRotation(AimingHandTargetTransform.position - FirstPersonCameraTranform.position);
        }
    }

    //gun
    void Shoot()
    {

    }


    //cors
    IEnumerator SmoothReturnLookDirectionCor()
    {
        _curLookDirectionDistance = GetDistance(_lookDirection, Vector2.zero);
        float firstDistance = _curLookDirectionDistance;

        while (_curLookDirectionDistance > 0.1f)
        {
            _lookDirection = Vector2.Lerp(_lookDirection, Vector2.zero, 1.5f * _deltaTime);
            LookingRig.weight = Mathf.Clamp01(_curLookDirectionDistance / firstDistance);

            _curLookDirectionDistance = GetDistance(_lookDirection, Vector2.zero);

            yield return null;
        }

        _lookDirection = Vector2.zero; LookingRig.weight = 0;
    }

    IEnumerator SmoothAimingHandCor()
    {
        while (true)
        {
            _randomAimingOffsetPos = LookingTargetTransform.position + GetRandomVector2(0.1f);
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

        _canLook = true;
    }

    //actions
    public void StartLooking()
    {
        StopCor(_returnLookDirectionCor);

        LookingRig.weight = 1.0f;
        ToggleLooking(true);

        _isometricInput.Disable();
        _firstPersonInput.Enable();
    }
    public void StopLooking()
    {
        StopCor(_returnLookDirectionCor);
        _returnLookDirectionCor = StartCoroutine(SmoothReturnLookDirectionCor());
        StopCor(_smoothAimingHandCor);

        StopCor(_increaseWeightOfHandCor);
        _dereaseWeightOfHandCor = StartCoroutine(DecreaseWeightOfHandCor());

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

    float GetDistance(Vector2 distance0, Vector2 distance1)
    {
        return Vector2.Distance(distance0, distance1);
    }

    Vector3 GetRandomVector3(float offset)
    {
        return new Vector3(GetRandomFloat(offset), GetRandomFloat(offset), GetRandomFloat(offset));
    }

    Vector3 GetRandomVector2(float offset)
    {
        return new Vector2(GetRandomFloat(offset), GetRandomFloat(offset));
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
