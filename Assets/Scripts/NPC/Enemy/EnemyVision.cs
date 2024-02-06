using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("settings")]
    public float VisionAngle;
    public LayerMask ObstacleLayer;

    [Header("other")]
    [SerializeField] Transform HeadTransform;

    //local
    Enemy enemy;

    //threshold 
    Vector3 _directionToPlayer;

    float _angleToPlayer;

    void Awake()
    {
        VisionAngle *= 0.5f;

        enemy = GetComponentInParent<Enemy>();
    }

    void OnTriggerStay(Collider collision)
    {
        _directionToPlayer = collision.transform.position - HeadTransform.position;
        _directionToPlayer.y = 0f;

        _angleToPlayer = Vector3.Angle(HeadTransform.forward, _directionToPlayer);

        if (_angleToPlayer <= VisionAngle && !Physics.Raycast(HeadTransform.position, _directionToPlayer, ObstacleLayer))
            enemy.PlayerNoticed();
        else
        {
            enemy.PlayerLost();
        }


        if (!collision.CompareTag("Player")) Debug.Log("remove from enemy vision - " + collision.gameObject.layer);//delater
    }

    void OnTriggerExit(Collider collision)
    {
        enemy.PlayerLost();
    }
}
