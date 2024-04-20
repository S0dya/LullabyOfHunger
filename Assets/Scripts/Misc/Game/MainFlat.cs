using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainFlat : SingletonSubject<MainFlat>
{
    [SerializeField] SkinnedMeshRenderer HeadMesh;
    [SerializeField] SkinnedMeshRenderer GasMaskMesh;
    [SerializeField] GameObject HandGunObj;
    [SerializeField] Image FPGasMaskVisorImage;

    //local
    InteractionDoorMainFlat _interactionDoorMainFlat;


    protected override void Awake()
    {
        base.Awake(); CreateInstance();

        _interactionDoorMainFlat = GameObject.FindGameObjectWithTag("InteractionDoorMainFlat").GetComponent<InteractionDoorMainFlat>();

        HeadMesh.enabled = true; GasMaskMesh.enabled = FPGasMaskVisorImage.enabled = false;
        HandGunObj.SetActive(false);
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
        GasMaskMesh.enabled = FPGasMaskVisorImage.enabled = true; HeadMesh.enabled = false;
    }

    public void MagFound()
    {
        MagsBag.Instance.AddMag();
    }
}
