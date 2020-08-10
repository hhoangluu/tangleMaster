namespace com.F4A.MobileThird
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    [Serializable]
    public class DMCCheatComponent
    {
        [SerializeField]
        private float _totalMoneyReward = 0.0f;
        public float TotalMoneyReward
        {
            get { return _totalMoneyReward; }
            set { _totalMoneyReward = value; }
        }

        [SerializeField]
        private float _totalMoneyUserReceive = 0.0f;
        public float TotalMoneyUserReceive
        {
            get { return _totalMoneyUserReceive; }
            set { _totalMoneyUserReceive = value; }
        }

        [SerializeField]
        public int _countTotalGetReward = 0;
        public int CountTotalGetReward
        {
            get { return _countTotalGetReward; }
            set { _countTotalGetReward = value; }
        }

        public void UpdateMoneyUserReceive(ERewardedAdNetwork adNetwork, string type, double amount)
        {
            _totalMoneyReward += (float)amount;
            _totalMoneyUserReceive += (float)amount * 0.15f;
            _countTotalGetReward++;
        }
    }

    [Serializable]
    public class DMCCoreDataInfo //: DMCBaseDataInfo<DMCCoreDataInfo>
    {
        [SerializeField]
        private bool _isRemoveAds;
        public bool IsRemoveAds
        {
            get { return _isRemoveAds; }
            set
            {
                _isRemoveAds = value;
            }
        }

        [SerializeField]
        private bool _isEnableSound = true;
        public bool IsEnableSound
        {
            get { return _isEnableSound; }
            set
            {
                _isEnableSound = value;
            }
        }

        [SerializeField]
        private bool _isEnableMusic = true;
        public bool IsEnableMusic
        {
            get { return _isEnableMusic; }
            set
            {
                _isEnableMusic = value;
            }
        }

        [SerializeField]
        private bool _isEnableVibrate = true;
        public bool IsEnableVibrate
        {
            get { return _isEnableMusic; }
            set
            {
                _isEnableMusic = value;
            }
        }
    }
}
