using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using UnityEngine.Networking;

namespace com.F4A.MobileThird
{
    public enum EStateLoginFacebook
    {
        None,
        Auto,
        Manual,
    }

    [System.Serializable]
    public class DMCStoreGameInfo
    {
        [SerializeField]
        private EStorePublishType _storePublishType;
        public EStorePublishType StorePublishType
        {
            get { return _storePublishType; }
            set { _storePublishType = value; }
        }

        [SerializeField]
        private string _nameStore;
        public string NameStore
        {
            get { return _nameStore; }
            set { _nameStore = value; }
        }

        [SerializeField]
        private string _urlGameOnStore;
        public string UrlGameOnStore
        {
            get { return _urlGameOnStore; }
            set { _urlGameOnStore = value; }
        }

        [SerializeField]
        private string _urlDeveloperOnStore;
        public string UrlDeveloperOnStore
        {
            get { return _urlDeveloperOnStore; }
            set { _urlDeveloperOnStore = value; }
        }

        [SerializeField]
        private string _mailTo = "mailto:blah@blah.com?subject=Question%20on%20Awesome%20Game";
        public string MailTo
        {
            get { return _mailTo; }
            set { _mailTo = value; }
        }
    }

    [System.Serializable]
    public class F4ASocialSettingInfo
    {
        [SerializeField]
        private DMCStoreGameInfo[] _storeGameInfos;
        public DMCStoreGameInfo[] StoreGameInfos
        {
            get { return _storeGameInfos; }
            set { _storeGameInfos = value; }
        }

        public string linkWebsite = "";
        public string googlePageLink = "";
        public string linkTwitter = "";
        [SerializeField]
        private string _linkPolicy = string.Empty;
        public string LinkPolicy
        {
            get { return _linkPolicy; }
            set { _linkPolicy = value; }
        }

        [SerializeField]
        private string _linkTerms = string.Empty;
        public string LinkTerms
        {
            get { return _linkTerms; }
            set { _linkTerms = value; }
        }

        [Header("FACEBOOK")]
        public string linkImageShareFB = "";
        public string linkFacebookPage = "";
        public string linkInstagram = string.Empty;
        public string[] facebookPermisions = { "public_profile" };
        public string[] facebookQueryFields = { "id", "email", "name", "first_name", "last_name", "picture.width(120).height(120)" };
        public int widthAvatarFacebook = 64, heightAvatarFacebook = 64;
        public bool isGetUserInfoFacebook = true;
        public bool hasInviteFriend = false;

        [Header("GAME SERVICES")]
        public string versionGooglePlayServices = "";
        [SerializeField]
        private bool _isStartGameServicesWhenStart = false;
        public bool IsStartGameServicesWhenStart
        {
            get { return _isStartGameServicesWhenStart; }
            set { _isStartGameServicesWhenStart = value; }
        }
        [SerializeField]
        private bool _enableLeaderBoard = true;
        public bool EnableLeaderBoard
        {
            get { return _enableLeaderBoard; }
            set { _enableLeaderBoard = value; }
        }

        [SerializeField]
        private bool _enableAchievement = true;
        public bool EnableAchievement
        {
            get { return _enableAchievement; }
            set { _enableAchievement = value; }
        }

        public DMCStoreGameInfo GetStoreInfo()
        {
            DMCStoreGameInfo store = null;
#if UNITY_ANDROID
            store = StoreGameInfos.Where(st => st.StorePublishType == EStorePublishType.GooglePlayStore).FirstOrDefault();
#elif UNITY_IOS
            store = StoreGameInfos.Where(st => st.StorePublishType == EStorePublishType.AppleStore).FirstOrDefault();
#endif
            return store;
        }

