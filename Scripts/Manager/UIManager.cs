using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class UIManager : Singleton<UIManager>
{
    private const string Music = "Music";
    private const string Vibrate = "Vibrate";
    public GameObject StartPanel, MarketPanel, PlayPanel;
    public GameObject SettingsPanel, VictoryPanel, DefeatPanel, MoneyBox, MoneyInage;

    public GameObject Guns;

    [Space(5)]
    [Header("Coins Points")]
    public GameObject CoinsPointInVictoryPanel;
    public GameObject CoinsForPointsPrefab;

    [Space(5)]
    [Header("Coins Panel")]
    public GameObject CoinsPanel;
    public TextMeshProUGUI CoinText;
    private int _money;
    private int _totalMoney;

    [HideInInspector]
    public PlayerData playerData;

    [Space(5)]
    [Header("Coins Panel")]
    public TextMeshProUGUI TotalMoneyText;

    [Tooltip("TotalX2moneyText in Ad Button Win Panel")]
    public TextMeshProUGUI TotalX2moneyText;
    public Image ShildImage;
    public GameObject Shild;

    public TextMeshProUGUI levelName;

    public GameObject MoneyPrefab;

    public Transform MoneyParent;

    private void Start()
    {
        UpdatePlayerData();
        UpdateGameState(GAMESTATE.START);
        UpdateCoin();
    }


    public void ShildController(bool isActive, float timer)
    {
        Shild.SetActive(isActive);
        ShildImage.fillAmount = timer / 10;
    }

    public void RequestReward()
    {

    }
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += UpdateGameState;
        PlayerDataManager.OnPlayerDataChanged += UpdatePlayerData;
        LevelManager.OnLevelLoaded += UpdateLevel;
    }


    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= UpdateGameState;
        PlayerDataManager.OnPlayerDataChanged -= UpdatePlayerData;
        LevelManager.OnLevelLoaded -= UpdateLevel;
    }

    private void UpdateLevel(bool arg0)
    {
        if (arg0)
        {
            EnemySoundController();

        }
    }

    private void UpdatePlayerData(PlayerData playerData)
    {
        this.playerData = playerData;
        CoinText.text = (this.playerData.coins).ToString();
    }

    public void UpdatePlayerData()
    {
        PlayerDataManager.Instance.UpdatePlayerData(playerData);
    }

    private void UpdateGameState(GAMESTATE switchState)
    {
        switch (switchState)
        {
            case GAMESTATE.START:
                _money = 0;
                _totalMoney = 0;
                UpdateUI(StartPanel);
                ShildImage.fillAmount = 1;


                break;
            case GAMESTATE.PLAY:
                levelName.SetText("Level " + LevelManager.Instance.currentLevel.ToString());
                UpdateUI(PlayPanel);
                BannerController(false);
                break;
            case GAMESTATE.VICTORY:
                UpdateUI(VictoryPanel);
                _totalMoney = _money;
                CoinsGoToMoneyBox();
                BannerController(true);
                break;
            case GAMESTATE.MARKET:
                UpdateUI(MarketPanel);
                BannerController(false);
                break;
            case GAMESTATE.DEFEAT:
                AdmonController.Instance.DefeatIntersitial();
                UpdateUI(DefeatPanel);
                BannerController(true);
                break;
            default:
                UpdateUI(null);
                break;

        }


    }

    public void UpdateUI(GameObject obj)
    {
        StartPanel.SetActive(false);
        MarketPanel.SetActive(false);
        PlayPanel.SetActive(false);
        VictoryPanel.SetActive(false);
        DefeatPanel.SetActive(false);
        SettingsPanel.SetActive(false);

        if (obj != null)
            obj.SetActive(true);

    }

    #region Coins
    public void UpdateCoin()
    {
        int total = GetCoin();
        _totalMoney += total;
        PlayerPrefs.SetInt("coins", _totalMoney);
        CoinText.text = _totalMoney.ToString();
    }

    public int GetCoin() => PlayerPrefs.GetInt("coins");
    public void SetMoneyCalculate(int getMoney) => _money += getMoney;

    #endregion
    public void OpensGun(bool isStatus) => Guns.SetActive(isStatus);
    public void CoinsGoToMoneyBox()
    {
        TotalMoneyText.SetText(_totalMoney.ToString());

        TotalX2moneyText.SetText("Clamp X2 " + _totalMoney * 2);

        CoinText.SetText((playerData.coins).ToString());

        UpdateCoin();

        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(CoinsForPointsPrefab, CoinsPointInVictoryPanel.transform);

            float xPosition = Random.Range(-40, 10f);
            float yPosition = Random.Range(-10, 10f);

            obj.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);

            obj.transform.DOLocalMove(new Vector2(yPosition, xPosition), 0.1f);


            float minDuration = Random.Range(1f, 2f);

            obj.transform.
                DOMove(MoneyBox.transform.position, minDuration).SetEase(Ease.InCubic).
                OnComplete(() =>
                {
                    VibrateController(7);
                    Destroy(obj);
                });
        }

        AdmonController.Instance.DefeatIntersitial();
    }

    #region Sound Vibrate
    public void SoundController(AudioClip clip)
    {
        if (PlayerPrefs.GetInt(Vibrate) == 1)
            VibrateController(15);
        if (PlayerPrefs.GetInt(Music) == 1)
            FindObjectOfType<CameraManager>().GunPlaySource(clip);

    }

    private void VibrateController(int val) => Vibrator.Vibrate(val);

    public void EnemySoundController()
    {

        if (PlayerPrefs.GetInt(Music) == 1)
        {
            EnemyNameSpace.EnemyController[] enemies = FindObjectsOfType<EnemyNameSpace.EnemyController>();
            foreach (EnemyNameSpace.EnemyController enemy in enemies)
                enemy.VoiceController(true);
        }
        else
        {
            EnemyNameSpace.EnemyController[] enemies = FindObjectsOfType<EnemyNameSpace.EnemyController>();
            foreach (EnemyNameSpace.EnemyController enemy in enemies)
                enemy.VoiceController(false);
        }
    }

    public bool SettingsDataController(SettingsIcon icon)
    {
        if (icon.dataType == dataType.MUSIC)
            return PlayerPrefs.GetInt(Music) == 1 ? true : false;
        else
            return PlayerPrefs.GetInt(Vibrate) == 1 ? true : false;
    }
    #endregion

    public void BannerController(bool isShow)
    {

        if (isShow)
            AdmobManager.Instance.bannerView.Show();
        else
            AdmobManager.Instance.bannerView.Hide();


    }

    public void TransformMoney(Vector2 pos)
    {
        StartCoroutine(InstantceAndTranslate(pos));
    }

    IEnumerator InstantceAndTranslate(Vector3 pos)
    {
        GameObject objx = Instantiate(MoneyPrefab, pos, Quaternion.identity, MoneyParent);

        while (Vector3.Distance(objx.transform.position, MoneyInage.transform.position) > 2f)
        {
            objx.transform.position = Vector3.Lerp(objx.transform.position, MoneyInage.transform.position, 2f * Time.deltaTime);
            yield return null;
        }

        int randomCoins = Random.Range(10, 30);

        SetCoin(randomCoins);


        Destroy(objx);
    }

    private void SetCoin(int v)
    {
        _totalMoney += v;
        CoinText.SetText((PlayerPrefs.GetInt("coins") + v).ToString());
    }
}