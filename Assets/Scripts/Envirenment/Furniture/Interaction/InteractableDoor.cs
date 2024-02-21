using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : MonoBehaviour
{
    [Header("settings")]
    public int SceneToOpen = 2;

    void OnTriggerEnter(Collider collision)
    {
        InteractionManager.Instance.AddDoor(this);
    }

    void OnTriggerExit(Collider collision)
    {
        InteractionManager.Instance.RemoveDoor();
    }

}
