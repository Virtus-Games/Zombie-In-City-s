using PlayerNameSpace;
using UnityEngine;

public class PlayerPoint : MonoBehaviour
{
    public Transform GunPoint;
    private PlayerController playerController;

    private GunOffsetSettings gunOffsetSettings;


    private void Start()
    {
        Uptading();

    }

    public void Uptading()
    {
        playerController = GetComponent<PlayerController>();
        gunOffsetSettings = GetComponentInChildren<GunOffsetSettings>();
        gunOffsetSettings.SetWeaponAimPose(MarketManager.Instance.selectWeapon);
    }


    public void TranslateCharackter(bool status)
    {
        Uptading();

        if (status)
        {
            playerController.OnTransate(true);
            playerController.enabled = false;
            GetComponentInChildren<GunOffsetSettings>().SetWeaponIdlePose(MarketManager.Instance.selectWeapon);
            GetComponent<Health>().Close(false);
        }
        else
        {
            playerController.enabled = true;
            GetComponentInChildren<GunOffsetSettings>().SetWeaponAimPose(MarketManager.Instance.selectWeapon);
            playerController.OnTransate(false);
            GetComponent<Health>().Close(true);
        }

        

    }

}
