using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionManager : SingletonMonobehaviour<InteractionManager>
{


    InteractableDoor _curDoor;
    
    protected override void Awake()
    {
        base.Awake();

    }

    public void Interact()
    {
        if (_curDoor != null) LoadingScene.Instance.OpenScene(_curDoor.SceneToOpen);

    }

    public void AddDoor(InteractableDoor door)
    {
        Debug.Log("123");
        _curDoor = door;
    }
    public void RemoveDoor()
    {
        _curDoor = null;
    }
}
