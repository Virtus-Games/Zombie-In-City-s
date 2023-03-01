using System.IO;
using UnityEngine;

public static class PlayerDataController
{
    public static PlayerData playerData = new PlayerData();
    public const string DirectoryPath = "/SaveDataw/";
    public const string fileName = "PlayerData.sav";
    public static bool SaveData()
    {

        var dir = Application.persistentDataPath + DirectoryPath;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(playerData, prettyPrint: true);
        File.WriteAllText(dir + fileName, json);
        GUIUtility.systemCopyBuffer = dir;

        return true;
    }

    public static void LoadData()
    {
        var dir = Application.persistentDataPath + DirectoryPath + fileName;

        PlayerData data = new PlayerData();

        if (File.Exists(dir))
        {
            string json = File.ReadAllText(dir);
            data = JsonUtility.FromJson<PlayerData>(json);

            playerData = data;

        }
        else
        {
            Debug.LogError("File not found");
            SaveData();
        }



    }
}
