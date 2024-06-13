using FMODUnity;
using PSXShaderKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : SingletonSubject<Shop>
{
    [Header("Settings")]
    [SerializeField] float VictoryDelayDuration = 5;

    [Header("Other")]
    [SerializeField] PSXShaderManager PsxShaderManager;

    [SerializeField] GameObject RealGO;
    [SerializeField] GameObject FalseGO;

    [SerializeField] StudioEventEmitter AmbientEmitter;

    //outside methods
    public void KidFoodFound()
    {
        FalseGO.SetActive(false); 

        GameManager.Instance.DisableVolume(); 
        PsxShaderManager._VertexGridResolution = 2000;
        AmbientEmitter.enabled = false;

        RealGO.SetActive(true);

        StartCoroutine(DelayVictory());
    }


    //other methods
    IEnumerator DelayVictory()
    {
        yield return new WaitForSeconds(VictoryDelayDuration);

        UIGameOver.Instance.VictoryOpenTab();
    }


}
