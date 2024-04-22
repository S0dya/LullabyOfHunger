public interface ISaveable
{
    string ISaveableUniqueID { get; set; }
    //GameObjectSave GameObjectSave { get; set; }

    void ISaveableRegister();
    void ISaveableDeregister();

    GameObjectSave ISaveableSave();
    void ISaveableLoad(GameData gameData);
}
