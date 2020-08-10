namespace com.F4A.MobileThird
{
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public class SaveLoadDataManager : SingletonMono<SaveLoadDataManager>
    {
        [SerializeField]
        private BaseDataController _dataController;
        public BaseDataController DataController
        {
            get { return _dataController; }
            set { _dataController = value; }
        }

        private void Awake()
        {
            if (!_dataController) _dataController = FindObjectOfType<BaseDataController>();

            _dataController.Init();
        }

        private void OnEnable()
        {
            AdsManager.OnRewardedAdCompleted += AdsManager_OnRewardedAdCompleted;

            FirebaseManager.OnLoginFacebookCompleted += FirebaseManager_OnLoginFacebookCompleted;
        }

        private void OnDisable()
        {
            AdsManager.OnRewardedAdCompleted -= AdsManager_OnRewardedAdCompleted;
            
            FirebaseManager.OnLoginFacebookCompleted -= FirebaseManager_OnLoginFacebookCompleted;
        }

        #region Events
        private void AdsManager_OnRewardedAdCompleted(ERewardedAdNetwork adNetwork, string type, double amount)
        {
#if UNITY_EDITOR
            _dataController.CheatComponent.UpdateMoneyUserReceive(adNetwork, type, amount);
#else
            if (FirebaseManager.Instance.IsSignIn())
            {
                _dataController.CheatComponent.UpdateMoneyUserReceive(adNetwork, type, amount);
            }
#endif
            SaveDataReward();
        }

        private void FirebaseManager_OnLoginFacebookCompleted(bool success, string error)
        {
            //AndroidNativeFunctions.ShowToast("FirebaseManager_OnLoginFacebookCompleted error:" + error);
            if(success)
            {
                //AndroidNativeFunctions.ShowToast("Firebase SignIn:" + FirebaseManager.Instance.IsSignIn());
                FirebaseManager.Instance.GetUserData("Reward", (ok, data) =>
                {
                    //AndroidNativeFunctions.ShowAlert("FirebaseManager_OnLoginFacebookCompleted data:" + data + " ok:" + ok, "",
                    //    "OK", "Cancel", "Never", null);
                    if (ok && data != null)
                    {
                        try
                        {
                            _dataController.CheatComponent = JsonConvert.DeserializeObject<DMCCheatComponent>(data.ToString());
                        }
                        catch (Exception ex)
                        {
                            _dataController.CheatComponent = new DMCCheatComponent();
                            SaveLoadDataManager.Instance.SaveDataReward();
                        }
                    }
                    else
                    {
                        _dataController.CheatComponent = new DMCCheatComponent();
                        SaveLoadDataManager.Instance.SaveDataReward();
                    }
                });
            }
            //else
            //{
            //    AndroidNativeFunctions.ShowAlert("FirebaseManager_OnLoginFacebookCompleted error:" + error, "",
            //        "OK", "Cancel", "Never", null);
            //}
        }
        #endregion

        //public void Save<T>(T saveData, string savePath)
        //{
        //    try
        //    {
        //        //lock (m_saveLock)
        //        //{
        //        //    using (FileStream fileStream = new FileStream(savePath, FileMode.OpenOrCreate))
        //        //    {
        //        //        m_binaryFormatter.Serialize(fileStream, saveData);
        //        //        fileStream.Close();
        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.LogError(ex.Message);
        //    }
        //}

        private void SaveDataReward()
        {
            var json = JsonConvert.SerializeObject(_dataController.CheatComponent);
            FirebaseManager.Instance.SaveDatabase("Reward", json);
        }
        public void SaveData(string databaseName)
        {
            var data = _dataController.GetData().ToString();
            FirebaseManager.Instance.SaveDatabase(databaseName, data);
        }

        public void SaveData(string databaseName, string data)
        {
            FirebaseManager.Instance.SaveDatabase(databaseName, data);
        }

        public void LoadData(string data)
        {
            _dataController.LoadData(data);
        }
    }
}