using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Linq;
#if UNITY_2018_3_OR_NEWER
using UnityEngine.Networking;
#endif

namespace com.F4A.MobileThird
{
    [System.Serializable]
    public class WebViewRowData
    {
        [JsonProperty("id")]
        public string id = "";
        [JsonProperty("offname")]
        public string offname = "";
        [JsonProperty("image")]
        public string image = "";
        [JsonProperty("des")]
        public string description = "";
        [JsonProperty("link_preview")]
        public string linkPreview = "";
        [JsonProperty("link")]
        public string link = "";

//#if UNITY_2018_3_OR_NEWER
//        [HideInInspector]
//        public UnityWebRequest wwwIconApp = null;
//#else
        [HideInInspector]
	    public WWW wwwIconApp = null;
//#endif
        [JsonIgnore]
	    public bool isIconAppError = false;

        public bool IsCorrectData()
        {
            if (wwwIconApp != null && wwwIconApp.texture != null && !isIconAppError)
            {
                return true;
            }
            else
            {
                return false;
            }

            //return true;
        }
    }

    [System.Serializable]
    public class WebViewConfigData
    {
        [JsonProperty("linkRequestAndroid")]
        public string linkRequestAndroid = "";
        [JsonProperty("linkRequestIos")]
        public string linkRequestIos = "";
        [JsonProperty("random")]
        public int random = 10;
        [JsonProperty("username")]
        public string username = "";
        [JsonProperty("idfa")]
        public string idfa = "";
        [JsonProperty("active_ads")]
        public bool activeAds = false;
    }

    [System.Serializable]
    public class WebViewRequestData
	{
        public bool useRequestData = true;

        public WebViewConfigData webViewConfigData = new WebViewConfigData();

        public string devicename = "";
        public int width = 375;
        public int height = 667;
        public string timezone = "-8";
        public string cname = "";
        public string ccountry = "US";
        public string ccountrycode = "310";
        public string cnetworkcode = "300";
        public string language = "en-US";
        public string region = "en-US";
        public int platform = 1;


        public string CreateRequestLink()
        {
	        string linkRequest = "";
#if UNITY_ANDROID
	        linkRequest = webViewConfigData.linkRequestAndroid;
#elif UNITY_IOS
	        linkRequest = webViewConfigData.linkRequestIos;
#else
	        linkRequest = webViewConfigData.linkRequestAndroid;
#endif
            if (useRequestData)
            {
                if (linkRequest.EndsWith("/"))
                {
                    linkRequest = linkRequest.Remove(linkRequest.Length - 1);
                }
                linkRequest += "?random=" + webViewConfigData.random;
                linkRequest += "&username=" + webViewConfigData.username;
                linkRequest += "&idfa=" + webViewConfigData.idfa;
                linkRequest += "&devicename=" + devicename;
                linkRequest += "&width=" + width;
                linkRequest += "&height=" + height;
                linkRequest += "&timezone=" + timezone;
                linkRequest += "&cname=" + cname;
                linkRequest += "&ccountry=" + ccountry;
                linkRequest += "&ccountrycode=" + ccountrycode;
                linkRequest += "&cnetworkcode=" + cnetworkcode;
                linkRequest += "&language=" + language;
                linkRequest += "&region=" + region;
	            //linkRequest += "&platform=" + platform;
            }
	        Debug.Log("linkRequest: " + linkRequest);
            return linkRequest;
        }
    }

    public partial class AdsManager
    {
        [Header("Ad Web View Scroll")]
        [SerializeField]
        private string urlWebViewConfig = null;
        [SerializeField]
        private TextAsset webViewConfigAsset = null;
        [SerializeField]
        private WebViewRequestData webViewRequestData = new WebViewRequestData();
        [SerializeField]
        private TextAsset textWebViewDataDefault = null;

        public List<WebViewRowData> webViewRowDatas = new List<WebViewRowData>();

        private bool isLoadWebViewCompleted = false;

        [SerializeField]
        private UIWebViewScrollController webViewScrollController = null;
        [SerializeField]
	    private Transform bannerWebViewTF = null;
	    [SerializeField]
	    private float scaleBannerWebView = 0.75f;
        private UIRowAppController rowAppControllerBanner = null;

        protected void InitWebViewScroll()
	    {
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
		    webViewRequestData.devicename = SystemInfo.deviceName;
		    webViewRequestData.width = Screen.width;
		    webViewRequestData.height = Screen.height;
		    webViewRequestData.timezone = System.TimeZone.CurrentTimeZone.ToString();
		    webViewRequestData.devicename = SystemInfo.deviceModel;
		    webViewRequestData.language = Application.systemLanguage.ToString();
		    webViewRequestData.region = Application.systemLanguage.ToString();
#if UNITY_EDITOR
	        webViewRequestData.platform = 2;
#elif UNITY_ANDROID
            webViewRequestData.platform = 1;
#elif UNITY_IOS
            webViewRequestData.platform = 2;
#endif
#endif


            if (webViewScrollController)
            {
                webViewScrollController.HideWebViewScroll();
            }
            
		    if(bannerWebViewTF){
		    	bannerWebViewTF.gameObject.SetActive(false);
		    }
        }

