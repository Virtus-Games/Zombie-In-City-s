using PlayerNameSpace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketManager : Singleton<MarketManager>
{
    private const string useCharacterIndex = "usecharackterIndex";
    private const string useGunIndexKey = "UsegunIndex";
    private const string NextCharackterPrice = "NextGunPrice";
    private const string NextGunPrice = "NextGunPrice";
    private const string useCharackterIndexKey = "usecharackterIndex";
    private const string useSelectGunIndexKey = "UseSelectGunIndex";
    [SerializeField] private GameObject[] CharacktersItems;
    [SerializeField] private GameObject[] GunItems;
    [SerializeField] private GameObject Background;
    [SerializeField] private Sprite CharackterBackground;
    [SerializeField] private Sprite GunBackground;
    [SerializeField] private MarketDataSO marketDataSO;
    [SerializeField] private TextMeshProUGUI BuyGunText;
    [SerializeField] private TextMeshProUGUI BuyCharackterText;
    private GameObject _lastGun;
    public GameObject Player;
    public GameObject GunPanel;
    public GameObject CharackterPanel;


    public ParticleSystem OnTranslateEffect;

    public Transform ViewPoint;

    public SelectWeapon selectWeapon;



    private void OnEnable()
    {
        LevelManager.OnLevelLoaded += OnLevelLoaded;
        GameManager.OnGameStateChanged += UpdateGameState;
    }

    private void UpdateGameState(GAMESTATE obj)
    {

        if (obj == GAMESTATE.MARKET)
        {

            //CameraControllerWithCinemachine.Instance.CharackterStatus(false);
            CameraManager.Instance.CharackterStatus(false);
            _isTranslate = true;
            TranslateCharackter();
        }

        else if (obj == GAMESTATE.PLAY)
        {
            SelectWeapont();
            _isTranslate = false;
            TranslateCharackter();
            CameraManager.Instance.CharackterStatus(true);

        }
    }

    int usecharackterIndex;
    int usegunIndex;

    public void MarketPrefController()
    {
        usecharackterIndex = PlayerPrefs.GetInt("usecharackter");
        usegunIndex = PlayerPrefs.GetInt("usegun");

    }

    private void OnLevelLoaded(bool arg0)
    {
        if (arg0)
        {
            AwakeData(CharacktersItems, marketDataSO.charackter);
            AwakeData(GunItems, marketDataSO.gun);


        }
    }

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(marketDataSO.charackter[0].itemName + marketDataSO.charackter[0].id))
        {
            PlayerPrefs.SetInt(marketDataSO.charackter[0].itemName + marketDataSO.charackter[0].id, usecharackterIndex);
            PlayerPrefs.SetInt(marketDataSO.gun[0].itemName + marketDataSO.gun[0].id, usegunIndex);
            PlayerPrefs.SetInt(useSelectGunIndexKey, marketDataSO.gun[0].id);
        }

        if (!PlayerPrefs.HasKey(NextCharackterPrice))
        {
            PlayerPrefs.SetInt(NextCharackterPrice, marketDataSO.charackter[0].itemPrice);
            PlayerPrefs.SetInt(NextGunPrice, marketDataSO.gun[0].itemPrice);
        }

        BuyTextController(PlayerPrefs.GetInt(NextGunPrice), PlayerPrefs.GetInt(NextCharackterPrice));
    }


    private void OnDisable()
    {
        LevelManager.OnLevelLoaded -= OnLevelLoaded;
        GameManager.OnGameStateChanged -= UpdateGameState;
    }

    private void Start()
    {
        OpenPanel(1);
    }


    void BuyTextController(int gun, int charackter)
    {
        BuyGunText.text = gun.ToString() + "$";
        BuyCharackterText.text = charackter.ToString() + "$";
    }


    #region Awake Set Data


    private void AwakeData(GameObject[] items, MarketDataAbstrack[] mar)
    {

        try
        {
            for (int i = 0; i < mar.Length; i++)
                items[i].GetComponent<Item>().SetData(mar[i]);

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetComponent<Item>().getData().itemType == ItemType.Charackter && usecharackterIndex == mar[i].id)
                    CharackterChanged(usecharackterIndex);
                // else if (mar[i].itemType == ItemType.Gun && _playerData.marketData.usegunIndex == mar[i].id)
                //     GunChanged(_playerData.marketData.usegunIndex);
                if (mar[i].id == PlayerPrefs.GetInt(mar[i].itemName + mar[i].id))
                    items[i].GetComponent<Item>().CloseImaged();
            }


        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }

    }
    #endregion

    #region Select Item
    public void SelectData(MarketDataAbstrack abs)
    {
        switch (abs.itemType)
        {
            case ItemType.Charackter:
                if (!marketDataSO.charackter[abs.id].itemPrefab.name.StartsWith(Player.name))
                {
                    CharackterChanged(abs.id);
                    PlayerPrefs.SetInt(useCharacterIndex, abs.id);
                }
                break;
            case ItemType.Gun:
                GunChanged(abs.id);
                PlayerPrefs.SetInt(useGunIndexKey, abs.id);
                break;
        }
    }

    #endregion

    #region  BuyItem

    public void BuyGunItem()
    {
        BuyGun(false);
    }

    public void BuyGun(bool isFree)
    {
        BuyController(
        marketDataSO.gunLength,
        isFree,
        PlayerPrefs.GetInt(NextGunPrice),
        marketDataSO.gun, GunItems
        );
    }


    public void BuyCharackterItem()
    {
        BuyCharackter(false);
    }

    public void BuyCharackter(bool isFree)
    {
        BuyController(
         CharacktersItems.Length,
         isFree,
        PlayerPrefs.GetInt(NextCharackterPrice),
        marketDataSO.charackter, CharacktersItems
        );
    }


    public void BuyController(int Length, bool isFree, int playerPrice, MarketDataAbstrack[] abs, GameObject[] items)
    {
        int coins = PlayerPrefs.GetInt("coins");

        for (int i = 0; i < Length; i++)
        {

            if (((coins >= abs[i].itemPrice && coins > 0) || isFree) && abs[i].id != PlayerPrefs.GetInt(abs[i].itemName + abs[i].id))
            {
                if (!isFree)
                {
                    coins -= abs[i].itemPrice;
                    PlayerPrefs.SetInt("coins", coins);
                }

                if (abs[i].itemType == ItemType.Charackter)
                {
                    if (i != Length - 1)
                    {
                        PlayerPrefs.SetInt(NextCharackterPrice, abs[i + 1].itemPrice);
                        PlayerPrefs.SetInt(useCharacterIndex, abs[i + 1].id);
                    }
                    else
                    {
                        PlayerPrefs.SetInt(NextCharackterPrice, abs[i].itemPrice);
                        PlayerPrefs.SetInt(useCharacterIndex, abs[i].id);
                    }

                    CharackterChanged(abs[i].id);
                }
                else
                {
                    if (i != Length - 1)
                    {
                        PlayerPrefs.SetInt(NextGunPrice, abs[i + 1].itemPrice);
                        PlayerPrefs.SetInt(useGunIndexKey, abs[i + 1].id);
                    }
                    else
                    {
                        PlayerPrefs.SetInt(NextGunPrice, abs[i].itemPrice);
                        PlayerPrefs.SetInt(useGunIndexKey, abs[i].id);
                    }

                    PlayerPrefs.SetInt(useSelectGunIndexKey, abs[i].id);
                    GunChanged(PlayerPrefs.GetInt(useGunIndexKey));
                }

                BuyTextController(PlayerPrefs.GetInt(NextGunPrice), PlayerPrefs.GetInt(NextCharackterPrice));
                PlayerPrefs.SetInt(abs[i].itemName + abs[i].id, abs[i].id);
                items[i].GetComponent<Item>().CloseImaged();

                TranslateCharackter();

                break;
            }
        }
    }


    public void OpenPanel(int val)
    {
        if (val == 0)
        {
            Background.GetComponent<Image>().sprite = GunBackground;
            CharackterPanel.SetActive(false);
            GunPanel.SetActive(true);
        }
        else
        {
            Background.GetComponent<Image>().sprite = CharackterBackground;
            CharackterPanel.SetActive(true);
            GunPanel.SetActive(false);
        }
    }





    #endregion

    #region InstantPrefab
    public void GunChanged(int id)
    {

        if (_lastGun != null)
            _lastGun.SetActive(false);

        for (int i = 0; i < marketDataSO.gunLength; i++)
        {
            if (marketDataSO.gun[i].id == id)
            {
                PlayerPrefs.SetInt(useSelectGunIndexKey, id);
                selectWeapon = marketDataSO.gun[i].selectWeapon;
                Player.GetComponentInChildren<GunOffsetSettings>().SetWeaponIdlePose(selectWeapon);
                //_lastGun = _Player.GetComponentInChildren<GunOffsetSettings>().SetWeapon(marketDataSO.gun[i].selectWeapon);
                //_playerData.marketData.usegunIndex = marketDataSO.gun[i].id;
                //UpdatePlayerDate();
                //break;
            }
        }

        AwakeData(GunItems, marketDataSO.gun);
    }

    void GunPoint()
    {
        PlayerPoint playerPoint = Player.GetComponent<PlayerPoint>();
        _lastGun.transform.SetParent(playerPoint.GunPoint);
        _lastGun.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void CharackterChanged(int id)
    {
        Transform Points = null;

        if (Player != null)
        {
            Player.GetComponentInChildren<Health>().Close(false);
            Destroy(Player);
        }

        for (int i = 0; i < marketDataSO.charackterLength; i++)
        {

            if (marketDataSO.charackter[i].id == id)
            {
                if (_isTranslate)
                    Points = ViewPoint;
                else
                    Points = GameObject.FindWithTag("Point").transform;

                PlayerPrefs.SetInt(useCharackterIndexKey, marketDataSO.charackter[i].id);
                OnTranslateEffect.Play();
                Player = Instantiate(marketDataSO.charackter[i].itemPrefab, Points.position, Points.rotation, GameObject.FindWithTag("Point").transform);
                Player.transform.SetParent(null);

                TranslateCharackter();
                break;
            }
        }

        Player.GetComponentInChildren<GunOffsetSettings>().SetWeaponIdlePose(selectWeapon);
        Player.GetComponent<PlayerAnimationControl>().SetMovementState(MovementState.idle);

        if (_lastGun != null)
            GunPoint();

        GunChanged(PlayerPrefs.GetInt(useGunIndexKey));
        AwakeData(CharacktersItems, marketDataSO.charackter);

    }


    bool _isTranslate = false;

    public void Translate()
    {
        _isTranslate = false;
        TranslateCharackter();
    }
    public void TranslateCharackter()
    {
        SelectWeapont();

        if (_isTranslate)
        {
            Player.GetComponentInChildren<PlayerPoint>().TranslateCharackter(true);
            Player.transform.position = ViewPoint.position;
            Player.transform.rotation = ViewPoint.rotation;
            OnTranslateEffect.Play();
        }
        else
        {
            OnTranslateEffect.Stop();
            Player.transform.position = GameObject.FindWithTag("Point").transform.position;
            Player.transform.rotation = GameObject.FindWithTag("Point").transform.rotation;
            Player.GetComponentInChildren<PlayerPoint>().TranslateCharackter(false);

        }
    }

    private void SelectWeapont()
    {
        for (int i = 0; i < marketDataSO.gun.Length; i++)
        {
            if (marketDataSO.gun[i].id == PlayerPrefs.GetInt(useSelectGunIndexKey))
            {
                selectWeapon = marketDataSO.gun[i].selectWeapon;
                Player.GetComponentInChildren<GunOffsetSettings>().SetWeaponIdlePose(selectWeapon);
                break;
            }
        }
    }

    #endregion
}
