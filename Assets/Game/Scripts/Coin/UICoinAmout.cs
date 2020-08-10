using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICoinAmout : MonoBehaviour
{
    [SerializeField]
    private Text _coinText;

    private void Awake()
    {
        if (!_coinText) _coinText = GetComponent<Text>();
    }

    private void OnEnable()
    {
        DMCGameUtilities.OnChangeCoin += DMCGameUtilities_OnChangeCoin;
        DMCGameUtilities_OnChangeCoin(DMCGameUtilities.CoinGame);
    }
    private void OnDisable()
    {
        DMCGameUtilities.OnChangeCoin -= DMCGameUtilities_OnChangeCoin;
    }

    private void DMCGameUtilities_OnChangeCoin(int coin)
    {
        _coinText.text = coin.ToString();
    }
}
