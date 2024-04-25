using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceToggle : MonoBehaviour
{
    [SerializeField] GameObject ObjectToToggle;

    void Start()
    {
        ToggleObj(false);

    }

    void OnTriggerEnter(Collider collision)
    {
        ToggleObj(true);
    }

    void OnTriggerExit(Collider collision)
    {
        ToggleObj(false);
    }

    void ToggleObj(bool toggle) => ObjectToToggle.SetActive(toggle);
}
