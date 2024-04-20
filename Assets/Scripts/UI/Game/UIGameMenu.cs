using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameMenu : UISingletonMonobehaviour<UIGameMenu>
{
    //[Header("UI")] [SerialzieField]


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

        SwitchCGSetTime(0); 
        Observer.Instance.NotifyObservers(EnumsActions.OnCloseGameMenu);
    }
    public void ButtonOptions()
    {

    }
    public void ButtonQuit()
    {

    }


}