        public string GetUrlGameOnStore()
        {
            DMCStoreGameInfo store = GetStoreInfo();
            //if(Application.platform == RuntimePlatform.Android)
            //{
            //    return "https://play.google.com/store/apps/details?id=" + Application.identifier;
            //}
            //else
            //{
            //    if (store != null)
            //    {
            //        return store.UrlGameOnStore;
            //    }
            //}

#if UNITY_ANDROID
            return "https://play.google.com/store/apps/details?id=" + Application.identifier;
#else
            if (store != null)
            {
                return store.UrlGameOnStore;
            }
#endif
            return "";
        }

        public string GetUrlDeveloperOnStore()
        {
            var store = GetStoreInfo();
            if (store != null)
            {
                return store.UrlDeveloperOnStore;
            }
            return "";
        }

        public string GetEmailContact()
        {
            var store = GetStoreInfo();
            if (store != null)
            {
                return store.MailTo;
            }
            return "";
        }
    }

    [AddComponentMenu("F4A/SocialManager")]
    public partial class SocialManager : SingletonMono<SocialManager>
	{
        private const string KeyVersionSocialConfig = "VersionSocialConfig";
        private const string NameFileSocialConfig = "SocialInfo.txt";

        [SerializeField]
	    private string urlConfigSocial = null;
	    [SerializeField]
	    private TextAsset textConfigDefault = null;
        
        [SerializeField]
        private F4ASocialSettingInfo socialSettingInfo = new F4ASocialSettingInfo();

        protected void Start()
        {
            F4ACoreManager.OnDownloadF4AConfigCompleted += F4ACoreManager_OnDownloadF4AConfigCompleted;
        }

        private void OnDestroy()
        {
            F4ACoreManager.OnDownloadF4AConfigCompleted -= F4ACoreManager_OnDownloadF4AConfigCompleted; ;
        }

        private void F4ACoreManager_OnDownloadF4AConfigCompleted(F4AConfigData config, bool success)
        {
            if (F4ACoreManager.Instance.IsGetConfigOnline)
            {
                if (config != null && !string.IsNullOrEmpty(config.urlSocial))
                {
                    urlConfigSocial = config.urlSocial;
                }

                var dataLocal = CPlayerPrefs.GetString(NameFileSocialConfig, "");
                if (config.versionSocial == CPlayerPrefs.GetInt(KeyVersionSocialConfig, 0)
                    //                    && DMCFileUtilities.IsFileExist(NameFileSocialConfig)
                    && !string.IsNullOrEmpty(dataLocal)
                    )
                {
                    if (!string.IsNullOrEmpty(dataLocal))
                    {
                        socialSettingInfo = JsonConvert.DeserializeObject<F4ASocialSettingInfo>(dataLocal);
                    }
                    InitFacebook();
                    InitGameServices();
                }
                else
                {
                    StartCoroutine(DMCMobileUtils.AsyncGetDataFromUrl(urlConfigSocial, textConfigDefault, (string data) =>
                    {
                        if (!string.IsNullOrEmpty(data))
                        {
                            socialSettingInfo = JsonConvert.DeserializeObject<F4ASocialSettingInfo>(data);
                            CPlayerPrefs.SetInt(KeyVersionSocialConfig, config.versionSocial);
                            CPlayerPrefs.SetString(NameFileSocialConfig, data);
#if UNITY_EDITOR
                            DMCFileUtilities.SaveFile(data, NameFileSocialConfig);
#endif
                        }
                        InitFacebook();
                        InitGameServices();
                    }));
                }
            }
            else
            {
                InitFacebook();
                InitGameServices();
            }
        }



#region OTHERS

        public void OpenRateGame()
	    {
	        DMCMobileUtils.OpenURL(socialSettingInfo.GetUrlGameOnStore());
        }

        public string GetLinkGame()
        {
            return socialSettingInfo.GetUrlGameOnStore();
        }

        public void OpenLinkDeveloper()
        {
            DMCMobileUtils.OpenURL(socialSettingInfo.GetUrlDeveloperOnStore());
        }

