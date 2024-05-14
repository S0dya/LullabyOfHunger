using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDoor : Interaction
{
    [SerializeField] bool Interactable;

    [SerializeField] SceneNameEnum SceneToOpen;

    //main method
    public void Interact()
    {
        if (!Interactable)
        {
            AudioManager.Instance.PlayOneShot("DoorLocked");

            return;
        }

        AudioManager.Instance.PlayOneShot("DoorOpen");

        LoadingScene.Instance.OpenScene(SceneToOpen);

        SaveManager.Instance.SaveDataToFile();
    }
}
