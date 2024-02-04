using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : Subject
{
    [Header("settings")]
    public int BulletsInMagazine = 7;
    public int MagsAmount = 6;

    [Header("gun")]
    [SerializeField] Transform ShootingTransform;

    [SerializeField] GameObject BulletTrackPrefab;
    
    [SerializeField] Animator Animator;

    [Header("reloading")]
    [SerializeField] Transform HandTransform;
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

    Transform _gunReloadTransform;
    Transform _emptyMagHandTransf;

    Transform _itemsParent;
    Transform _effectsParent;

    //gun
    [HideInInspector] public int _curBulletsAmount = 8;

    //animator 
    int _animIDShoot;
    int _animIDShootLast;
    int _animIDReloadLast;

    //reloading
    bool _gunHasMag = true;

    bool _isHoldingFullMag;
    bool _isHoldingEmptyMag;

    protected override void Awake()
    {
        base.Awake();

        _gunReloadTransform = GunReloadObj.transform;
        _emptyMagHandTransf = EmptyMagHandObj.transform;

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

    //actions
    void Shoot()
    {
        _curBulletsAmount--;

        if (Physics.Raycast(ShootingTransform.position, ShootingTransform.forward, out RaycastHit hit))
        {
            Instantiate(BulletTrackPrefab, hit.point, hit.transform.rotation, _effectsParent);

            Enemy enemy = hit.collider.GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                Vector3 force = (hit.point - ShootingTransform.position).normalized;
                force.y = 0;

                enemy.Push(force, hit.point);
            }
        }

        Animator.Play(_curBulletsAmount == 0 ? _animIDShootLast : _animIDShoot);
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
        if (GetDistance(GetPos(HandTransform), GetPos(MagsBagTransform)) < 0.12f)
        {
            TakeMagFromBag();
        }
        else if (GetDistance(GetPos(HandTransform), GetPos(_gunReloadTransform)) < 0.17f)
        {
            if (_gunHasMag) TakeMagFromGun();
        }
    }
    void InteractionRelease()
    {
        if (_isHoldingFullMag && !_gunHasMag && GetDistance(GetPos(HandTransform), GetPos(_gunReloadTransform)) < 0.17f)
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
        if (MagsAmount < 0) return;

        MagsAmount--;
        NotifyObserver(EnumsActions.OnTakeMagFromBag);

        _isHoldingFullMag = true;
        ToggleGO(MagHandObj, true);
    }
    
    void TakeMagFromGun()
    {
        if (_curBulletsAmount > 1) _curBulletsAmount = 1;

        _isHoldingEmptyMag = true;
        _gunHasMag = false;
        ToggleGO(EmptyMagHandObj, true);
        for (int i = 0; i < 2; i++) ToggleGO(InGunMagObjs[i], false);
    }
    void PutMagInGun()
    {
        if (_curBulletsAmount == 0)
        {
            NotifyObserver(EnumsActions.OnRecieverReloaded);
        }

        _curBulletsAmount += 7;
        _gunHasMag = true;
        _isHoldingFullMag = false;
        ToggleGO(MagHandObj, false);
        for (int i = 0; i < 2; i++) ToggleGO(InGunMagObjs[i], true);

        NotifyObserver(EnumsActions.OnStopReloading);
    }

    void DropMag()
    {
        Instantiate(_isHoldingEmptyMag ? EmptyMagPrefab : FullMagPrefab, _emptyMagHandTransf.position, _emptyMagHandTransf.rotation, _itemsParent);

        ToggleGO(_isHoldingEmptyMag ? EmptyMagHandObj : MagHandObj, false);
        _isHoldingEmptyMag = _isHoldingFullMag = false;
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
