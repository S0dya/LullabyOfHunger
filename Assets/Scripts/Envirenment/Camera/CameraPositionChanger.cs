using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionChanger : MonoBehaviour
{
    [SerializeField] Transform CameraPosTransf;


    bool _isSetToThisPos;

    void OnTriggerEnter(Collider collsion)
    {
        if (!_isSetToThisPos)
        {
            _isSetToThisPos = true;

            IsometricCameraController.Instance.NewPositionForCameraFollow(CameraPosTransf);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        _isSetToThisPos = false;
    }
}
