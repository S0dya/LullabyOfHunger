using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Animations;

public class RiggingController : SingletonSubject<RiggingController>
{
    [Header("Settings")]
    public float Durability = 4;

    [Header("gun")]
    [Range(0, 1.2f)] public float GunRecoilPower = 0.5f;
    public float GunRecoilSpeed = 20;
    public float GunRecoilReturnSpeed = 10;

    [Header("rigging")]
    [SerializeField] Rig LookingRig;
    [SerializeField] Rig AimingRig;
    [SerializeField] AimConstraint HandRig;
    [SerializeField] Rig ReloadingRig;
    [SerializeField] Rig FingersRig;

    [SerializeField] MultiAimConstraint MacHead;
    [SerializeField] MultiAimConstraint MacChest;

    [Header("targets")]
    [SerializeField] Transform RecoilTargetTransform;
    [SerializeField] Transform AimingHandTargetTransform;

    //local
    Player player;

    //general
    float _curDurability = 2;

    //rotation 
    float _curLookDirectionDistance;
    Vector2 LookDirection
    {
        get { return player._lookDirection; }
        set { player._lookDirection = value; }
    }

    //hand&aiming
    float _curHandAimingDistance;
    float _curAimingRigWeight;

    Vector3 _randomAimingOffsetPos;
    Vector2 _cameraAimingOffset;

    //cons
    float ArmWeight
    {
        get { return AimingRig.weight; }
        set { AimingRig.weight = value; HandRig.weight = value; }
    }

    //cors
    Coroutine _returnLookDirectionCor;
    Coroutine _smoothAimingHandCor;
    Coroutine _recoilVisualizsationCor;

    Coroutine _lerpRecoilUpCor;
    Coroutine _lerpRecoilDownCor;

    Coroutine _increaseWeightOfHandCor;
    Coroutine _dereaseWeightOfHandCor;

    //reloading
    bool _isReloading;

    //serialization
    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();

        AddAction(EnumsActions.OnSwitchToFirstPerson, ToFirstPersonView);
        AddAction(EnumsActions.OnSwitchToIsometric, ToIsometricView);
        AddAction(EnumsActions.OnSwitchToInteraction, ToInteractionView);

        AddAction(EnumsActions.OnAim, StartAiming);

        AddAction(EnumsActions.OnReload, StartReloading);
        AddAction(EnumsActions.OnStopReloading, StopReloading);

        AddAction(EnumsActions.OnFire, VisualiseRecoil);

