using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameMenu : UISingletonMonobehaviour<UIGameMenu>
{
    //[Header("UI")] [SerialzieField]

    void Start()
    {
        Observer.Instance.NotifyObservers(EnumsActions.OnSwitchToIsometric);
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
        if (!MainCG.blocksRaycasts) return;

        UIOptions.Instance.CloseOptions(); //myb remove
        SwitchCGSetTime(0); 
        Observer.Instance.NotifyObservers(EnumsActions.OnCloseGameMenu);
    }
    public void ButtonOptions()
    {
        UIOptions.Instance.OpenOptions();
    }
    public void ButtonQuit()
    {

        LoadingScene.Instance.OpenScene(SceneNameEnum.Menu);
    }
}