	    public void LoadWebViewScroll()
	    {
            isLoadWebViewCompleted = false;
            StartCoroutine(AsynLoadFileConfigWebView());
        }

        private IEnumerator AsynLoadFileConfigWebView()
	    {
            yield return null;
            StartCoroutine(DMCMobileUtils.AsyncGetDataFromUrl(urlWebViewConfig, webViewConfigAsset, (string data) => 
            {
                if (!string.IsNullOrEmpty(data))
                {
                    webViewRequestData.webViewConfigData = JsonConvert.DeserializeObject<WebViewConfigData>(data);
                }
                if (webViewRequestData.webViewConfigData.activeAds)
                {
                    StartCoroutine(AsynLoadFileDataWebView());
                }
            }));
        }

        public void LoadFileDataWebView()
        {
            isLoadWebViewCompleted = false;
            StartCoroutine(AsynLoadFileDataWebView());
        }

        private IEnumerator AsynLoadFileDataWebView()
	    {
            string linkGetData = webViewRequestData.CreateRequestLink();
            string text = "";
            if (!string.IsNullOrEmpty(linkGetData))
            {
            	//F4ACoreManager.Instance.Toast("AsynLoadFileWebView linkGetData: " + linkGetData);
                WWW www = new WWW(linkGetData);
#if UNITY_WEBGL
	            bool isDone = false;
	            float timeRun = 0;
	            while(!isDone && timeRun <= 8){
		            isDone = www.isDone;
		            timeRun += Time.deltaTime;
	            }
#else
	            while (!www.isDone)
	            {
	                yield return null;
	            }
#endif
                yield return www;
#if UNITY_WEBGL
	            if (string.IsNullOrEmpty(www.error) && !string.IsNullOrEmpty(www.text))
	            {
		            text = www.text;
		            //F4ACoreManager.Instance.Toast("AsynLoadFileWebView text: " + text, false);
	            }
#else
	            if (string.IsNullOrEmpty(www.error))
                {
                    text = System.Text.Encoding.UTF8.GetString(www.bytes);
	            }
#endif
            }

            if (string.IsNullOrEmpty(text) && textWebViewDataDefault)
            {
                text = textWebViewDataDefault.ToString();
            }

            if (!string.IsNullOrEmpty(text))
            {
                JArray job = JsonConvert.DeserializeObject<JArray>(text);
                if(job.HasValues)
                {
	                webViewRowDatas.Clear();
                    webViewRowDatas = JsonConvert.DeserializeObject<List<WebViewRowData>>(text);
                    //List<WebViewRowData> webViewRowDatasTemp = new List<WebViewRowData>();
                    //webViewRowDatasTemp = JsonConvert.DeserializeObject<List<WebViewRowData>>(text);
                    //foreach(var row in webViewRowDatas){
                    //       webViewRowDatasTemp.Where(r=>r.id.)
                    //}
                }
            }
            StartCoroutine(AsynLoadContentDataWebView());
        }

