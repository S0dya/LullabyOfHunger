using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

[DefaultExecutionOrder(-1)]
public class GameManager : SingletonMonobehaviour<GameManager>, ISaveable
{
    [SerializeField] PostProcessVolume Volume;

    //local
    UnityEngine.Localization.Tables.StringTable _table;
    Locale _curLocale;

    //save
    public string ISaveableUniqueID { get; set; }

    protected override void Awake()
    {
        base.Awake();

        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        ChangeLocale();
    }

    void OnEnable()
    {
        ISaveableRegister();
    }
    void OnDisable()
    {
        ISaveableDeregister();
    }

    //outside methods
    public void DisableVolume() => Volume.weight = 0;

    public string GetLocalizedString(string key)
    {
        var entry = _table.GetEntry(key);

        if (entry == null) Debug.Log(key + " not found");

        return entry == null ? "key" : entry.LocalizedValue;
    }

    public void ChangeLocale()
    {
        _curLocale = LocalizationSettings.SelectedLocale;

        _table = LocalizationSettings.StringDatabase.GetTable("Table", _curLocale);
    }

    //save
    public void ISaveableRegister() => SaveManager.Instance.AddISaveable(this);
    public void ISaveableDeregister() => SaveManager.Instance.RemoveISaveable(this);

    public GameObjectSave ISaveableSave()
    {
        GameObjectSave gameObjectSave = new();

        gameObjectSave.BoolDict.Add("FirstTime", Settings.firstTime);
        gameObjectSave.BoolDict.Add("HasGasMask", Settings.hasGasMask);

        gameObjectSave.StringDict.Add("CurScene", Settings.curScene.ToString());

        Settings.curMagsN = MagsBag.Instance.GetMagsN();
        Settings.curBulletsAmount = GunController.Instance.GetBulletsInMagN();
        Settings.gunHasMag = GunController.Instance.GetGunHasMag();

        gameObjectSave.IntDict.Add("CurMagsN", Settings.curMagsN);
        gameObjectSave.IntDict.Add("CurBulletsAmount", Settings.curBulletsAmount);
        gameObjectSave.BoolDict.Add("gunHasMag", Settings.gunHasMag);
        
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

            if (gameObjectSave.StringDict.TryGetValue("CurScene", out string CurScene) && CurScene != "")
                Settings.curScene = (SceneNameEnum)Enum.Parse(typeof(SceneNameEnum), CurScene);

            gameObjectSave.IntDict.TryGetValue("CurMagsN", out Settings.curMagsN);
            gameObjectSave.IntDict.TryGetValue("CurBulletsAmount", out Settings.curBulletsAmount);
            gameObjectSave.BoolDict.TryGetValue("gunHasMag", out Settings.gunHasMag);
        }
    }
}
