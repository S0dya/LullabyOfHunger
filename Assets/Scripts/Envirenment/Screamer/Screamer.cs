using UnityEngine;

public class Screamer : MonoBehaviour //myb rename later
{
    [SerializeField] GameObject ScreamerObj;
    [SerializeField] GameObject[] OtherObjs;

    [SerializeField] ScreamerNameEnum ScreamerName;

    //local
    Animator _animator;

    Animator[] _otherAnimators;
    int _animatorsN;

    void Awake()
    {
        _animator = ScreamerObj.GetComponent<Animator>();

        _animatorsN = OtherObjs.Length;
        if (_animatorsN > 0)
        {
            _otherAnimators = new Animator[_animatorsN];

            for (int i = 0; i < _animatorsN; i++) _otherAnimators[i] = OtherObjs[i].GetComponent<Animator>();
        }

        ToggleObj(false);
    }

    //outside methods
    public void PlayScreamer()
    {
        ToggleObj(true);

        _animator.Play(ScreamerName.ToString());
        if (_animatorsN > 0) foreach (var otherAnim in _otherAnimators) otherAnim.Play(ScreamerName.ToString());
    }

    public void ToggleObj(bool toggle) => ScreamerObj.SetActive(toggle);
}

public enum ScreamerNameEnum
{
    none, 

    FlatHallwayKitchenCheck,
    FlatLullabyRoom,
    HallwayDoorClosing,
    HallwayAboveDoor,
}