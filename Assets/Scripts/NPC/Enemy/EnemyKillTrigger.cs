using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillTrigger : MonoBehaviour
{

    //local
    Enemy _enemy;

    void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    //triggers
    void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.CompareTag("Player")) Debug.Log("Remove " + collision.gameObject.tag);

        if (Player.Instance.IsDead()) return;
        _enemy.Kill();
    }
}
