using UnityEngine;


[CreateAssetMenu(fileName = "MarketDataSO",
            menuName = "SO/MarketDataSO", order = 1)]
public class MarketDataSO : ScriptableObject
{
    [Header("Gun")]
    [Space(10)]
    public Gun[] gun;
    public int gunLength { get { return gun.Length; } }


    [Header("Charackter")]
    [Space(10)]
    public Charackter[] charackter;

    public int charackterLength { get { return charackter.Length; } }
}

[System.Serializable]
public class Charackter : MarketDataAbstrack
{

}


[System.Serializable]

public class Gun : MarketDataAbstrack
{
    public SelectWeapon selectWeapon;
}


public abstract class MarketDataAbstrack
{
    public string itemName;
    public int id;
    public ItemType itemType;
    public int itemPrice;
    public Sprite itemImage;
    public GameObject itemPrefab;
   
}

[System.Serializable]
public enum ItemType
{
    Gun,
    Charackter
}