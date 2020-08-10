using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using UnityEngine.Networking;

namespace com.F4A.MobileThird
{
    [AddComponentMenu("F4A/DailyRewardsManager")]
	public class DailyRewardsManager : SingletonMonoDontDestroy<DailyRewardsManager>
    {
        public string worldClockUrl = "http://worldclockapi.com/api/json/est/now";  // The World Clock JSON API
        public string worldClockFMT = "yyyy-MM-dd'T'HH:mmzzz";  // World Clock Format

        public bool useWorldClockApi = true;                    // Use World Clock API
        public WorldClock worldClock;                           // The World Clock object

        public string errorMessage;                             // Error Message
        public bool isErrorConnect;                             // Some error happened on connecting?
        public DateTime now;                                    // The actual date. Either returned by the using the world clock or the player device clock

        public int maxRetries = 3;                              // The maximum number of retries for the World Clock connection
        private int connectrionRetries;                         // Retries counter

        // Delegates
        public delegate void DelInitialize(bool error = false, string errorMessage = ""); // When the timer initializes. Sends an error message in case it happens. Should wait for this delegate if using World Clock API
        public static DelInitialize onInitialize;

        // Delegates
        public delegate void OnClaimPrize(System.Object info);                 // When the player claims the prize
        public static OnClaimPrize onClaimPrize;

        public bool isInitialized = false;

		protected override void Initialization ()
		{
			onInitialize += OnInitialize;
            connectrionRetries = 0;
            StartCoroutine(IEInitializationDailyReward());
		}

//        protected void Awake()
//        {
//            onInitialize += OnInitialize;
//            connectrionRetries = 0;
//            StartCoroutine(IEInitializationDailyReward());
//        }

		public void Reconect(){
			connectrionRetries = 0;
			StopCoroutine (IEInitializationDailyReward());
			StartCoroutine(IEInitializationDailyReward());
		}

        private void OnInitialize(bool error = false, string errorMessage = "")
        {
            if (error)
            {
                StartCoroutine(IEWaitForSeconds(120, () => 
                {
                    StartCoroutine(IEInitializationDailyReward());
                }));
            }
            else
            {

            }
        }

        public IEnumerator IEInitializationDailyReward()
        {
            yield return StartCoroutine(IEInitializeDate());

            if (isErrorConnect)
            {
                if (onInitialize != null) onInitialize(true, errorMessage);
            }
            else
            {
                // We don't count seconds on Daily Rewards
                now = now.AddSeconds(-now.Second);
                if(onInitialize != null) onInitialize();
            }
        }

        // Initializes the current DateTime. If the player is using the World Clock initializes it
        public IEnumerator IEInitializeDate()
        {
            if (useWorldClockApi)
            {
                UnityWebRequest www = UnityWebRequest.Get(worldClockUrl);
                yield return www.SendWebRequest();
                while (!www.isDone)
                {
                    yield return null;
                }

                if (!string.IsNullOrEmpty(www.error))
                {
                    // Try again to connect
                    if (connectrionRetries < maxRetries)
                    {
                        connectrionRetries++;
                        Debug.Log("<color=red>Error Loading World Clock. Retrying connection " + connectrionRetries + "</color>");
                        yield return StartCoroutine(IEInitializeDate());
                    }
                    else
                    {
                        isErrorConnect = true;
                        Debug.Log("<color=red>Error Loading World Clock:" + www.error + "</color>");

                        errorMessage = www.error;
                    }
                }
                else
                {
                    var clockJson = www.downloadHandler.text;

                    // Loads the world clock from JSON
                    worldClock = JsonUtility.FromJson<WorldClock>(clockJson);
                    var dateTimeStr = worldClock.currentDateTime;

                    now = DateTime.ParseExact(dateTimeStr, worldClockFMT, CultureInfo.InvariantCulture);
                    // World Clock don't count the seconds. So we pick the seconds from the local machine
                    now = now.AddSeconds(DateTime.Now.Second);

                    var time = string.Format("{0:D4}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

                    Debug.Log("World Clock Time: " + time);
                    isInitialized = true;
                }
            }
            else
            {
                now = DateTime.Now;
                isInitialized = true;
            }
        }

        private void Update()
        {
            if (!isInitialized) return;
            now = now.AddSeconds(Time.unscaledDeltaTime);
        }

        IEnumerator IEWaitForSeconds(float delay, System.Action onComplete)
        {
            yield return new WaitForSeconds(delay);
            if(onComplete != null)
                onComplete();
        }
    }
}
