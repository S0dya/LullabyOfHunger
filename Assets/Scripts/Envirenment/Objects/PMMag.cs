using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMMag : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.PlayOneShot("MagFall", transform.position);

        Invoke("TurnOffRb", 1f);
    }

    void TurnOffRb()
    {
        var rb = GetComponent<Rigidbody>();
        var collider = GetComponent<BoxCollider>();

        if (rb != null) rb.isKinematic = true;
        if (collider != null) collider.enabled = false;
    }
}
