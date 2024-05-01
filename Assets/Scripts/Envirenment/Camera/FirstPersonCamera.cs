using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FirstPersonCamera : SingletonCamera<FirstPersonCamera>, ICamera
{
    [Header("settings")]
    public float ShakeDuration = 0.2f;
    public Vector3 ShakeRotation = Vector3.one;

    //outside methods
    public void Shoot()
    {
        Camera.DOComplete();
        Camera.DOShakeRotation(ShakeDuration, ShakeRotation);
    }
    //interface
    public override void ToggleCam(bool toggle)
    {
        base.ToggleCam(toggle); Player.Instance.ToggleHead(!toggle);
    }
}
