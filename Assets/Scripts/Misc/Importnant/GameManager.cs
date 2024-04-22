using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : SingletonMonobehaviour<GameManager>, ISaveable
{
    //save
    public string ISaveableUniqueID { get; set; }

    protected override void Awake()
    {
        base.Awake();

        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        
        //Settings.firstTime = false;
    }

    void Start()
    {

        SaveManager.Instance.SaveDataToFile();
        SaveManager.Instance.LoadDataFromFile();
    }

    void OnEnable()
    {
        ISaveableRegister();
    }
    void OnDisable()
    {
        ISaveableDeregister();
    }

    //save
    public void ISaveableRegister() => SaveManager.Instance.AddISaveable(this);
    public void ISaveableDeregister() => SaveManager.Instance.RemoveISaveable(this);

    public GameObjectSave ISaveableSave()
    {
        GameObjectSave gameObjectSave = new();

        gameObjectSave.BoolDict.Add("FirstTime", Settings.firstTime);
        gameObjectSave.BoolDict.Add("HasGasMask", Settings.hasGasMask);

        gameObjectSave.IntDict.Add("CurMagsN", Settings.curMagsN);

        return gameObjectSave;
    }
    public void ISaveableLoad(GameData gameData)
    {
        if (gameData.GameDataDict.TryGetValue(ISaveableUniqueID, out GameObjectSave gameObjectSave))
        {
            gameObjectSave.BoolDict.TryGetValue("FirstTime", out Settings.firstTime);

            if (Settings.firstTime)
            {
                Settings.firstTime = false;
                
                UIOptions.Instance.SaveGameSettings();
            }
            
            gameObjectSave.BoolDict.TryGetValue("HasGasMask", out Settings.hasGasMask);

            gameObjectSave.IntDict.TryGetValue("CurMagsN", out Settings.curMagsN);

        }
    }
}
