public interface ISaveable
{
    string ISaveableUniqueID { get; set; }
    //GameObjectSave GameObjectSave { get; set; }
    SceneNameEnum ISaveableScene { get; set; }

    void ISaveableRegister();
    void ISaveableDeregister();

    void ISaveableAssign(SceneNameEnum sceneName);

    GameObjectData ISaveableSave();
    void ISaveableLoad(GameData gameData);
}
