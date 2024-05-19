using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class UIIntro : SingletonMonobehaviour<UIIntro>
{
    //local
    VideoPlayer _videoPlayer;

    void Awake()
    {
        _videoPlayer = GetComponentInChildren<VideoPlayer>();
    }

    void Start()
    {
        if (SceneManager.sceneCount < 2)
        {
            _videoPlayer.loopPointReached += OnVideoEnd; _videoPlayer.Play();

            LoadingScene.Instance.ToggleLoadingScreen(true);
        }
        else gameObject.SetActive(false);
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        LoadingScene.Instance.LoadMenu();

        gameObject.SetActive(false);
    }
}
