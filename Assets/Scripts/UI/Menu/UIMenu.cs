using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : UISingletonMonobehaviour<UIMenu>
{

    //Input
    public void OpenGameMenu()
    {
        Observer.Instance.NotifyObservers(EnumsActions.OnOpenGameMenu);
        FadeSetTime(1, 0.4f);

    }

    //buttons
    public void ButtonContinue()
    {
        LoadingScene.Instance.OpenScene(Settings.curScene);
    }
    public void ButtonNewGame()
    {
        LoadingScene.Instance.OpenScene(SceneNameEnum.MCFlat);
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
