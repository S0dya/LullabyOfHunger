using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonobehaviour<LevelManager>
{

    //local inheriting
    [HideInInspector] public Transform playerTransf;

    void Start()
    {
        playerTransf = GameObject.FindGameObjectWithTag("Player").transform;
    }

}
