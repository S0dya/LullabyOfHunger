using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("settings")]
    public float VisionRange;
    public LayerMask ObstacleLayer;

    [Header("other")]
    [SerializeField] Transform HeadTransform;

    //local
    Enemy enemy;

    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    void OnTriggerStay(Collider collision)
    {
        Vector3 directionToPlayer = collision.transform.position - HeadTransform.position;
        directionToPlayer.y = 0f;

        if (!Physics.Raycast(HeadTransform.position, directionToPlayer, VisionRange, ObstacleLayer))
            enemy.PlayerNoticed();
        else enemy.PlayerLost();

        if (!collision.CompareTag("Player")) Debug.Log("remove from enemy vision - " + collision.gameObject.layer);//delater
    }
}
