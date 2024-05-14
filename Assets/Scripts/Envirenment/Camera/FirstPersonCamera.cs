using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class FirstPersonCamera : SingletonCamera<FirstPersonCamera>, ICamera
{
    [Header("settings")]
    //public float ShakeDuration = 0.2f;
    //public Vector3 ShakeRotation = Vector3.one;

    [SerializeField] float ShakeMagnitude = 0.2f;
    [SerializeField] float ShakeRoughness = 0.1f;
    [SerializeField] float ShakeFadeIn = 0.1f;

    //local

    //threshold
    CameraShakeInstance _shakeInstance;

    //Quaternion _ogRot;

    //cors
    Coroutine _shakeCameraCor;

    void Start()
    {
        //_ogRot = Camera.transform.localRotation;
    }

    //outside methods
    public void Shoot()
    {
        /*
        if (_shakeCameraCor != null)
        {
            StopCoroutine(_shakeCameraCor); ResetTranf();
        }
        _shakeCameraCor = StartCoroutine(ShakeCameraCor());
        */

        if (_shakeInstance != null) _shakeInstance.StartFadeOut(0.01f);
        _shakeInstance = CameraShaker.Instance.ShakeOnce(ShakeMagnitude, ShakeRoughness, 0.1f, ShakeFadeIn);
    }

    //interface
    public override void ToggleCam(bool toggle)
    {
        base.ToggleCam(toggle); Player.Instance.ToggleHead(!toggle);
    }

    /*
    //cors
    IEnumerator ShakeCameraCor()
    {
        float curTime = 0;
        while (curTime < ShakeDuration)
        {
            Camera.transform.localRotation *= Quaternion.Euler(new Vector3(GetRand(ShakeRotation.x), GetRand(ShakeRotation.y), 0));

            curTime += Time.deltaTime;
            yield return null;
        }

        ResetTranf();
    }

    //other 
    float GetRand(float val)
    {
        return Random.Range(-val, val);
    }

    void ResetTranf() => Camera.transform.localRotation = _ogRot;
    */
}
