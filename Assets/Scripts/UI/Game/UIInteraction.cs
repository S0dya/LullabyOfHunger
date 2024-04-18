using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;


public class UIInteraction : UISingletonMonobehaviour<UIInteraction>
{
    [Header("settings")]
    [SerializeField] float ScaleSpeed = 1;
    [SerializeField] float RotationSpeed = 1;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI ContinueButtonText;

    [Header("other")]
    [SerializeField] InteractionObject[] InteractionItems;

    //local

    bool _scaledUp;

    //threshold
    InteractionObject _curInteractionObj;
    Transform _curInteractionitemObjTransf;

    InteractionItemDestroyable _curInteractionItem;
    Vector3 _curItemTargetScale;

    //cors
    Coroutine _rotateObjCor;

    //buttons
    public void Continue()
    {
        if (!MainCG.blocksRaycasts || !_scaledUp) return;

        _curInteractionObj.Object.SetActive(false);
        StopCoroutine(_rotateObjCor);

        FadeSetTime(0, 0.1f, NotifyOnSwitchOff);

        if (_curInteractionObj.Pickable && _curInteractionObj.ActionOnPicked != null)
        {
            _curInteractionObj.ActionOnPicked.Invoke();

            _curInteractionItem.DisableObj();
        }
    }

    //outside methods
    public void StartInteraction(InteractionItemDestroyable interactionItme, InteractionItemEnum interactionItemEnum)
    {
        _curInteractionItem = interactionItme;

        StartInteraction(interactionItemEnum);
    }
    public void StartInteraction(InteractionItemEnum interactionItemEnum)
    {
        FadeSetTime(1, 0.1f, NotifyOnSwitchOn);

        _curInteractionObj = InteractionItems.FirstOrDefault(item => item.ItemName == interactionItemEnum);
        _curInteractionitemObjTransf = _curInteractionObj.Object.transform;

        _curInteractionObj.Object.SetActive(true); _scaledUp = false;

        _curItemTargetScale = _curInteractionitemObjTransf.localScale; _curInteractionitemObjTransf.localScale = Vector3.zero;


        _curInteractionitemObjTransf.DOScale(_curItemTargetScale, ScaleSpeed).SetUpdate(true).SetEase(Ease.OutQuad).OnComplete(OnScaledUp);

        _rotateObjCor = StartCoroutine(RotateObjCor());

        ContinueButtonText.text = (_curInteractionObj.Pickable ? "Equip" : "Continue");
    }

    //other methods
    void NotifyOnSwitchOn() => Observer.Instance.NotifyObservers(EnumsActions.OnSwitchToInteraction);
    void NotifyOnSwitchOff() => Observer.Instance.NotifyObservers(EnumsActions.OnSwitchToIsometric);

    void OnScaledUp() => _scaledUp = true;

    //cors 
    IEnumerator RotateObjCor()
    {
        while (true)
        {
            _curInteractionitemObjTransf.rotation *= Quaternion.Euler(0, 0, Time.fixedDeltaTime * RotationSpeed);

            yield return null;
        }
    }

}

[System.Serializable]
public class InteractionObject
{
    [SerializeField] public InteractionItemEnum ItemName;
    [SerializeField] public GameObject Object;

    [SerializeField] public bool Pickable;
    [SerializeField] public UnityEvent ActionOnPicked;
}

public enum InteractionItemEnum
{
    none,

    Handgun,
    Magazine,

}
