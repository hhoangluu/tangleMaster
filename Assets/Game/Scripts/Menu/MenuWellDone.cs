using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.VersionControl;

public class MenuWellDone : MenuAbs<MenuWellDone>
{
    [SerializeField]
    private List<MenuComponent> _menuComponents;
    [SerializeField]
    private Image _bg;

    [SerializeField]
    private Image _fillRewardProgress;
    [SerializeField]
    private Text _fillRewardProgressText;

    [SerializeField]
    private Text _rewardX3Amount;
    public Text rewardX3Amount => _rewardX3Amount;

    [SerializeField]
    private Text _rewardAmount;
    public Text rewardAmount => _rewardAmount;

    private float _menuSpeed = 0.5f;
    private Coroutine _bgCorou;

    [SerializeField]
    private GameObject _loadingx3RewardGO;

    private void Start()
    {
        DOVirtual.DelayedCall(2, () => 
        {
            _loadingx3RewardGO.SetActive(!PluginManager.instance.IsRewardAdsReady());
        });
    }

    public override void Open()
    {
        _isOpened = true;
        ColorLerp(ref _bgCorou, _bg, _bg.color, new Color(_bg.color.r, _bg.color.g, _bg.color.b, 0.5f), _menuSpeed, onStart: () => _bg.gameObject.SetActive(true));
        _menuComponents.ForEach(mC => mC.Open(_menuSpeed));
        components.SetActive(true);

        int coin3 = GameConfigManager.instance.CoinReceiveWatchVideoDoneLevel * DMCGameUtilities.LevelCurrent;
        if (coin3 > 300) coin3 = 300;
        rewardX3Amount.text = coin3.ToString();

        int coin = GameConfigManager.instance.CoinReceiveWhenDoneLevel * DMCGameUtilities.LevelCurrent;
        if (coin > 100) coin = 100;
        rewardAmount.text = coin.ToString();
    }

    public override void Close()
    {
        if (!_isOpened) return;
        _isOpened = false;
        ColorLerp(ref _bgCorou, _bg, _bg.color, new Color(_bg.color.r, _bg.color.g, _bg.color.b, 0f), _menuSpeed, onDone: () => _bg.gameObject.SetActive(false));
        _menuComponents.ForEach(mC => mC.Close(_menuSpeed));
        components.SetActive(false);
    }

    public void GetX3Reward()
    {
        //TangleMasterGame.instance.LoadNextLevel();
        //Close();

        if (!PluginManager.instance.IsRewardAdsReady()) return;

        PluginManager.instance.ShowRewardAds(delegate
        {
            int coin3 = GameConfigManager.instance.CoinReceiveWatchVideoDoneLevel * DMCGameUtilities.LevelCurrent;
            if (coin3 > 300) coin3 = 300;
            DMCGameUtilities.CoinGame += coin3;
            TangleMasterGame.instance.LoadNextLevel();
            Close();
        }, delegate
        {
            int coin = GameConfigManager.instance.CoinReceiveWhenDoneLevel * DMCGameUtilities.LevelCurrent;
            if (coin > 100) coin = 100;
            DMCGameUtilities.CoinGame += coin;
            TangleMasterGame.instance.LoadNextLevel();
            Close();
        });
    }

    public void GetReward()
    {
        int coin = GameConfigManager.instance.CoinReceiveWhenDoneLevel * DMCGameUtilities.LevelCurrent;
        if (coin > 100) coin = 100;
        DMCGameUtilities.CoinGame += 100;
        TangleMasterGame.instance.LoadNextLevel();
        Close();
    }
}
