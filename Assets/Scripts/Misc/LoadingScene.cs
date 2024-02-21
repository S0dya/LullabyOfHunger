using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class LoadingScene : SingletonMonobehaviour<LoadingScene>
{
    [SerializeField] Camera LoadingCamera;
    [SerializeField] GameObject LoadingScreen;
    [SerializeField] Image LoadingBarFill;

    //outside methods
    void Start()
    {
        //LoadMenu();
        LoadingCamera.enabled = false;
    }

    public void LoadMenu()
    {
        StartCoroutine(LoadMenuCor());
    }
    public void OpenMenu(int sceneToClose)
    {
        SceneManager.UnloadSceneAsync(sceneToClose);
        LoadMenu();
    }
    public void OpenMenu() => OpenMenu(Settings.curSceneId);

    public void OpenScene(int sceneToOpen)
    {
        OpenScene(sceneToOpen, Settings.curSceneId);

        Settings.curSceneId = sceneToOpen;
    }
    public void OpenScene(int sceneToOpen, int sceneToClose)
    {
        StartCoroutine(LoadSceneCor(sceneToOpen, sceneToClose));
    }

    //main cors
    IEnumerator LoadMenuCor()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        ToggleLoadingScreen(true);

        while (!operation.isDone)
        {
            SetFillAmount(operation.progress);

            yield return null;
        }

        ToggleLoadingScreen(false);
    }

    IEnumerator LoadSceneCor(int sceneToOpen, int sceneToClose)
    {
        SceneManager.UnloadSceneAsync(sceneToClose);
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
}
