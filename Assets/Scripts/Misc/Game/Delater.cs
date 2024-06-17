using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delater : MonoBehaviour
{
    [SerializeField] GameObject PMLap;
    [SerializeField] GameObject PMHand;

    public void TogglePMLap()
    {
        PMLap.SetActive(false);
        PMHand.SetActive(true);
    }

}
