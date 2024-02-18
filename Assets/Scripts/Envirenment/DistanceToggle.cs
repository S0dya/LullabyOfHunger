using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceToggle : MonoBehaviour
{
    [SerializeField] GameObject ObjectToToggle;

    void OnTriggerEnter(Collider collision)
    {
        ObjectToToggle.SetActive(true);
    }

    void OnTriggerExit(Collider collision)
    {
        ObjectToToggle.SetActive(false);
    }
}
