using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Animations;

public class RiggingController : Subject
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
    [SerializeField] AimConstraint ArmRig;
    [SerializeField] Rig ReloadingRig;
    [SerializeField] Rig FingersRig;

    [SerializeField] MultiAimConstraint MacHead;
    [SerializeField] MultiAimConstraint MacChest;
    [SerializeField] TwoBoneIKConstraint TbikcArm;

    [SerializeField] Transform RecoilTargetTransform;
    [SerializeField] Transform AimingHandTargetTransform;

    [Header("reloading")]
    [SerializeField] Transform HandTransform;

    [SerializeField] GameObject GunHandObj;
    [SerializeField] GameObject MagHandObj;
    [SerializeField] GameObject EmptyMagHandObj;
    [SerializeField] GameObject GunReloadObj;

    [SerializeField] Transform MagsBagTransform;

    [SerializeField] GameObject EmptyMagPrefab;
    [SerializeField] GameObject FullMagPrefab;

    //local
    Player player;

    Transform GunReloadTransform;
    Transform ItemsParent;

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

    //reloading
    bool _gunHasMag = true;

    bool _isHoldingFullMag;
    bool _isHoldingEmptyMag;

    //cors
    Coroutine _returnLookDirectionCor;
    Coroutine _smoothAimingHandCor;
    Coroutine _recoilVisualizsationCor;

    Coroutine _increaseWeightOfHandCor;
    Coroutine _dereaseWeightOfHandCor;


    //serialization
    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();

        AddAction(EnumsActions.OnSwitchToFirstPerson, ToFirstPersonView);
        AddAction(EnumsActions.OnSwitchToIsometric, ToIsometricView);
        AddAction(EnumsActions.OnSwitchToInteraction, ToInteractionView);
        AddAction(EnumsActions.OnReload, StartReloading);
        AddAction(EnumsActions.OnReloadingStop, StopReloading);

        AddAction(EnumsActions.OnFire, VisualiseRecoil);

        AddAction(EnumsActions.OnInteractionGrab, InteractionGrab);
        AddAction(EnumsActions.OnInteractionRelease, InteractionRelease);
    }

    void Start()
    {
        _cameraAimingOffset = new Vector2(Screen.width / 2, Screen.height / 2);

        GunReloadTransform = GunReloadObj.transform;
        ItemsParent = GameObject.FindGameObjectWithTag("ItemsParent").transform;
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
        AimingRig.weight = 1;

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
        TbikcArm.data.target = player.ReloadingTargetTransform;

        AimingRig.weight = 0;
        ReloadingRig.weight = 1;
        ToggleGO(GunHandObj, false);
        ToggleGO(GunReloadObj, true);
    }
    void StopReloading()
    {
        SetMacWeights(MacHead, 0.7f, 0);
        SetMacWeights(MacChest, 0.1f, 0);
        TbikcArm.data.target = player.LookingTargetTransform;

        ReloadingRig.weight = 0;
        ToggleGO(GunHandObj, true);
        ToggleGO(GunReloadObj, false);
    }

    void InteractionGrab()
    {
        FingersRig.weight = 0;

        if (GetDistance(GetPos(HandTransform), GetPos(MagsBagTransform)) < 0.3f)
        {
            TakeMagFromBag();
        }
        else if (GetDistance(GetPos(HandTransform), GetPos(GunReloadTransform)) < 0.3f)
        {
            if (_gunHasMag) TakeMagFromGun();
        }
    }

    void InteractionRelease()
    {
        FingersRig.weight = 1;

        if (_isHoldingFullMag && GetDistance(GetPos(HandTransform), GetPos(GunReloadTransform)) < 0.3f)
        {
            if (_gunHasMag) TakeMagFromGun();
        }
        else if (_isHoldingFullMag || _isHoldingEmptyMag)
        {
            DropMag();
        }
    }

    //reloading methods
    void TakeMagFromBag()
    {
        _isHoldingFullMag = true;

        ToggleGO(MagHandObj, true);
    }

    void TakeMagFromGun()
    {
        _isHoldingEmptyMag = true;
        _gunHasMag = false;

        ToggleGO(MagHandObj, true);
    }

    void DropMag()
    {
        Instantiate(_isHoldingEmptyMag ? EmptyMagPrefab : FullMagPrefab, HandTransform.position, HandTransform.rotation, ItemsParent);

        ToggleGO(MagHandObj, false);
    }

    void PutMagInGun()
    {
        _gunHasMag = true;

        Observer.Instance.NotifyObservers(EnumsActions.OnReloadingStop);
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
        yield return StartCoroutine(SmoothlyLerpLocalPosCor(RecoilTargetTransform, (Vector2)GetLocalPos(RecoilTargetTransform) + Vector2.up * GunRecoilPower + GetRandomVector2(0.05f), 0.3f, GunRecoilSpeed));
        yield return StartCoroutine(SmoothlyLerpLocalPosCor(RecoilTargetTransform, Vector2.zero, 0.1f, GunRecoilReturnSpeed));
    }

    IEnumerator IncreaseWeightOfHandCor()
    {
        while (AimingRig.weight < 1f)
        {
            ArmRig.weight = AimingRig.weight = Mathf.Lerp(AimingRig.weight, 1.05f, _curDurability * Time.deltaTime);

            yield return null;
        }

        ArmRig.weight = AimingRig.weight = 1;
    }
    IEnumerator DecreaseWeightOfHandCor()
    {
        while (AimingRig.weight > 0)
        {
            ArmRig.weight = AimingRig.weight = Mathf.Lerp(AimingRig.weight, -0.05f, Durability * Time.deltaTime);

            yield return null;
        }

        ArmRig.weight = AimingRig.weight = 0;
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
        float curDistance = GetDistance(GetLocalPos(transf), endPos);

        while (curDistance > offsetDistance)
        {
            transf.localPosition = Vector2.Lerp(GetLocalPos(transf), endPos, addSpeed * Time.deltaTime);
            curDistance = GetDistance(GetLocalPos(transf), endPos);

            yield return null;
        }
    }
}
