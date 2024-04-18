using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LightInteraction : MonoBehaviour
{
    [Header("settings")]
    [SerializeField] float PingPongIntensityDuration = 1;

    [SerializeField] Light Light;

    //local
    Tween _pingPongTween;

    void Awake()
    {
        if (Light == null) Light = GetComponent<Light>();
    }

    void Start()
    {
        this.gameObject.SetActive(false);
    }
    void OnEnable()
    {
        PingPongLightIntensity(); Light.enabled = true;
    }
    void OnDisable()
    {
        Light.DOComplete();
        _pingPongTween.Kill(); Light.enabled = false;
    }

    //tween
    void PingPongLightIntensity()
    {
        _pingPongTween = DOTween.To(() => Light.intensity, x => Light.intensity = x, 1f, PingPongIntensityDuration)
            .SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).OnComplete(PingPongLightIntensity);
    }
}
