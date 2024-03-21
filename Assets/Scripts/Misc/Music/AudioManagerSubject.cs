using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerSubject : SingletonSubject<AudioManagerSubject>
{
    AudioManager _audioManager;

    protected override void Awake()
    {
        base.Awake();

        _audioManager = GetComponent<AudioManager>();

        //AddAction(EnumsActions.SkillUsed, OnSkillUsed);
    }

    //actions

    public override void PerformAction(EnumsActions actionEnum)
    {
        _audioManager.PlayOneShot(actionEnum);
    }
    //void OnSkillUsed() => AudioManager.Instance.PlayOneShot("OnSkillUsed");
}
