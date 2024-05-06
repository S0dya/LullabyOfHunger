using UnityEngine;

public class Screamer : MonoBehaviour //myb rename later
{
    [SerializeField] GameObject ScreamerObj;

    [SerializeField] ScreamerNameEnum ScreamerName;

    //local
    Animator _animator;

    void Awake()
    {
        _animator = ScreamerObj.GetComponent<Animator>();

        ToggleObj(false);
    }

    //outside methods
    public void PlayScreamer()
    {
        ToggleObj(true);

        _animator.Play(ScreamerName.ToString());
    }

    public void ToggleObj(bool toggle) => ScreamerObj.SetActive(toggle);
}

public enum ScreamerNameEnum
{
    none, 

    FlatHallwayKitchenCheck,
}