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

        if (!SaveManager.Instance.HasData())
        {
            ContinueButtonCG.interactable = false;
            ContinueButtonCG.alpha = 0.8f;
        }

        UIOptions.Instance.SaveGameSettings();

        ToggleCursor(true);
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
        SaveManager.Instance.LoadDataFromFile();
        LoadingScene.Instance.OpenScene(Settings.curScene, SceneNameEnum.Menu);

        ToggleCursor(false);
    }
    public void ButtonNewGame()
    {
        SaveManager.Instance.LoadInitialData();
        LoadingScene.Instance.OpenScene(SceneNameEnum.MCFlat, SceneNameEnum.Menu);
        
        ToggleCursor(false);
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
