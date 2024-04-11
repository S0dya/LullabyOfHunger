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

    //threshold
    InteractionObject _curInteractionItem;
    Transform _curInteractionitemObjTransf;

    Tweener _curRotationTweener;

    //buttons
    public void Continue()
    {
        FadeSetTime(0, 0.05f, NotifyOnSwitchOff);

        if (_curInteractionItem.Pickable)
        {
            _curInteractionItem.ActionOnPicked.Invoke();
        }
    }

    //outside methods
    public void StartInteraction(InteractionItemEnum interactionItemEnum)
    {
        FadeSetTime(1, 0.1f, NotifyOnSwitchOn);

        _curInteractionItem = InteractionItems.OrderBy(item => item.ItemName).FirstOrDefault(item => item.ItemName == interactionItemEnum);
        _curInteractionitemObjTransf = _curInteractionItem.Object.transform;

        _curInteractionItem.Object.SetActive(true); _curInteractionitemObjTransf.localScale = Vector3.zero;

        _curInteractionitemObjTransf.DOScale(Vector3.one, ScaleSpeed).SetEase(Ease.OutQuad);
        _curRotationTweener = _curInteractionitemObjTransf.DOLocalRotate(new Vector3(90, 0, 0), RotationSpeed, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

        ContinueButtonText.text = (_curInteractionItem.Pickable ? "Equip" : "Continue");
    }

    //other methods
    void NotifyOnSwitchOn() => Observer.Instance.NotifyObservers(EnumsActions.OnSwitchToInteraction);
    void NotifyOnSwitchOff() => Observer.Instance.NotifyObservers(EnumsActions.OnSwitchToIsometric);

}

[System.Serializable]
class InteractionObject
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
