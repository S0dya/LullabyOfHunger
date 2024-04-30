using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : SingletonSubject<GunController>
{
    [Header("settings")]
    public int BulletsInMagazine = 7;

    public float GunInteractionDistance = 0.17f;
    public float MagInteractionDistance = 0.12f;

    public float ShotEffectTime;

    [Header("gun")]
    [SerializeField] Transform ShootingTransform;

    [SerializeField] GameObject BulletTrackPrefab;
    
    [SerializeField] Animator Animator;

    [SerializeField] LayerMask RayCollidingLayer;

    [Header("Effects")]
    [SerializeField] GameObject ShotEffect;

    [Header("reloading")]
    [SerializeField] Transform HandTargetTransform;
    [SerializeField] Transform HandOriginTransform;

    [SerializeField] GameObject GunHandObj;
    [SerializeField] GameObject MagHandObj;
    [SerializeField] GameObject EmptyMagHandObj;
    [SerializeField] GameObject GunReloadObj;
    [SerializeField] GameObject[] InGunMagObjs = new GameObject[2];

    [SerializeField] Transform MagsBagTransform;

    [SerializeField] GameObject EmptyMagPrefab;
    [SerializeField] GameObject FullMagPrefab;

    //local
    MagsBag _magsBag;

    Transform _gunReloadTransform;
    Transform _emptyMagHandTransf;

    Transform _itemsParent;
    Transform _effectsParent;

    Light _shotLight;

    //gun
    [HideInInspector] public int _curBulletsAmount;

    //animator 
    int _animIDShoot;
    int _animIDShootLast;
    int _animIDReloadLast;

    //reloading
    bool _gunHasMag = true;

    bool _isHoldingFullMag;
    bool _isHoldingEmptyMag;

    //cors 
    Coroutine _visualiseShotCor;

    protected override void Awake()
    {
        base.Awake();

        _magsBag = GetComponent<MagsBag>();

        _gunReloadTransform = GunReloadObj.transform;
        _emptyMagHandTransf = EmptyMagHandObj.transform;

        _shotLight = GunHandObj.GetComponentInChildren<Light>();

        _itemsParent = GameObject.FindGameObjectWithTag("ItemsParent").transform;
        _effectsParent = GameObject.FindGameObjectWithTag("EffectsParent").transform;

        AddAction(EnumsActions.OnFire, Shoot);
        AddAction(EnumsActions.OnRecieverReloaded, RecieverReloaded);

        AddAction(EnumsActions.OnReload, StartReloading);
        AddAction(EnumsActions.OnStopReloading, StopReloading);
        AddAction(EnumsActions.OnInteractionGrab, InteractionGrab);
        AddAction(EnumsActions.OnInteractionRelease, InteractionRelease);

        _animIDShoot = Animator.StringToHash("Shoot");
        _animIDShootLast = Animator.StringToHash("ShootLast");
        _animIDReloadLast = Animator.StringToHash("ReloadLast");
    }

    void Start()
    {
        _curBulletsAmount = Settings.curBulletsAmount;

    }

    //actions
    void Shoot()
    {
        _curBulletsAmount--;

        if (Physics.Raycast(ShootingTransform.position, ShootingTransform.forward, out RaycastHit hit, 1000, RayCollidingLayer))
        {
            var enemyAnimationController = hit.collider.GetComponentInParent<EnemyAnimationController>();

            if (enemyAnimationController != null)
            {
                Vector3 force = (hit.point - ShootingTransform.position).normalized; force.y = 0;

                enemyAnimationController.Shot(force, hit.point);
            }
            else
            {
                Instantiate(BulletTrackPrefab, hit.point, Quaternion.identity, hit.collider.transform);
            }

        }

        Animator.Play(_curBulletsAmount == 0 ? _animIDShootLast : _animIDShoot);

        if (_visualiseShotCor != null) StopCoroutine(_visualiseShotCor);
        _visualiseShotCor = StartCoroutine(VisualiseShotCor());
    }

    //reloading
    void StartReloading()
    {
        ToggleGO(GunHandObj, false);
        ToggleGO(GunReloadObj, true);
    }
    void StopReloading()
    {
        if (_isHoldingEmptyMag || _isHoldingFullMag) DropMag();

        ToggleGO(GunHandObj, true);
        ToggleGO(GunReloadObj, false);
    }

    void InteractionGrab()
    {
        if (GetDistance(GetPos(HandTargetTransform), GetPos(MagsBagTransform)) < MagInteractionDistance)
        {
            TakeMagFromBag();
        }
        else if (GetDistance(GetPos(HandTargetTransform), GetPos(_gunReloadTransform)) < GunInteractionDistance)
        {
            if (_gunHasMag) TakeMagFromGun();
        }
    }
    void InteractionRelease()
    {
        if (_isHoldingFullMag && !_gunHasMag && GetDistance(GetPos(HandTargetTransform), GetPos(_gunReloadTransform)) < GunInteractionDistance)
        {
            PutMagInGun();
        }
        else if (_isHoldingFullMag || _isHoldingEmptyMag)
        {
            DropMag();
        }
    }

    void RecieverReloaded()
    {
        Animator.Play(_animIDReloadLast);

        //myb add more interesting things here
    }

    //reloading methods
    void TakeMagFromBag()
    {
        if (!_magsBag.HasMags()) return;

        NotifyObserver(EnumsActions.OnTakeMagFromBag);

        _isHoldingFullMag = true;
        ToggleGO(MagHandObj, true);
    }
    
    void TakeMagFromGun()
    {
        AudioManager.Instance.PlayOneShot("TakeMagFromGun");

        if (_curBulletsAmount > 1) _curBulletsAmount = 1;

        _isHoldingEmptyMag = true;
        _gunHasMag = false;
        ToggleGO(EmptyMagHandObj, true);
        for (int i = 0; i < 2; i++) ToggleGO(InGunMagObjs[i], false);
    }
    void PutMagInGun()
    {
        AudioManager.Instance.PlayOneShot("PutMagInGun");

        if (_curBulletsAmount == 0)
        {
            NotifyObserver(EnumsActions.OnRecieverReloaded);
        }

        _curBulletsAmount += 7;
        _gunHasMag = true;
        _isHoldingFullMag = false;
        ToggleGO(MagHandObj, false);
        for (int i = 0; i < 2; i++) ToggleGO(InGunMagObjs[i], true);

        //NotifyObserver(EnumsActions.OnStopReloading);
    }

    void DropMag()
    {
        Instantiate(_isHoldingEmptyMag ? EmptyMagPrefab : FullMagPrefab, _emptyMagHandTransf.position, _emptyMagHandTransf.rotation, _itemsParent);

        ToggleGO(_isHoldingEmptyMag ? EmptyMagHandObj : MagHandObj, false);
        _isHoldingEmptyMag = _isHoldingFullMag = false;
    }

    //cors
    IEnumerator VisualiseShotCor()
    {
        Instantiate(ShotEffect, ShootingTransform.position, Quaternion.identity, _effectsParent);

        _shotLight.enabled = true;
        yield return new WaitForSeconds(ShotEffectTime);
        _shotLight.enabled = false;
    }


    //other methods 
    float GetDistance(Vector3 distance0, Vector3 distance1)
    {
        return Vector3.Distance(distance0, distance1);
    }
    
    Vector3 GetPos(Transform transform)
    {
        return transform.position;
    }
    
    void ToggleGO(GameObject GO, bool toggle)
    {
        GO.SetActive(toggle);
    }

    //outside methods
    public Vector2 GetHandOriginPos()
    {
        return new Vector2(-HandOriginTransform.localPosition.x, HandOriginTransform.localPosition.y);
    }
}
