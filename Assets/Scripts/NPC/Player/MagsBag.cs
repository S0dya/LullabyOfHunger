using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagsBag : SingletonSubject<MagsBag>
{
    [SerializeField] SkinnedMeshRenderer[] MagsObjs = new SkinnedMeshRenderer[6];

    //local
    int _maxMagsN = 5;
    int _lastMagIndex;

    protected override void Awake()
    {
        base.Awake(); CreateInstance();

        AddAction(EnumsActions.OnTakeMagFromBag, TakeMagFromBag);
    }

    void Start()
    {
        for (int i = 0; i < Settings.curMagsN; i++) MagsObjs[i].enabled = true;
        for (int i = Settings.curMagsN; i < MagsObjs.Length; i++) MagsObjs[i].enabled = false;

        _lastMagIndex = Settings.curMagsN;
    }

    //outside methods
    public bool HasMags()
    {
        return _lastMagIndex > 0;
    }

    public void AddMag()
    {
        if (_lastMagIndex >= _maxMagsN) return;

        _lastMagIndex++; MagsObjs[_lastMagIndex].enabled = true;
    }

    //other outside methods
    public int GetMagsN()
    {
        return _lastMagIndex;
    }

    //actions
    void TakeMagFromBag()
    {
        MagsObjs[_lastMagIndex].enabled = false;

        _lastMagIndex--;
    }
}
