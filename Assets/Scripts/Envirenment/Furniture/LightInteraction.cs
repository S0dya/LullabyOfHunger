using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LightInteraction : MonoBehaviour
{
    [Header("settings")]
    [SerializeField] float PingPongIntensityDuration;

    [SerializeField] Light Light;

    //local
    Tween _pingPongTween;

    void Awake()
    {
        if (Light == null) Light = GetComponent<Light>();
    }

    void Start()
    {
        PingPongLightIntensity();
    }

    void OnEnable()
    {
        PingPongLightIntensity(); Light.enabled = true;
    }
    void OnDisable()
    {
        _pingPongTween.Kill(); Light.enabled = false;
    }

    //tween
    void PingPongLightIntensity()
    {
        _pingPongTween = DOTween.To(() => Light.intensity, x => Light.intensity = x, 1f, PingPongIntensityDuration)
            .SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).OnComplete(PingPongLightIntensity);
    }
}
