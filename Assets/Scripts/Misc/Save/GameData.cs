using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //id, data
    [SerializeField] public Dictionary<string, GameObjectSave> GameDataDict = new Dictionary<string, GameObjectSave>();
}

[System.Serializable]
public class GameObjectSave
{
    [SerializeField] public List<float> FloatList = new List<float>(); 

    [SerializeField] public Dictionary<string, int> IntDict = new Dictionary<string, int>();
    [SerializeField] public Dictionary<string, float> FloatDict = new Dictionary<string, float>();
    [SerializeField] public Dictionary<string, bool> BoolDict = new Dictionary<string, bool>();
    [SerializeField] public Dictionary<string, string> StringDict = new Dictionary<string, string>();

    [SerializeField] public Dictionary<string, Vector3Serializable> Vector3Dict = new Dictionary<string, Vector3Serializable>();
}

[System.Serializable]
public class Vector3Serializable
{
    [SerializeField] public float x, y, z;

    public Vector3Serializable(Vector3 vector3)
    {
        x = vector3.x; y = vector3.y; z = vector3.z;
    }

    public Vector3 GetVector3()
    {
        return new Vector3(x, y, z);
    }
}
