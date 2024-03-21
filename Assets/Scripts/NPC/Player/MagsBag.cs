using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagsBag : SingletonSubject<MagsBag>
{
    [SerializeField] SkinnedMeshRenderer[] MagsObjs = new SkinnedMeshRenderer[6];

    int _lastMagIndex = 5;

    protected override void Awake()
    {
        base.Awake();

        AddAction(EnumsActions.OnTakeMagFromBag, TakeMagFromBag);
    }

    //actions
    void TakeMagFromBag()
    {
        MagsObjs[_lastMagIndex].enabled = false;

        _lastMagIndex--;
    }
}
