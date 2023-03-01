using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [SerializeField] private GameObject Parent;

    [SerializeField] private Button SelectUpgradeButton;
    private UpgradeSO _so;

    public void Show()
    {
        SelectUpgradeButton.interactable = false;
        Parent.SetActive(true);
    }

    public void CloseThis(UpgradeSO upgrade)
    {
        _so = upgrade;
        SelectUpgradeButton.interactable = true;
    }

    // Select Upgrade Button Events
    public void interactableButton()
    {
        GameObject g = GameObject.FindWithTag("Player");
        g.GetComponent<PlayerNameSpace.PlayerController>().Upgrade(_so);
        Parent.SetActive(false);
    }

}
