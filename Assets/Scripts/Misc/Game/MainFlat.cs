using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFlat : SingletonSubject<MainFlat>
{
    [SerializeField] GameObject HandGunObj;

    //local
    InteractionDoorMainFlat _interactionDoorMainFlat;


    protected override void Awake()
    {
        base.Awake(); CreateInstance();

        _interactionDoorMainFlat = GameObject.FindGameObjectWithTag("InteractionDoorMainFlat").GetComponent<InteractionDoorMainFlat>();

        HandGunObj.SetActive(false);
    }

    void Start()
    {
        Player.Instance.PlayWakeUp();

        SaveManager.Instance.SaveDataToFile();//myb remove later
    }

    //outside methods
    public void HandGunFound()
    {
        HandGunObj.SetActive(true);

        InputManager.Instance.EnableGun();

        _interactionDoorMainFlat.EnableDoor();
    }

    public void GasMaskFound()
    {
        Player.Instance.ToggleGasMask(true);

        Settings.hasGasMask = true;
    }

    public void MagFound()
    {
        MagsBag.Instance.AddMag();
    }
}
