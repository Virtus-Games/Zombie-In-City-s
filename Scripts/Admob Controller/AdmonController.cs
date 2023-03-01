using System;
using UnityEngine;

public enum AdmobStatus
{
    AdWatching,
    SkinLevel,
    WinX2,
    Bazuka,
    Grading,
    Charackter,
    Gun,

}


public class AdmonController : Singleton<AdmonController>
{

    public AdmobStatus admobStatus;
    public event Action<AdmobStatus> OnGameStateChanged;

    public float timerWaiting = 60;

    private float _timer;
    public bool isVideoLoaded;

    public void UptadeAdmobStatus(AdmobStatus status)
    {
        timerWaiting = _timer;
        isVideoLoaded = false;
        admobStatus = status;
        OnGameStateChanged?.Invoke(status);
    }


    // Defeat Panel > Show
    public void DefeatIntersitial()
    {
        AdmobManager.Instance.isReadyinterstitial();
    }

    private void Start() => _timer = timerWaiting;
    void Update()
    {
        timerWaiting = Mathf.Max(0, timerWaiting - Time.deltaTime);
        
        if (timerWaiting <= 0)
            isVideoLoaded = true;
        else
            isVideoLoaded = false;
    }


    public void SetRequestAtGun(AdmobStatus status)
    {
        if (isVideoLoaded)
        {
            admobStatus = status;
            OpenUpgradeGunBazukaOrGradele(false);
        }
        else
        {
            // TODO: Animatation
        }
    }

    public void SetRequest()
    {
        // if (isVideoLoaded)
        // {
        //     OpenUpgradeGunBazukaOrGradele(true);
        // }
        // else
        //     OpenUpgradeGunBazukaOrGradele(false);
    }

    public void SetStatus(AdmobStatus status)
    {
        admobStatus = status;

        if (isVideoLoaded && AdmobManager.Instance.rewardedAd.IsLoaded())
            AdmobManager.Instance.rewardedAd.Show();

    }

    // Play Panel ----> Guns
    public void OpenUpgradeGunBazukaOrGradele(bool isStatus)
    {
        UIManager.Instance.OpensGun(isStatus);
    }

    public void GiveReward()
    {
        switch (admobStatus)
        {
            case AdmobStatus.AdWatching:
                // Market Manager
                break;
            case AdmobStatus.SkinLevel:
                LevelManager.Instance.NextLevel();
                // Next Level
                break;
            case AdmobStatus.WinX2:
                UIManager.Instance.UpdateCoin();
                // Win X2
                break;
            case AdmobStatus.Bazuka:
                // Bazuka
                break;
            case AdmobStatus.Grading:
                // Grading
                break;
            case AdmobStatus.Charackter:
                MarketManager.Instance.BuyCharackter(true);
                // Charackter
                break;
            case AdmobStatus.Gun:
                MarketManager.Instance.BuyGun(true);
                // Gun
                break;

        }

        admobStatus = AdmobStatus.WinX2;
        UptadeAdmobStatus(admobStatus);
    }

    internal void ShowReward()
    {
        if (isVideoLoaded && AdmobManager.Instance.rewardedAd.IsLoaded())
            AdmobManager.Instance.rewardedAd.Show();
    }
}
