using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("settings")]
    public float VisionAngle;
    public LayerMask ObstacleLayer;

    public float MinDistanceToPlayer;

    [Header("other")]
    [SerializeField] Transform HeadTransform;

    //local
    Enemy enemy;

    Transform _playerTransf;

    //threshold 
    Vector3 _directionToPlayer;

    float _angleToPlayer;
    float _distanceToPlayer;

    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    void Start()
    {
        _playerTransf = Player.Instance.transform;
    }

    void OnTriggerStay(Collider collision)
    {
        _directionToPlayer = _playerTransf.position - HeadTransform.position; _directionToPlayer.y = 0f;

        _angleToPlayer = Vector3.Angle(HeadTransform.forward, _directionToPlayer);
        _distanceToPlayer = Vector3.Distance(_playerTransf.position, transform.position);

        if ((_angleToPlayer < VisionAngle || _distanceToPlayer < MinDistanceToPlayer) && !Physics.Raycast(HeadTransform.position, _directionToPlayer, _distanceToPlayer, ObstacleLayer))
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
