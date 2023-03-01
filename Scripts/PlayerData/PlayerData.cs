using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public string playerName = "Player";
    public int coins = 0;
    public int highScore = 0;
    public int currentLevel = 1;
    public float speed = 1.0f;
    public float health = 100.0f;
    public MarketData marketData;
    public SettingsData settingsData;
    public Enemy enemy;

}

[System.Serializable]
public class MarketData
{
    public int usegunIndex = 10;
    public int usecharackterIndex = 0;
    public int nextgunPrice = 1000;
    public int nextcharackterPrice = 1500;

}
[System.Serializable]
public struct SettingsData
{
    public bool isMusicOn;
    public bool isVibrateOn;
}

[System.Serializable]
public struct Enemy
{
    public int level;
    public float health
    {
        get
        {
            if (level > 3)
                return 100.0f + (level * 2.0f);
            else
                return 100.0f;
        }
    }
    public float speed
    {
        get
        {
            if (level > 3)
                return 1.0f + (level * 0.15f);
            else
                return 5f;
        }
    }

    public float radius
    {
        get
        {
            if (level > 3)
                return 1.0f + (level * 0.15f);
            else
                return 5f;
        }
    }
}





