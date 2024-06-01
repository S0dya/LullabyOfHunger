using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDoor : Interaction
{
    [SerializeField] DoorTypeEnum DoorType;

    [SerializeField] bool ClosesOnStart;
    [SerializeField] bool Interactable;

    [SerializeField] SceneNameEnum SceneToOpen;

    void Start()
    {
        if (ClosesOnStart) AudioManager.Instance.PlayOneShot(DoorType.ToString() + "DoorClose");
    }

    //main method
    public void Interact()
    {
        if (!Interactable)
        {
            AudioManager.Instance.PlayOneShot(DoorType.ToString() + "DoorLocked");

            return;
        }

        AudioManager.Instance.PlayOneShot(DoorType.ToString() + "DoorOpen");

        LoadingScene.Instance.OpenScene(SceneToOpen);

        SaveManager.Instance.SaveDataToFile();
    }
}

public enum DoorTypeEnum { Wooden, Lift }
