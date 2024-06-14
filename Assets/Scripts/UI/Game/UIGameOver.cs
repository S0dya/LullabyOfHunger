using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIGameOver : UISingletonMonobehaviour<UIGameOver>
{
    [Header("settings")]
    [SerializeField] float TextFadeDuration = 1;
    [SerializeField] float TextScaleDuration = 0.9f;
    [SerializeField] float SwitchingDelayDuration = 4;

    [SerializeField] [Range(0.5f, 1)] float TextInitialScale = 0.9f;

    [Header("UI")]
    [SerializeField] RectTransform TextTransf;
    [SerializeField] Color VictoryTextColor;

    [SerializeField] Image BgImage;

    //local
    CanvasGroup _textCG;
    TextMeshProUGUI _textText; //remove later

    protected override void Awake()
    {
        base.Awake();

        _textText = TextTransf.GetComponent<TextMeshProUGUI>();
        _textCG = TextTransf.gameObject.GetComponent<CanvasGroup>(); _textCG.alpha = 0;
        TextTransf.localScale = new Vector3(TextInitialScale, TextInitialScale, TextInitialScale);
    }

    //outside methods
    public void OpenTab()
    {
        Observer.Instance.NotifyObservers(EnumsActions.OnGameOver);

        FadeSetTime(1, 0.8f, ShowText);
    }

    public void VictoryOpenTab()
    {
        _textText.text = GameManager.Instance.GetLocalizedString("VictoryKey") + "!"; _textText.color = VictoryTextColor;
        BgImage.color = Color.white;

        AudioManager.Instance.PlayOneShot("Victory");

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
        yield return new WaitForSecondsRealtime(SwitchingDelayDuration);

        LoadingScene.Instance.OpenScene(SceneNameEnum.Menu);
    }
}
