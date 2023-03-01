using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemButtonManager : MonoBehaviour
{

    // MarketPanel -> Main -> 
    // Background Panel -> Image Background -> 
    // Grid Panel - > Items  Compoenent Button
    public void ItemClicked()
    {
        Item item = GetComponentInParent<Item>();
        MarketManager.Instance.SelectData(item.getData());
    }

    public void GunBuyButton()
    {
        AdmonController.Instance.SetStatus(AdmobStatus.Gun);
    }


    // Market Manager > Bottom Button
    public void GunBuy()
    {
        MarketManager.Instance.BuyGunItem();
    }


    // Market Manager > Bottom Button
    public void CharackterBuy()
    {
        MarketManager.Instance.BuyCharackterItem();
    }
    public void BuyCharackterItem()
    {
        AdmonController.Instance.SetStatus(AdmobStatus.Charackter);
    }


    public void OpenBazuka()
    {
        AdmonController.Instance.SetRequestAtGun(AdmobStatus.Bazuka);
    }

    public void OpenGrading()
    {
        AdmonController.Instance.SetRequestAtGun(AdmobStatus.Grading);
    }

}
