using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class CoinsManager : FiveSingleton<CoinsManager>
{
    [SerializeField]
    private GameObject _coinSpreadPrefab;

    [SerializeField]
    private List<Sprite> _coinSprites;

    private const string KEY_COIN_CURRENT = "KEY_COIN_CURRENT";
    public int curCoin
    {
        get => PlayerPrefs.GetInt(KEY_COIN_CURRENT, 0);
        private set
        {
            instance.LerpCoin(curCoin, value, 1f);
            PlayerPrefs.SetInt(KEY_COIN_CURRENT, value);
        }
    }

    public void AddCoin(int amount)
    {
        curCoin += amount;
    }

    public void SubstractCoin(int amount)
    {
        if (curCoin - amount <= 0)
            curCoin = 0;
        else
            curCoin -= amount;
    }

    private Coroutine coinCorou;

    private void LerpCoin(int cur, int tar, float timeMultipler)
    {
        if (coinCorou != null) StopCoroutine(coinCorou);
        coinCorou = StartCoroutine(LerpCoinIE(CoinBar.instance.curCoin, cur, tar, timeMultipler));
    }

    private IEnumerator LerpCoinIE(Text showText, int cur, int tar, float timeMultipler)
    {
        float t = 0f;
        while (t < 1f)
        {
            showText.text = ((int)Mathf.Lerp(cur, tar, t)).ToString();
            t += Time.deltaTime * timeMultipler;
            yield return GameManager.WaitForEndOfFrame;
        }
        showText.text = tar.ToString();
    }

    private int spreadAndCollectCount;

    public void SpreadAndCollectGold(Transform parent, int goldAmount)
    {
        StartCoroutine(SpreadAndCollectGoldIE(parent, goldAmount));
        spreadAndCollectCount += 1;
    }

    private IEnumerator SpreadAndCollectGoldIE(Transform parent, int amount)
    {
        List<CoinSpread> spreads = CreateGoldSpreads(parent, amount / 10);

        foreach (var gS in spreads)
        {
            gS.LerpTo(GetRandomPosInCircle(parent.position), 2f);
        }

        if (CoinBar.instance.curMenuState != MenuState.sub)
            CoinBar.instance.Sub(1f);

        yield return new WaitForSeconds(2f);

        foreach (var gS in spreads)
        {
            gS.LerpToGoldIcon(CoinBar.instance.iconCoin.transform, 2f, () =>
            {
                spreadAndCollectCount -= 1;
                if (spreadAndCollectCount == 0)
                    CoinBar.instance.Close(1f);
                Destroy(gS.gameObject);
            });
        }
        AddCoin(amount);
        yield break;
    }

    private List<CoinSpread> CreateGoldSpreads(Transform parent, int amountSpread)
    {
        List<CoinSpread> goldSpreads = new List<CoinSpread>();
        for (int i = 0; i < amountSpread; i++)
        {
            CoinSpread goldSpread = CreateGoldSpread();
            goldSpread.transform.position = parent.position;
            goldSpreads.Add(goldSpread);
        }
        return goldSpreads;
    }

    private CoinSpread CreateGoldSpread()
    {
        CoinSpread goldSpread = Instantiate(_coinSpreadPrefab, transform).GetComponent<CoinSpread>();
        goldSpread.coinImage.sprite = GetRandomGoldSprite();
        goldSpread.gameObject.SetActive(true);
        return goldSpread;
    }

    private Sprite GetRandomGoldSprite()
    {
        return _coinSprites[UnityEngine.Random.Range(0, _coinSprites.Count)];
    }

    private Vector2 GetRandomPosInCircle(Vector3 tarPos)
    {
        Vector2 cirPos = UnityEngine.Random.insideUnitCircle * 2.5f;
        return new Vector2(tarPos.x + cirPos.x, tarPos.y + cirPos.y);
    }
}
