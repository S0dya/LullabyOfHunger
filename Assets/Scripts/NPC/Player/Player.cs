using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: SingletonSubject<Player>
{
    [Header("main settings")]
    public float WalkSpeed = 2;
    public float RunSpeed = 4;
    public float RotationSpeed = 6;

    public float Durability = 4;
    public float AimDelayDuration = 0.5f;

    public float SensitivityInReloading = 0.5f;

    [Header("smoothness of animation, 0 - performed, 1 - canceled")]
    public float[] MoveSmoothness = new float[2] { 3, 2.5f };
    public float[] RotateSmoothness = new float[2] { 7, 8 }; 

    [Header("FOV")]
    public float[] LookRotationOffset = new float[4];
    public float[] ReloadingOffset = new float[4];

    public float ReloadingZOffset;

    [Header("shake")]
    public float ShakeDuration = 0.2f;
    public Vector3 ShakeRotation = Vector3.one;

    [Header("targets")]
    public Transform LookingTargetTransform;
    public Transform ReloadingTargetTransform;

    [Header("animation & sound")]
    public float MinimumSpeedToPlayFootStepSound = 1;

    //local
    GunController _gunController;
    CharacterController _cc;
    Animator _animator;
    InteractionController _interactionController;
    Transform _fPCamTranf;

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
    Action<Vector2> _handleMouseDeltaInput;

    //other
    float _deltaTime;
    
    bool _isLooking;
    bool _canLook;

    bool _isAiming;

    bool _isReloading;
    bool _canReload = true;

    //serialization
    protected override void Awake()
    {
        base.Awake(); CreateInstance();

        _gunController = GetComponent<GunController>();
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _interactionController = GetComponent<InteractionController>();
        _fPCamTranf = FirstPersonCamera.Instance.gameObject.transform;

        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _handleMouseDeltaInput = OnLook;

        AddAction(EnumsActions.OnFire, Shoot);
        AddAction(EnumsActions.OnAim, StartAiming);

        AddAction(EnumsActions.OnReload, Reload);
        AddAction(EnumsActions.OnStopReloading, StopReloading);

        AddAction(EnumsActions.OnSwitchToFirstPerson, ToFirstPersonView);
        AddAction(EnumsActions.OnSwitchToIsometric, ToIsometricView);
        AddAction(EnumsActions.OnSwitchToInteraction, ToInteractionView);

        AddAction(EnumsActions.OnStoppedAiming, StoppedAiming);
    }

    void Start()
    {
        _movementSpeed = WalkSpeed;

        StartCoroutine(DelayAimCor());
    }

    //input
    public void OnMove(float direction) => Move(direction, MoveSmoothness[0]);
    public void OnMoveStop(float direction) => Move(direction, MoveSmoothness[1]);
    void Move(float direction, float smoothSpeed)
    {
        _curMovementDirection = direction;
        ChangeTargetSpeed();

        _curMoveSmoothness = smoothSpeed;
    }

    public void OnRotate(float direction) => Rotate(direction, RotateSmoothness[0]);
    public void OnRotateStop(float direction) => Rotate(direction, RotateSmoothness[1]);
    void Rotate(float direction, float smoothSpeed)
    {
        _targetRotation = direction;

        _curRotateSmoothness = smoothSpeed;
    }

    public void OnRun() => Run(RunSpeed);
    public void OnRunStop() => Run(WalkSpeed);
    public void Run(float speed)
    {
        _movementSpeed = speed;

        ChangeTargetSpeed();
    }

    public void IsometricInteracte()
    {
        _interactionController.Interact();
    }

    public void IsometricReload()
    {
        if (_canReload) NotifyObserver(EnumsActions.OnReload);
    }

    public void IsometricLook()
    {
        if (_canLook)
        {
            NotifyObserver(EnumsActions.OnSwitchToFirstPerson);

            _canLook = _canReload = false;
        }
    }

    public void ReloadGrab()
    {
        NotifyObserver(EnumsActions.OnInteractionGrab);
    }
    public void ReloadRelease()
    {
        NotifyObserver(EnumsActions.OnInteractionRelease);
    }
    public void ReloadStopReloading()
    {
        NotifyObserver(EnumsActions.OnStopReloading);
    }

    public void OnMouseDelta(Vector2 direction)
    {
        _handleMouseDeltaInput.Invoke(direction);
    }
    public void OnLook(Vector2 direction)
    {
        _lookDirection = new Vector2(Mathf.Clamp(_lookDirection.x + direction.x, -LookRotationOffset[0], LookRotationOffset[1]), 
            Mathf.Clamp(_lookDirection.y + direction.y, -LookRotationOffset[2], LookRotationOffset[3]));
    }
    public void OnReloadLook(Vector2 direction)
    {
        _lookDirection = new Vector2(Mathf.Clamp(_lookDirection.x + direction.x * SensitivityInReloading, -ReloadingOffset[0], ReloadingOffset[1]),
            Mathf.Clamp(_lookDirection.y + direction.y * SensitivityInReloading, -ReloadingOffset[2], ReloadingOffset[3]));
    }

    public void OnFire()
    {
        if (_isAiming)
        {
            if (_gunController._curBulletsAmount > 0) NotifyObserver(EnumsActions.OnFire);
            else AudioManager.Instance.PlayOneShot("ShootNoBullets");
        }
        else
        {
            NotifyObserver(EnumsActions.OnAim);
        }
    }
    public void FirstPersonStopLooking()
    {
        NotifyObserver(EnumsActions.OnSwitchToIsometric);
    }

    //update
    void Update()
    {
        _deltaTime = Time.deltaTime;

        Gravity();
        MoveRotate();
        HandleMouse();

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
        transform.rotation = Quaternion.Euler(0.0f, transform.eulerAngles.y + _curRotationDirection * (1 - Mathf.Clamp01(GetAbs(_curMovementSpeed) / RotationSpeed)), 0.0f);
    }
    void HandleMouse()
    {
        if (_isLooking)
        {
            _fPCamTranf.localRotation = Quaternion.Euler(-_lookDirection.y, _lookDirection.x, 0.0f);

            //if (Physics.Raycast(FirstPersonCamera.ScreenPointToRay(_lookDirection + _cameraAimingOffset), out RaycastHit hit)) LookingTargetTransform.position = hit.point;
            LookingTargetTransform.position = _fPCamTranf.position + _fPCamTranf.forward * 2f;

            //AimingHandTargetTransform.position = (GetPos(_fPCamTranf) + (GetPos(LookingTargetTransform) - GetPos(_fPCamTranf)).normalized * 2);
            //AimingHandTargetTransform.rotation = Quaternion.LookRotation(AimingHandTargetTransform.position - _fPCamTranf.position);
        }
        else if (_isReloading)
        {
            ReloadingTargetTransform.localPosition = new Vector3(-_lookDirection.x, _lookDirection.y, ReloadingZOffset);
        }
    }

    //animation&sound
    public void PlayFootStep()
    {
        if (GetAbs(_curMovementSpeed) > MinimumSpeedToPlayFootStepSound) AudioManager.Instance.PlayOneShot("PlayerFootSteps");
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
        _isLooking = true;
    }
    void ToIsometricView()
    {
        StartCoroutine(DelayAimCor());
        ToggleLooking(false);
    }
    void ToInteractionView()
    {
        ToggleLooking(false);
    }

    void Shoot()
    {
        FirstPersonCamera.Instance.Shoot();
    }
    void StartAiming() => _isAiming = true;

    void Reload()
    {
        _lookDirection = _gunController.GetHandOriginPos();

        _isReloading = true;

        _handleMouseDeltaInput = OnReloadLook;
        _curOffsetMouseInput = ReloadingOffset;

        //CHANGE LATER
        ToInteractionView();
    }
    void StopReloading()
    {
        _isReloading = false;

        _handleMouseDeltaInput = OnLook;
        _curOffsetMouseInput = LookRotationOffset;
        NotifyObserver(EnumsActions.OnSwitchToIsometric);
    }

    void StoppedAiming()
    {
        _canReload = true;
    }

    //other methods

    void ToggleLooking(bool toggle) => _isAiming = _isLooking = toggle;

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