        public void OpenLinkPolicy()
        {
            DMCMobileUtils.OpenURL(socialSettingInfo.LinkPolicy);
        }

        public void OpenLinkTerms()
        {
            DMCMobileUtils.OpenURL(socialSettingInfo.LinkTerms);
        }

        public void OpenLinkFacebookPage()
        {
            DMCMobileUtils.OpenURL(socialSettingInfo.linkFacebookPage);
        }

        public void OpenLinkInstagram()
        {
            DMCMobileUtils.OpenURL(socialSettingInfo.linkInstagram);
        }

        public void OpenTwitterPage()
        {
            DMCMobileUtils.OpenURL(socialSettingInfo.linkTwitter);
        }

        internal void ShareTwitter(string styleGame)
        {
            string s = "What an awesome trending " + styleGame + " game! \nVery funny, exciting, and challenge!\nCheck it Out! \ud83d\udc49" + Application.productName + "\ud83d\udc48 \n#" + Application.productName.Replace(" ", string.Empty)
            + " #Playing #Free #" + styleGame + " #Game #Awesome #Fun #Trending #Trend #" + BuildManager.Instance.GetCompanyName();
            string str = socialSettingInfo.GetUrlGameOnStore();
            string url = @"https://twitter.com/intent/tweet?url=" + str + "&text=" + UnityWebRequest.EscapeURL(s);
            DMCMobileUtils.OpenURL(url);
        }

        public void SendMail(){
            ContactUs();
        }

        public void ContactUs()
        {
            string email = socialSettingInfo.GetEmailContact();
            string subject = EscapeURL(BuildManager.Instance.GetGameName() + " [" + BuildManager.Instance.GetVersion() + "] Support");
            string body = EscapeURL("Feedback:");
            Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
        }

        public string GetLinkImageShareFB(){
	    	return socialSettingInfo.linkImageShareFB;
	    }
	    
	    public void OpenWebsite(){
            DMCMobileUtils.OpenURL(socialSettingInfo.linkWebsite);
        }

        public void OpenGooglePage()
        {
            DMCMobileUtils.OpenURL(socialSettingInfo.googlePageLink);
        }
        
#endregion


        IEnumerator IEWaitForSeconds(float seconds, System.Action callBack)
        {
            yield return new WaitForSeconds(seconds);
            callBack();
        }

        public static string EscapeURL(string url)
        {
            return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
        }

#if UNITY_EDITOR
        public void SaveInfo()
        {
            string str = JsonConvert.SerializeObject(socialSettingInfo);
            string path = Application.dataPath + "/Common/Data/" + NameFileSocialConfig;

            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            file.WriteLine(str);
            file.Close();

            UnityEditor.AssetDatabase.Refresh();
        }

        public void LoadInfo()
        {
            string path = Application.dataPath + "/Common/Data/" + NameFileSocialConfig;
            string text = System.IO.File.ReadAllText(path);
            socialSettingInfo = JsonConvert.DeserializeObject<F4ASocialSettingInfo>(text);
            UnityEditor.AssetDatabase.Refresh();
        }

        //public void LoadGameServices()
        //{
        //    string path = System.IO.Path.Combine(Application.dataPath, "GPGSIds.cs");
        //    //string text = System.IO.File.ReadAllText(path);
        //    string[] lines = System.IO.File.ReadAllLines(path);

        //    List<string> listAchievement = new List<string>();
        //    foreach(var line in lines)
        //    {
        //        string s = line.Trim();
        //        if(s.StartsWith("public const string achievement_"))
        //        {
        //            var arr = s.Split('"');
        //            listAchievement.Add(arr[1]);
        //        }
        //        else if(s.StartsWith("public const string leaderboard_"))
        //        {
        //            var arr = s.Split('"');
        //            idBestScoreAndroid = arr[1];
        //            idBestScoreIos = arr[1];
        //        }
        //    }

        //    idAchievements = listAchievement.ToArray();
        //}
#endif
    }
}