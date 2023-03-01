using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private Image CurrentImage;
    [SerializeField] private Image CloseImage;
    MarketDataAbstrack abs;
    public int id;

    public void SetData(MarketDataAbstrack abs)
    {
        this.abs = abs;
        CurrentImage.sprite = abs.itemImage;
        this.id = abs.id;
    }

    public MarketDataAbstrack getData(){
        return abs;
    }
    public void CloseImaged(){
        CloseImage.gameObject.SetActive(false);
    }
}
