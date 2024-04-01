using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDoor : Interaction
{
    [SerializeField] SceneNameEnum SceneToOpen;

    //main method
    public void Interact()
    {
        LoadingScene.Instance.OpenScene(SceneToOpen);
    }
}
