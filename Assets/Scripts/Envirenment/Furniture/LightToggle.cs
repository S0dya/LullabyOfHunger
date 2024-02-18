using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightToggle : MonoBehaviour
{
    [Header("settings")]
    [SerializeField] bool TurnedOn = true;
    [Range(0, 1)][SerializeField] float Chance = 0.5f;

    [Header("random time between toggles")]
    [SerializeField] float MinTime = 0.01f;
    [SerializeField] float MaxTime = 10;

    [Header("other")]
    [SerializeField] Light Light;

    //local
    float _minTimeToTurnOn;
    float _maxTimeToTurnOn;

    //bool
    bool _lights;

    //cor
    Coroutine _toggleCor;

    void Awake()
    {
        if (TurnedOn && Chance > Random.value)
        {
            _lights = true;

            _minTimeToTurnOn = MinTime / 10;
            _maxTimeToTurnOn = MaxTime / 10;
        }
    }

    void OnEnable()
    {
        if (_lights)
        {
            _toggleCor = StartCoroutine(ToggleCor());
        }
    }
    void OnDisable()
    {
        if (_lights)
        {
            if (_toggleCor != null) StopCoroutine(_toggleCor);
        }
    }


    IEnumerator ToggleCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(MinTime, MaxTime));
            Light.enabled = false;

            yield return new WaitForSeconds(Random.Range(_minTimeToTurnOn, _maxTimeToTurnOn));
            Light.enabled = true;
        }
    }
}
