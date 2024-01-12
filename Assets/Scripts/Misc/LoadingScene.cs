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
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Image loadingBarFill;

    protected override void Awake()
    {
        base.Awake();

        //StartCoroutine(LoadMenu());
    }

    //outside methods
    public void OpenMenu(int sceneToClose)
    {
        SceneManager.UnloadSceneAsync(sceneToClose);
        StartCoroutine(LoadMenu());
    }
    public void OpenMenu() => OpenMenu(Settings.curSceneId);

    public void OpenScene(int sceneToOpen)
    {
        StartCoroutine(LoadGame(sceneToOpen));
    }

    //main cors
    IEnumerator LoadMenu()
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

    IEnumerator LoadGame(int sceneToOpen)
    {
        SceneManager.UnloadSceneAsync(1);
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
    void ToggleLoadingScreen(bool toggle) => loadingScreen.SetActive(toggle);
    
    void SetFillAmount(float progress) => loadingBarFill.fillAmount = Mathf.Clamp01(progress / 0.9f);
}
