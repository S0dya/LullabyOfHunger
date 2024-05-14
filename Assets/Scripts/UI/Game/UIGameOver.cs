using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class UIGameOver : UISingletonMonobehaviour<UIGameOver>
{
    [Header("settings")]
    [SerializeField] float TextFadeDuration = 1;
    [SerializeField] float TextScaleDuration = 0.9f;
    [SerializeField] float SwitchingDelayDuration = 1;

    [SerializeField] [Range(0.5f, 1)] float TextInitialScale = 0.9f;

    [Header("UI")]
    [SerializeField] RectTransform TextTransf;

    //local
    CanvasGroup _textCG;
    TextMeshProUGUI _textText; //remove later

    bool _forDemoEnd;

    protected override void Awake()
    {
        base.Awake();
        
        _textCG = TextTransf.gameObject.GetComponent<CanvasGroup>(); _textCG.alpha = 0;
        _textText = TextTransf.gameObject.GetComponent<TextMeshProUGUI>();
        TextTransf.localScale = new Vector3(TextInitialScale, TextInitialScale, TextInitialScale);
    }

    //outside methods
    public void OpenTab()
    {
        Observer.Instance.NotifyObservers(EnumsActions.OnGameOver);

        FadeSetTime(1, 0.8f, ShowText);
    }

    public void DemoEndOpenTab()
    {
        _textText.text = "Thanks for playing the demo!"; _textText.color = Color.white;
        _forDemoEnd = true;

        OpenTab();
    }

    //other methods
    void ShowText()
    {
        _textCG.DOFade(1, TextFadeDuration).SetUpdate(true).SetEase(Ease.InOutQuint);
        TextTransf.DOScale(1, TextScaleDuration).SetUpdate(true).SetEase(Ease.OutQuad).OnComplete(() => StartCoroutine(DelaySceneSwitchingCor()));
    }

    //cors
    IEnumerator DelaySceneSwitchingCor()
    {
        yield return new WaitForSecondsRealtime(_forDemoEnd ? 4 : SwitchingDelayDuration);

        LoadingScene.Instance.OpenScene(SceneNameEnum.Menu);
    }
}
