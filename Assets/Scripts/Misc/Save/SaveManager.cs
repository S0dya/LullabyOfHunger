using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System;

[DefaultExecutionOrder(-2)]
public class SaveManager : SingletonMonobehaviour<SaveManager>
{
    GameData _gameData = new GameData();
    List<ISaveable> _iSaveableObjectList = new List<ISaveable>();

    [SerializeField] TextAsset InitialData;

    //outside methods
    public void LoadInitialData() => LoadData(InitialData.text);
    public void LoadDataFromFile() => LoadDataFromFile("/data.json");
    public void LoadDataFromFile(string fileName)
    {
        string filePath = Application.persistentDataPath + fileName;

        if (File.Exists(filePath)) LoadData(File.ReadAllText(filePath));
    }
    public void LoadData(string data)
    {
        //_gameData = JsonConvert.DeserializeObject<GameData>(data);
        _gameData = JsonConvert.DeserializeObject<GameData>(EncryptionHelper.Decrypt(data));

        for (int i = _iSaveableObjectList.Count - 1; i >= 0; i--)
        {
            if (_gameData.GameDataDict.ContainsKey(_iSaveableObjectList[i].ISaveableUniqueID))
                _iSaveableObjectList[i].ISaveableLoad(_gameData);
            else Destroy(((Component)_iSaveableObjectList[i]).gameObject);
        }
    }

    public void SaveDataToFile() => SaveDataToFile("/data.json");
    public void SaveDataToFile(string fileName)
    {
        foreach (var iSaveableObject in _iSaveableObjectList) 
            AssignData(iSaveableObject.ISaveableUniqueID, iSaveableObject.ISaveableSave());

        string json = JsonConvert.SerializeObject(_gameData);
        //Debug.Log(Application.persistentDataPath + "/data.json");
        //File.WriteAllText(Application.persistentDataPath + fileName, json);
        File.WriteAllText(Application.persistentDataPath + fileName, EncryptionHelper.Encrypt(json));
    }

    public void AddISaveable(ISaveable iSaveable) => _iSaveableObjectList.Add(iSaveable);
    public void RemoveISaveable(ISaveable iSaveable) => _iSaveableObjectList.Remove(iSaveable);

    public bool HasData()
    {
        return File.Exists(Application.persistentDataPath + "/data.json");
    }

    //other methods
    public void AssignData(string id, GameObjectSave data)
    {
        if (_gameData.GameDataDict.ContainsKey(id)) _gameData.GameDataDict[id] = data;
        else _gameData.GameDataDict.Add(id, data);
    }

    /*
    public void AssignSceneData()
    {
        foreach (var iSaveableObject in _iSaveableObjectList) iSaveableObject.ISaveableAssign(Settings.curScene);
    }

    //other outside methods
    public void AddISaveable(ISaveable iSaveable) => _iSaveableObjectList.Add(iSaveable);
    public void RemoveISaveable(ISaveable iSaveable) => _iSaveableObjectList.Remove(iSaveable);

    public GameData GetData()
    {
        return _gameData;
    }
    public GameObjectData GetGameObjectData(string id)//myb remove later
    {
        return _gameData.GameDataDict.ContainsKey(id) ? _gameData.GameDataDict[id] : null;
    }

    public void RemoveItem(string id)
    {
        if (_gameData.GameDataDict.ContainsKey(id)) _gameData.GameDataDict.Remove(id);
    }
    */
}

public class EncryptionHelper
{
    private static readonly string key = "Sg9f0db5ZQxP47j4bScU/J2z4oPzuvxRff9DJsK/3NU="; // Your generated key
    private static readonly string iv = "0dJ+lBri8gRa2zZdlYc5Ew=="; // Your generated IV

    public static string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String(key);
            aes.IV = Convert.FromBase64String(iv);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String(key);
            aes.IV = Convert.FromBase64String(iv);

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}