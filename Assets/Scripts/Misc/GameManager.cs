using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using TMPro;

[DefaultExecutionOrder(-1)]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    protected override void Awake()
    {
        base.Awake();

        //LoadData();
        //Settings.firstTime = false;
    }

    //UI

    /*
    public void Open(CanvasGroup CG, float duration) => Open(CG, duration, 1);
    public void Open(CanvasGroup CG, float duration, float endVal)
    {
        LTDescr tween = LeanTween.alphaCanvas(CG, endVal, duration).setEase(LeanTweenType.easeInOutQuad);
        CG.blocksRaycasts = true;
        SetUseEstimatedTime(tween);
    }

    public void Close(CanvasGroup CG, float duration)
    {
        LTDescr tween = LeanTween.alphaCanvas(CG, 0, duration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => CloseComletely(CG));
        SetUseEstimatedTime(tween);
    }
    void CloseComletely(CanvasGroup CG) => CG.blocksRaycasts = false;

    public void FadeIn(CanvasGroup CG, float durationStart) => LeanTween.alphaCanvas(CG, 1f, durationStart).setEase(LeanTweenType.easeInOutQuad);
    public void FadeInAndOut(CanvasGroup CG, float durationStart, float durationEnd) => LeanTween.alphaCanvas(CG, 1f, durationStart).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => FadeOut(CG, durationEnd));
    public void FadeOut(CanvasGroup CG, float durationEnd) => LeanTween.alphaCanvas(CG, 0f, durationEnd).setEase(LeanTweenType.easeInOutQuad);

    public void FadeInAndOutAndDestroy(GameObject gO, float durationStart, float durationEnd)
    {
        CanvasGroup CG = gO.GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(CG, 1f, durationStart).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => FadeOutAndDestroy(gO, CG, durationEnd));
    }
    public void FadeOutAndDestroy(GameObject gO, CanvasGroup CG, float durationEnd)
    {
        LeanTween.alphaCanvas(CG, 0f, durationEnd).setEase(LeanTweenType.easeInOutQuad);
        Destroy(gO);
    }

    public void MoveTransform(RectTransform transform, float x, float y, float duration)
    {
        LTDescr tween = LeanTween.move(transform, new Vector2(x, y), duration).setEaseOutQuad();
        SetUseEstimatedTime(tween);
    }

    public void ChangeScale(GameObject obj, float scale, float duration)
    {
        LTDescr tween = LeanTween.scale(obj, new Vector2(scale, scale), duration).setEase(LeanTweenType.easeOutBack);
        SetUseEstimatedTime(tween);
    }

    public void SetUseEstimatedTime(LTDescr tween) => tween.setUseEstimatedTime(true);
    */

    //save/load
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveData();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveData();
        }
    }

    void OnApplicationQuit()
    {
        SaveData();
    }

    public void SaveData()
    {
        //for (int i = 0; i < Settings.unlockedMaps.Length; i++) SaveBool($"unlockedMaps {i}", Settings.unlockedMaps[i]);
    }

    public void LoadData() 
    {
        /*
        if (LoadBool("firstTime")) return;

        for (int i = 0; i < Settings.charactersPrices.Length; i++) Settings.charactersPrices[i] = LoadInt($"charactersPrices {i}");
        */
    }

    void SaveInt(string name, int val) => PlayerPrefs.SetInt(name, val);
    void SaveFloat(string name, float val) => PlayerPrefs.SetFloat(name, val);
    void SaveBool(string name, bool val) => PlayerPrefs.SetInt(name, val ? 0 : 1);

    int LoadInt(string name)
    {
        return PlayerPrefs.GetInt(name);
    }
    float LoadFloat(string name)
    {
        return PlayerPrefs.GetFloat(name);
    }
    bool LoadBool(string name)
    {
        return PlayerPrefs.GetInt(name) == 0;
    }
}
