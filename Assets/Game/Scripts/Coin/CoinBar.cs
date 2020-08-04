using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoinBar : FiveSingleton<CoinBar>
{
    [SerializeField]
    private MenuComponent _menuComponent;

    [SerializeField]
    private Image _iconCoin;
    public Image iconCoin => _iconCoin;

    [SerializeField]
    private Text _curCoin;
    public Text curCoin => _curCoin;

    public MenuState curMenuState => _menuComponent.curMenuState;

    public void SetAsClose()
    {
        _menuComponent.SetAsClosed();
    }

    public void Open(float inTime)
    {
        _menuComponent.Open(inTime);
    }

    public void Close(float inTime)
    {
        _menuComponent.Close(inTime);
    }

    public void Sub(float inTime)
    {
        _menuComponent.Sub(inTime);
    }
}