        private IEnumerator AsynLoadContentDataWebView()
	    {
            yield return null;
            if (webViewRowDatas != null && webViewRowDatas.Count > 0)
            {
                foreach (var data in webViewRowDatas)
                {
	                if (!string.IsNullOrEmpty(data.linkPreview) 
		                && data.linkPreview.Contains("https://itunes.apple.com"))
                    {
                        var array = data.linkPreview.Split('/');
                        int len = array.Length;
                        if (len > 0 && array[len - 1].Contains("id"))
                        {
                            string idApp = array[len - 1].Replace("id", "");
                            idApp = idApp.Split('?')[0];
#if UNITY_EDITOR
                            Debug.Log("idApp: " + idApp);
#endif
                            WWW www = new WWW("https://itunes.apple.com/lookup?id=" + idApp);
#if UNITY_WEBGL
	                        bool isDone = false;
	                        float timeRun = 0;
	                        while(!isDone && timeRun < 8){
	                            timeRun += Time.deltaTime;
	                            isDone = www.isDone;
	                        }
#else
                            while (!www.isDone)
                            {
                                yield return null;
                            }
#endif
                            yield return www;
                            if (string.IsNullOrEmpty(www.error))
                            {
                                string text = System.Text.Encoding.UTF8.GetString(www.bytes);
                                JObject jobject = JsonConvert.DeserializeObject<JObject>(text);
                                if (jobject["results"] != null && !string.IsNullOrEmpty(jobject["results"].ToString()))
                                {
                                    JArray results = JsonConvert.DeserializeObject<JArray>(jobject["results"].ToString());
                                    if (results.Count > 0)
                                    {
                                        var result = JsonConvert.DeserializeObject<JObject>(results[0].ToString());
                                        if (result["artworkUrl60"] != null
                                           && !string.IsNullOrEmpty(result["artworkUrl60"].ToString())
                                        //&& WebFileExists(result["artworkUrl60"].ToString())
                                        )
                                        {
#if UNITY_EDITOR
                                            Debug.Log("Load image game " + data.offname + " at: " + result["artworkUrl60"].ToString());
#endif
                                            data.wwwIconApp = new WWW(result["artworkUrl60"].ToString());
#if UNITY_WEBGL
	                                        isDone = false;
	                                        timeRun = 0;
	                                        while(!isDone && timeRun < 8){
	                                            timeRun += Time.deltaTime;
	                                            isDone = data.wwwIconApp.isDone;
	                                        }
#else
                                            while (!data.wwwIconApp.isDone)
                                            {
                                                yield return null;
                                            }
#endif
                                            data.isIconAppError = false;
                                            yield return data.wwwIconApp;
                                        }
                                        else
                                        {
                                            data.isIconAppError = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
#if UNITY_EDITOR
                                Debug.Log("error load image: " + www.error);
#endif
                            }
                        }
                    }
#if UNITY_ANDROID || UNITY_IOS
                    else if (WebFileExists(data.image))
                    {
                        Debug.Log("Load image at: " + data.image);
                        data.wwwIconApp = new WWW(data.image);
                        yield return data.wwwIconApp;
	                }
	                else
	                {
	                	data.isIconAppError = true;
	                }
#else
	                //Debug.Log("Load image at: " + data.image);
	                //F4ACoreManager.Instance.Toast("Load image at: " + data.image);
	                data.wwwIconApp = new WWW(data.image);
	                bool isCompleted = false;
	                float timeDelay = 0;
	                while(!isCompleted && timeDelay <= 8){
		                isCompleted = data.wwwIconApp.isDone;
		                timeDelay += Time.deltaTime;
	                }
	                data.isIconAppError = false;
	                yield return data.wwwIconApp;
#endif
                    yield return null;
                }
                yield return null;
                Debug.Log("AsynLoadContentWebView Completed!");
	            isLoadWebViewCompleted = true;
	            //F4ACoreManager.Instance.Toast("AsynLoadContentWebView Completed!");
            }
            else
            {
                Debug.Log("AsynLoadContentWebView Failed!");
	            isLoadWebViewCompleted = false;
	            //F4ACoreManager.Instance.Toast("AsynLoadContentWebView Failed!");
            }
        }
        
	    static public bool WebFileExists (string uri) {
		    long fileLength = -1;
		    WebRequest request = HttpWebRequest.Create(uri);
		    request.Method = "HEAD";
		    WebResponse resp = null;
		    try {
			    resp = request.GetResponse();
		    } catch {
			    resp = null;
		    }
		    if (resp != null) {
			    long.TryParse(resp.Headers.Get("Content-Length"), out fileLength);
		    }
		    return fileLength > 0;
     }

        protected bool ShowInterstitialWebView()
        {
	        if (isLoadWebViewCompleted && webViewScrollController && webViewRowDatas.Count > 0)
            {
                return webViewScrollController.ShowWebViewScroll(webViewRowDatas);
            }
            return false;
        }

        protected bool IsBannerWebViewReady()
        {
            if (isLoadWebViewCompleted && webViewRequestData.webViewConfigData.activeAds)
            {
                return true;
            }
            return false;
        }


        protected bool ShowBannerWebView(){
	        if (webViewRequestData.webViewConfigData.activeAds && isLoadWebViewCompleted && webViewRequestData.webViewConfigData.activeAds && webViewRowDatas.Count > 0)
	    	{
	    		if(!rowAppControllerBanner){
	    			var row = webViewScrollController.CreateRowAppController();
	    			if(row && row.GetComponent<UIRowAppController>()){
	    				rowAppControllerBanner = row.GetComponent<UIRowAppController>();
	    			}
	    		}
	    		if(rowAppControllerBanner){
	    			int lenght = webViewRowDatas.Count;
	    			var data = webViewRowDatas[Random.Range(0, lenght)];
	    			int count = 0;
	    			while(!data.IsCorrectData() && count < 5){
	    				count++;
	    				data = webViewRowDatas[Random.Range(0, lenght)];
	    			}
	    			
	    			if(data.IsCorrectData()){
	    				rowAppControllerBanner.SetupRowApp(data);
		    			rowAppControllerBanner.transform.parent = bannerWebViewTF;
		    			rowAppControllerBanner.gameObject.SetActive(true);
		    			rowAppControllerBanner.transform.localPosition = Vector3.zero;
		    			rowAppControllerBanner.transform.localScale = Vector3.one * scaleBannerWebView;
		    			rowAppControllerBanner.OnDownload -= RowAppControllerBanner_OnDownload;
		    			rowAppControllerBanner.OnDownload += RowAppControllerBanner_OnDownload;
	    			}
                }
                if (bannerWebViewTF)
                {
                    bannerWebViewTF.gameObject.SetActive(true);
                }
            }
		    return false;
	    }

        private void RowAppControllerBanner_OnDownload()
        {
            ShowBannerWebView();
        }

        protected bool HideBannerWebView(){
	    	if(bannerWebViewTF && bannerWebViewTF.gameObject.activeSelf){
	    		bannerWebViewTF.gameObject.SetActive(false);
	    	}
	    	return false;
	    }
    }
}