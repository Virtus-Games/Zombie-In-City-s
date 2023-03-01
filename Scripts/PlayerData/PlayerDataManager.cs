using System;
using UnityEngine;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    public static event Action<PlayerData> OnPlayerDataChanged;
    private const string _PlayerDataName = "PlayerData";
    public PlayerData playerData;
    public void UpdatePlayerData(PlayerData playerData)
    {
        if (playerData.coins <= 0)
            playerData.coins = 0;

        this.playerData = playerData;
        SaveGame();
        OnPlayerDataChanged?.Invoke(this.playerData);
    }

    private void OnEnable()
    {
        LoadGame();
    }

    public void SaveGame()
    {
        PlayerDataController.playerData = playerData;
        PlayerDataController.SaveData();
    }

    public void LoadGame()
    {
       PlayerDataController.LoadData();
        UpdatePlayerData(PlayerDataController.playerData);


    }

    public void SaveCoin(int val)
    {
        PlayerDataController.playerData.coins += val;
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public int GetCoins()
    {
        return playerData.coins;
    }
}