        AddAction(EnumsActions.OnInteractionGrab, InteractionGrab);
        AddAction(EnumsActions.OnInteractionRelease, InteractionRelease);
    }

    void Start()
    {
        _cameraAimingOffset = new Vector2(Screen.width / 2, Screen.height / 2);

    }

    //actions
    void ToFirstPersonView()
    {
        StartLooking();
    }
    void ToIsometricView()
    {
        StopLooking();
    }
    void ToInteractionView()
    {
        StartLooking();
    }

    public void StartAiming()
    {
        StopCor(_smoothAimingHandCor);
        _smoothAimingHandCor = StartCoroutine(SmoothAimingHandCor());

        StopCor(_dereaseWeightOfHandCor);
        _increaseWeightOfHandCor = StartCoroutine(IncreaseWeightOfHandCor());
    }

    public void StartLooking()
    {
        StopCor(_returnLookDirectionCor);

        LookingRig.weight = 1.0f;
    }
    public void StopLooking()
    {
        StopCor(_returnLookDirectionCor);
        _returnLookDirectionCor = StartCoroutine(SmoothReturnLookDirectionCor());
        StopCor(_smoothAimingHandCor);

        StopCor(_increaseWeightOfHandCor);
        _dereaseWeightOfHandCor = StartCoroutine(DecreaseWeightOfHandCor());
    }

    void VisualiseRecoil()
    {
        StopCor(_recoilVisualizsationCor);
        _recoilVisualizsationCor = StartCoroutine(RecoilVisualizsationCor());
    }

    void StartReloading()
    {
        SetMacWeights(MacHead, 0, 0.7f);
        SetMacWeights(MacChest, 0, 0.1f);
        _isReloading = true;

        AimingRig.weight = 0;
        FingersRig.weight = ReloadingRig.weight = 1;
    }
    void StopReloading()
    {
        SetMacWeights(MacHead, 0.7f, 0);
        SetMacWeights(MacChest, 0.1f, 0);
        _isReloading = false;

        FingersRig.weight = ReloadingRig.weight = 0;
    }

    void InteractionGrab()
    {
        FingersRig.weight = 0;
    }
    void InteractionRelease()
    {
        if (_isReloading) FingersRig.weight = 1;
    }

    //cors
    IEnumerator SmoothReturnLookDirectionCor()
    {
        _curLookDirectionDistance = GetDistance(LookDirection, Vector2.zero);
        float firstDistance = _curLookDirectionDistance;

        while (_curLookDirectionDistance > 0.1f)
        {
            LookDirection = Vector2.Lerp(LookDirection, Vector2.zero, 1.5f * Time.deltaTime);
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
            _randomAimingOffsetPos = GetPos(player.LookingTargetTransform) + GetRandomVector3(0.1f);
            _curHandAimingDistance = GetDistance(GetPos(AimingHandTargetTransform), _randomAimingOffsetPos);

            while (_curHandAimingDistance > 0.1f && _curHandAimingDistance < 0.2f)//change later
            {
                AimingHandTargetTransform.position = Vector3.Lerp(GetPos(AimingHandTargetTransform), _randomAimingOffsetPos, 1.5f * Time.deltaTime);
                _curHandAimingDistance = GetDistance(GetPos(AimingHandTargetTransform), _randomAimingOffsetPos);

                yield return null;
            }

            yield return null;
        }
    }

    IEnumerator RecoilVisualizsationCor()
    {
        StopCor(_lerpRecoilUpCor);
        yield return _lerpRecoilUpCor =  StartCoroutine(SmoothlyLerpLocalPosCor(RecoilTargetTransform, (Vector2)GetLocalPos(RecoilTargetTransform) + Vector2.up * GunRecoilPower + GetRandomVector2(0.05f), 0.3f, GunRecoilSpeed));

        StopCor(_lerpRecoilDownCor);
        yield return _lerpRecoilDownCor = StartCoroutine(SmoothlyLerpLocalPosCor(RecoilTargetTransform, Vector2.zero, 0.025f, GunRecoilReturnSpeed));
    }

    IEnumerator IncreaseWeightOfHandCor()
    {
        while (ArmWeight < 1f)
        {
            ArmWeight = Mathf.Lerp(ArmWeight, 1.05f, _curDurability * Time.deltaTime);

            yield return null;
        }

        ArmWeight = 1;
    }
    IEnumerator DecreaseWeightOfHandCor()
    {
        while (ArmWeight > 0)
        {
            ArmWeight = Mathf.Lerp(ArmWeight, -0.05f, Durability * Time.deltaTime);

            yield return null;
        }

        ArmWeight = 0;

        NotifyObserver(EnumsActions.OnStoppedAiming);
    }

    //other methods
    float GetDistance(Vector2 distance0, Vector2 distance1)
    {
        return Vector2.Distance(distance0, distance1);
    }

    Vector3 GetPos(Transform transform)
    {
        return transform.position;
    }
    Vector3 GetLocalPos(Transform transform)
    {
        return transform.localPosition;
    }

    Vector3 GetRandomVector3(float offset)
    {
        return new Vector3(GetRandomFloat(offset), GetRandomFloat(offset), GetRandomFloat(offset));
    }
    Vector2 GetRandomVector2(float offset)
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

    void SetMacWeights(MultiAimConstraint mac, float weight0, float weight1)
    {
        var sources = mac.data.sourceObjects;
        sources.SetWeight(0, weight0);
        sources.SetWeight(1, weight1);
        mac.data.sourceObjects = sources;
    }

    void ToggleGO(GameObject GO, bool toggle)
    {
        GO.SetActive(toggle);
    }

    //other cors
    IEnumerator SmoothlyLerpLocalPosCor(Transform transf, Vector2 endPos, float offsetDistance, float addSpeed)
    {
        Debug.Log(endPos);

        float curDistance = GetDistance(GetLocalPos(transf), endPos);

        while (curDistance > offsetDistance)
        {
            transf.localPosition = Vector2.Lerp(GetLocalPos(transf), endPos, addSpeed * Time.deltaTime);
            curDistance = GetDistance(GetLocalPos(transf), endPos);

            yield return null;
        }
    }
}
