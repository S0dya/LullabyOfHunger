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
    [SerializeField] Button ContinueButton;
    [SerializeField] TextMeshProUGUI ContinueButtonText;

    [Header("other")]
    [SerializeField] InteractionObject[] InteractionItems;

    //local

    bool _scaledUp;

    //threshold
    InteractionObject _curInteractionObj;
    Transform _curInteractionitemObjTransf;

    InteractionItem _curInteractionItem;
    InteractionItemDestroyable _curInteractionItemDestroyable;

    Vector3 _curItemTargetScale;

    //cors
    Coroutine _rotateObjCor;

    //props
    bool Interactable
    {
        get { return _scaledUp; }
        set { _scaledUp = ContinueButton.interactable = value; }
    }

    //buttons
    public void Continue()
    {
        if (!MainCG.blocksRaycasts || !_scaledUp) return;

        _curInteractionObj.Object.SetActive(false);
        StopCoroutine(_rotateObjCor);

        FadeSetTime(0, 0.1f, NotifyOnSwitchOff);

        if (_curInteractionItemDestroyable != null) _curInteractionItemDestroyable.DestroyObj();
        else _curInteractionItem.ToggleObj(true);

        if (_curInteractionObj.ActionOnPicked != null) _curInteractionObj.ActionOnPicked.Invoke();
    }

    //outside methods
    public void StartInteraction(InteractionItem interactionItem, InteractionItemEnum interactionItemEnum)
    {
        Interactable = false;
        FadeSetTime(1, 0.1f, NotifyOnSwitchOn);
        _curInteractionItem = interactionItem; _curInteractionItem.ToggleObj(false);
        
        _curInteractionItemDestroyable = null;
        if (_curInteractionItem is InteractionItemDestroyable) 
            _curInteractionItemDestroyable = _curInteractionItem as InteractionItemDestroyable;

        _curInteractionObj = InteractionItems.FirstOrDefault(item => item.ItemName == interactionItemEnum);
        _curInteractionitemObjTransf = _curInteractionObj.Object.transform;

        _curInteractionObj.Object.SetActive(true); 
        _curItemTargetScale = _curInteractionitemObjTransf.localScale; _curInteractionitemObjTransf.localScale = Vector3.zero;
        
        _curInteractionitemObjTransf.DOScale(_curItemTargetScale, ScaleSpeed).SetUpdate(true).SetEase(Ease.OutQuad).OnComplete(OnScaledUp);
        _rotateObjCor = StartCoroutine(RotateObjCor());

        ContinueButtonText.text = (_curInteractionItemDestroyable != null ? "Equip" : "Continue");
    }

    //other methods
    void NotifyOnSwitchOn() => Observer.Instance.NotifyObservers(EnumsActions.OnSwitchToInteraction);
    void NotifyOnSwitchOff() => Observer.Instance.NotifyObservers(EnumsActions.OnSwitchToIsometric);

    void OnScaledUp() => Interactable = true;

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

    [SerializeField] public UnityEvent ActionOnPicked;
}

public enum InteractionItemEnum
{
    none,

    Handgun,
    Magazine,
    
    FlatIcon,
    
    GasMaskMC,
    GasMaskChild,
    
    ParadizePicture,

    WomanFramePhoto,
    PoliceOfficerFramePhoto,
    WedingFramePhoto,
    HospitalFramePhoto,

    Haloperidol,
}
