using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

#if DEFINE_SHEPHERTZ
using com.shephertz.app42.paas.sdk.csharp;
using App42Social = com.shephertz.app42.paas.sdk.csharp.social;
using com.shephertz.app42.paas.sdk.csharp.storage;
#endif

using System;

namespace com.F4A.MobileThird
{
    [HelpURL("http://api.shephertz.com/app42-dev.php")]
    [AddComponentMenu("F4A/ShephertzManager")]
    public partial class ShephertzManager : ManualSingletonMono<ShephertzManager>
    {
        public static Action<object> OnLoginSocialSuccess = delegate { };
        public static Action OnLoginSocialError = delegate { };
        public static Action<EStorageCallBackType, object> OnStorageServiceSuccess = delegate { };
        public static Action<EStorageCallBackType, Exception> OnStorageServiceError = delegate { };

        [Header("INFO CONFIG")]
        [SerializeField]
        private string apiKey = "";
        [SerializeField]
        private string secretKey = "";

        [SerializeField]
        private bool isDebug = true;

#if DEFINE_SHEPHERTZ
        private App42Social.SocialService socialService = null;
        private StorageService storageService = null;
#endif
	    
        [SerializeField]
        private string dbName = "", collectionNameUserInfo = "";

        public string Id = "", UserName = "", LinkFacebook = "";

        [HideInInspector]
        public bool IsLogin = false;


        protected override void Initialization()
        {
#if DEFINE_SHEPHERTZ
            App42Log.SetDebug(isDebug);        //Print output in your editor console 
            App42API.Initialize(apiKey, secretKey);
            socialService = App42API.BuildSocialService();
            storageService = App42API.BuildStorageService();
#endif
        }


        /// <summary>
        /// Logins the facebook.
        /// </summary>
        public void LoginFacebook()
        {
#if DEFINE_SHEPHERTZ && DEFINE_FACEBOOK_SDK
            var accessToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            string strToken = accessToken.TokenString;
            LoginFacebook(strToken, UserName);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        public void LoginFacebook(string accessToken, string userName)
        {
#if DEFINE_SHEPHERTZ
			if (socialService != null && !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(accessToken))
			{
				socialService.LinkUserFacebookAccount(userName, accessToken, new App42LoginFacebookCallBack());
			}
#endif
        }

        /// <summary>
        /// Finds the document by key value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <param name="collectionName">Collection name.</param>
        /// <param name="callBackType">Call back type.</param>
        public void FindDocumentByKeyValue(string key, string value, string collectionName, EStorageCallBackType callBackType)
        {
#if DEFINE_SHEPHERTZ
			if (socialService != null && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
			{
				storageService.FindDocumentByKeyValue(dbName, collectionName, key, value, new App42StorageCallBack() { callBackType = callBackType });
			}
#endif
        }

        public void FindUserById()
        {
            FindUserById(EStorageCallBackType.FindUserById);
        }

        /// <summary>
        /// 
        /// </summary>
        public void FindUserById(EStorageCallBackType callBackType)
        {
            FindDocumentByKeyValue("id", Id, collectionNameUserInfo, callBackType);
        }

        public void SaveOrUpdateDocumentByKeyValue(string key, string value, string json, string collectionName)
        {
#if DEFINE_SHEPHERTZ
			if (socialService != null && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value)){
				storageService.SaveOrUpdateDocumentByKeyValue(dbName, collectionName,key, value, json, new App42StorageCallBack());
			}
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
		public void SaveOrUpdateDocumentByUserName(string json)
        {
#if DEFINE_SHEPHERTZ
            if (IsLogin && !string.IsNullOrEmpty (UserName)) {
				SaveOrUpdateDocumentByKeyValue ("userName", UserName, json, collectionNameUserInfo);
			}
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        public void SaveOrUpdateDocumentById(string json)
        {
#if DEFINE_SHEPHERTZ
            if (IsLogin && !string.IsNullOrEmpty(Id))
            {
				SaveOrUpdateDocumentByKeyValue("id", Id, json, collectionNameUserInfo);
            }
#endif
        }





        //######################
        //######################
        //######################
        Dictionary<string, object> dicInfo = new Dictionary<string, object>();

        public void SaveInfo()
        {
#if UNITY_EDITOR
            dicInfo.Clear();
            dicInfo["apiKey"] = apiKey;
            dicInfo["secretKey"] = secretKey;
            dicInfo["isDebug"] = isDebug;
            dicInfo["dbName"] = dbName;
			dicInfo["collectionNameUserInfo"] = collectionNameUserInfo;
			dicInfo ["collectionNameGiftCode"] = collectionNameGiftCode;
            var str = JsonConvert.SerializeObject(dicInfo);
            string path = Application.dataPath + "/Common/Data/ShephertzInfo.txt";

            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            file.WriteLine(str);
            file.Close();

            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        public void LoadInfo()
        {
#if UNITY_EDITOR
            string path = Application.dataPath + "/Common/Data/ShephertzInfo.txt";

            string text = System.IO.File.ReadAllText(path);
            dicInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);

            apiKey = (string)dicInfo["apiKey"];
            secretKey = (string)dicInfo["secretKey"];
            isDebug = bool.Parse(dicInfo["isDebug"].ToString());
            dbName = (string)dicInfo["dbName"];
			collectionNameUserInfo = (string)dicInfo["collectionNameUserInfo"];
			collectionNameGiftCode = dicInfo ["collectionNameGiftCode"].ToString ();
#endif
        }
    }

    public enum EStorageCallBackType
    {
        None,
        FindUserById,
        FindGiftCode,
    }

#if DEFINE_SHEPHERTZ
    public class App42LoginFacebookCallBack : App42CallBack
    {
        public void OnException(Exception ex)
        {
            if (ShephertzManager.OnLoginSocialError != null)
            {
                ShephertzManager.OnLoginSocialError();
            }
            App42Log.Console("Exception : " + ex);
        }

        public void OnSuccess(object response)
        {
            App42Social.Social social = (App42Social.Social)response;
            if(social != null)
            {
                if (!string.IsNullOrEmpty(social.GetFacebookAccessToken()))
                {
					ShephertzManager.Instance.IsLogin = true;
                }
            }

            if (ShephertzManager.OnLoginSocialSuccess != null)
            {
                ShephertzManager.OnLoginSocialSuccess(response);
            }
        }
    }

    public class App42StorageCallBack : App42CallBack
    {
        public EStorageCallBackType callBackType = EStorageCallBackType.None;
        public void OnException(Exception ex)
        {
            //App42Log.Console("Exception: " + ex);
            if(ShephertzManager.OnStorageServiceError != null)
            {
                ShephertzManager.OnStorageServiceError(callBackType, ex);
            }
        }

        public void OnSuccess(object response)
        {
            //App42Log.Console("Exception: " + ex);
            if (ShephertzManager.OnStorageServiceSuccess != null)
            {
                ShephertzManager.OnStorageServiceSuccess(callBackType, response);
            }
        }
    }
#endif
}