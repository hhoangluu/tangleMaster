using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DMCGameUtilities
{
    public static event Action<int> OnChangeCoin = delegate { };
    public static int CoinGame
    {
        get
        {
            return CPlayerPrefs.GetInt("TM_COIN", 0);
        }
        set
        {
            CPlayerPrefs.SetInt("TM_COIN", value);
            OnChangeCoin?.Invoke(value);
        }
    }

    public static event Action<int> OnChangeCurrentLevel = delegate { };
    public static int LevelCurrent
    {
        get { return CPlayerPrefs.GetInt("TM_CURRENTLEVEL", 0); }
        set
        {
            CPlayerPrefs.SetInt("TM_CURRENTLEVEL", value);
            OnChangeCurrentLevel?.Invoke(value);
        }
    }

    public static event Action<int> OnChangeMaterialRope = delegate { };
    public static int MaterialRopeCurrent
    {
        get { return CPlayerPrefs.GetInt("TM_MaterialRopeCurrent", 0); }
        set
        {
            CPlayerPrefs.SetInt("TM_MaterialRopeCurrent", value);
            OnChangeMaterialRope?.Invoke(value);
        }
    }
}
