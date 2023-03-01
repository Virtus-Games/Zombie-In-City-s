using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayerNameSpace;
using System;

public class UpgaredeItem : MonoBehaviour
{
    public Image icon;
    public Image background;
    public TextMeshProUGUI text;
    public UpgradeSO upgrade;
    public void OnPointerEnter()
    {
        UpgradeManager.Instance.CloseThis(upgrade);
        background.enabled = true;
        // GameObject.FindWithTag("Player").GetComponent<PlayerController>().Upgrade(upgrade);
    }

    void Start()
    {
        icon.sprite = upgrade.icon;
        text.SetText(upgrade.upgradeName);
    }
}
