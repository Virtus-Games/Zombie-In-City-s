
using UnityEngine;

[System.Serializable]
public enum UpgradeType
{
    Kalkan,
    Hizli_atis,
    Explosion,
    İkiliAtis
}

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade/Upgrade")]
public class UpgradeSO : ScriptableObject
{
    public string upgradeName;
    public Sprite icon;
    public UpgradeType upgradedItem;
    public float floatTpye;
    public bool boolenType = false;

}

