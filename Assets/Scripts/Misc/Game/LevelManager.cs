using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class LevelManager : SingletonMonobehaviour<LevelManager>
{
    //local
    Transform playerTransf;

    void Start()
    {
        playerTransf = GameObject.FindGameObjectWithTag("Player").transform;
    }


    //outside methods
    public Transform GetPlayerTransforsm()
    {
        return playerTransf;
    }
}
