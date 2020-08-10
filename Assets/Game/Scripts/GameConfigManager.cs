using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigManager : FiveSingleton<GameConfigManager>
{
    public int CoinReceiveWhenDoneLevel = 5;
    public int CoinReceiveWatchVideoDoneLevel
    {
        get { return CoinReceiveWhenDoneLevel * 3; }
    }
}
