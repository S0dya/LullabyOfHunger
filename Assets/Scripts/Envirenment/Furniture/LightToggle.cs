using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightToggle : MonoBehaviour
{
    [SerializeField] Light Light;
    [Range(0, 1)] [SerializeField] float Chance;

    [Header("Random time between toggles")]
    [SerializeField] float MinTime = 0.01f;
    [SerializeField] float MaxTime = 10;

    //local
    float _minTimeToTurnOn;
    float _maxTimeToTurnOn;

    Coroutine _toggleCor;

    void Start()
    {
        if (Chance > Random.value)
        {
            _toggleCor = StartCoroutine(ToggleCor());

            _minTimeToTurnOn = MinTime / 10;
            _maxTimeToTurnOn = MaxTime / 10;
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
