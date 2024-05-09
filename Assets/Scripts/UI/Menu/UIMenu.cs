using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : UISingletonMonobehaviour<UIMenu>
{

    [Header("UI")] 
    [SerializeField] CanvasGroup ContinueButtonCG;

    void Start()
    {
        Time.timeScale = 1;

        if (Settings.curScene == SceneNameEnum.None)
        {
            ContinueButtonCG.interactable = false;
            ContinueButtonCG.alpha = 0.8f;
        }

    }

    //Input
    public void OpenGameMenu()
    {
        Observer.Instance.NotifyObservers(EnumsActions.OnOpenGameMenu);

        FadeSetTime(1, 0.4f);
    }

    //buttons
    public void ButtonContinue()
    {
        LoadingScene.Instance.OpenScene(Settings.curScene, SceneNameEnum.Menu);
    }
    public void ButtonNewGame()
    {
        LoadingScene.Instance.OpenScene(SceneNameEnum.MCFlat, SceneNameEnum.Menu);
    }
    public void ButtonOptions()
    {
        UIOptions.Instance.OpenOptions();
    }
    public void ButtonQuit()
    {
        Application.Quit();
    }
}
