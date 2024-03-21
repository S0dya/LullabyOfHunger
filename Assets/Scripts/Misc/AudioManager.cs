using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : SingletonSubject<AudioManager>
{
    [Header("Sounds")]
    [SerializeField] KeyValueSound[] KvSounds;
    [SerializeField] EnumValueSound[] EvSounds;

    Dictionary<string, EventInstance> _eventInstancesDict = new Dictionary<string, EventInstance>();
    Dictionary<EnumsActions, EventInstance> _enumInstancesDict = new Dictionary<EnumsActions, EventInstance>();

    //initialization
    protected override void Awake()
    {
        base.Awake();

        CreateInstance();
    }

    void Start()
    {
        foreach (var kvSound in KvSounds) _eventInstancesDict.Add(kvSound.Name, CreateInstance(kvSound.Sound));
        foreach (var evSound in EvSounds) _enumInstancesDict.Add(evSound.EnumAction, CreateInstance(evSound.Sound));
    }
    EventInstance CreateInstance(EventReference sound)
    {
        return RuntimeManager.CreateInstance(sound);
    }

    //main methods
    public void PlayOneShot(string sound) => _eventInstancesDict[sound].start();
    public void PlayOneShot(EventInstance sound) => sound.start();

    public void PlayOneShot(string sound, Vector3 position)
    {
        _eventInstancesDict[sound].set3DAttributes(RuntimeUtils.To3DAttributes(position));
        _eventInstancesDict[sound].start();
    }

    public void PlayOneShot(EnumsActions enumAction)
    {
        if (_enumInstancesDict.ContainsKey(enumAction)) _enumInstancesDict[enumAction].start();
    }

    public override void PerformAction(EnumsActions actionEnum)
    {
        PlayOneShot(actionEnum);
    }

    //settings
    public void SetVolume(int index, float volume)
    {
        RuntimeManager.GetBus((index == 0? "bus:/MUSIC" : "bus:/SFX")).setVolume(volume);
        Settings.musicStats[index] = volume;
    }
}

[System.Serializable]
class KeyValueSound
{
    [SerializeField] public string Name;
    [SerializeField] public EventReference Sound;
}

[System.Serializable]
class EnumValueSound
{
    [SerializeField] public EnumsActions EnumAction;
    [SerializeField] public EventReference Sound;
}
