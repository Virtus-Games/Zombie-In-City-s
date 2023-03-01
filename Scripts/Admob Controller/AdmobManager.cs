using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AdmobManager : Singleton<AdmobManager>
{
    public BannerView bannerView;
    public InterstitialAd interstitial;
    public RewardedAd rewardedAd;
    private float timerForInserstitial = 15;
    public string androindBannerId = "";
    public string androindInterstitialId = "";
    public string androindRewardId = "";

    public void InitiliazedAds()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        this.RequestBanner();
        this.RequestInterstitial();
        this.RequestReward();
    }

    private void Update()
    {
        if (timerForInserstitial > 0)
            timerForInserstitial -= Time.deltaTime;

    }
    private void RequestBanner()
    {
#if UNITY_ANDROID
        // string adUnitId = "ca-app-pub-3940256099942544/6300978111"; // test
        string adUnitId = androindBannerId;
#elif UNITY_IPHONE
    string adUnitId ="";
#else
    string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();



        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    #region Reward
    public void RequestReward()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = androindRewardId;
#elif UNITY_IPHONE
       adUnitIdRewawrdIphone = "";
#else
                                    adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;

        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    private void HandleRewardedAdClosed(object sender, EventArgs e)
    {
        timerForInserstitial = 60;
        Time.timeScale = 1f;
        Camera.main.GetComponent<AudioListener>().enabled = true;
        RequestReward();
    }

    private void HandleUserEarnedReward(object sender, Reward e)
    {
        AdmonController.Instance.GiveReward();
    }
    private void HandleRewardedAdOpening(object sender, EventArgs e)
    {
        Time.timeScale = 0f;
        Camera.main.GetComponent<AudioListener>().enabled = false;
    }
    private void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        RequestReward();
    }

    #endregion


    #region Interestitial
    public bool isReadyinterstitial()
    {
        if (timerForInserstitial < 0 && interstitial.IsLoaded()){
            RequestInterstitial();
            return true;

        }
        else
            return false;
    }

    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = androindInterstitialId;
#elif UNITY_IPHONE
            string adUnitIdIntersititialIphone = "";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }
    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        RequestInterstitial();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        Time.timeScale = 0f;
        Camera.main.GetComponent<AudioListener>().enabled = false;
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        timerForInserstitial = 60;
        Time.timeScale = 1f;
        Camera.main.GetComponent<AudioListener>().enabled = true;
        RequestInterstitial();

    }
    #endregion


}
