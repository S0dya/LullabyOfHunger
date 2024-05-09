using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

[DefaultExecutionOrder(-1)]
public class LoadingScene : SingletonMonobehaviour<LoadingScene>
{
    [SerializeField] Camera LoadingCamera;
    [SerializeField] GameObject LoadingScreen;
    [SerializeField] Image LoadingBarFill;

    protected override void Awake()
    {
        base.Awake();

        FindCurSceneID();//remove later
    }

    void Start()
    {
        if (SceneManager.sceneCount < 2) LoadMenu();

    }

    //outside methods
    public void LoadMenu()
    {
        StartCoroutine(LoadSceneCor(GetSceneIndexByName(SceneNameEnum.Menu)));
    }
    public void OpenMenu() => OpenMenu(Settings.curScene);
    public void OpenMenu(SceneNameEnum sceneToClose)
    {
        SceneManager.UnloadSceneAsync(GetSceneIndexByName(sceneToClose));
        LoadMenu();
    }

    public void OpenScene(SceneNameEnum sceneToOpen)
    {
        OpenScene(sceneToOpen, Settings.curScene);
    }
    public void OpenScene(SceneNameEnum sceneToOpen, SceneNameEnum sceneToClose)
    {
        StartCoroutine(LoadSceneCor(GetSceneIndexByName(sceneToOpen), GetSceneIndexByName(sceneToClose)));

        if (sceneToOpen != SceneNameEnum.Menu) Settings.curScene = sceneToOpen;
    }

    //main cors
    IEnumerator LoadSceneCor(int sceneToOpen, int sceneToClose)
    {
        SceneManager.UnloadSceneAsync(sceneToClose);

        yield return StartCoroutine(LoadSceneCor(sceneToOpen));
    }

    IEnumerator LoadSceneCor(int sceneToOpen)
    {
        //Observer.Instance.NotifyObservers(EnumsActions.OnChangeScene);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToOpen, LoadSceneMode.Additive);

        ToggleLoadingScreen(true);

        while (!operation.isDone)
        {
            SetFillAmount(operation.progress);

            yield return null;
        }

        ToggleLoadingScreen(false);
    }

    //other methods
    void ToggleLoadingScreen(bool toggle)
    {
        LoadingCamera.enabled = toggle;
        LoadingScreen.SetActive(toggle);
    }
    void SetFillAmount(float progress) => LoadingBarFill.fillAmount = Mathf.Clamp01(progress / 0.9f);

    void FindCurSceneID()
    {
        for (int i = 1; i < SceneManager.sceneCount; i++)
        {
            var sceneName = SceneManager.GetSceneAt(i).name;

            if (Enum.TryParse(sceneName, out SceneNameEnum sceneEnum))
            {
                Settings.curScene = sceneEnum; break;
            }
            else Debug.Log("Scene not found");
        }
    }

    int GetSceneIndexByName(SceneNameEnum sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            var sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (sceneNameInBuild == sceneName.ToString()) return i;
        }

        Debug.Log("Scene not found");
        return -1;
    }
}

public enum SceneNameEnum
{
    None,

    Menu,
    Hallway,
    MCFlat,
}