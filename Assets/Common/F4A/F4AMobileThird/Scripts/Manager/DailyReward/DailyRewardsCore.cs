using System;
using System.Globalization;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace com.F4A.MobileThird
{
	/**
     * Daily Rewards common methods
     **/
	public abstract class DailyRewardsCore<T> : MonoBehaviour where T : Component
	{
		public bool isSingleton = true;                         // Checks if should be used as singleton
		public string worldClockUrl = "http://worldclockapi.com/api/json/est/now";  // The World Clock JSON API
		public string worldClockFMT = "yyyy-MM-dd'T'HH:mmzzz";  // World Clock Format

		public bool useWorldClockApi = true;                    // Use World Clock API
		public WorldClock worldClock;                           // The World Clock object

		public string errorMessage;                             // Error Message
		public bool isErrorConnect;                             // Some error happened on connecting?
		public DateTime now;                                    // The actual date. Either returned by the using the world clock or the player device clock

		public int maxRetries = 3;                              // The maximum number of retries for the World Clock connection
		private int connectrionRetries;                         // Retries counter

		public delegate void OnInitialize(bool error = false, string errorMessage = ""); // When the timer initializes. Sends an error message in case it happens. Should wait for this delegate if using World Clock API
		public static OnInitialize onInitialize;

		public bool isInitialized = false;

		// Initializes the current DateTime. If the player is using the World Clock initializes it
		public IEnumerator InitializeDate()
		{
            if (useWorldClockApi)
			{
#if UNITY_2018_3_OR_NEWER
                UnityWebRequest www = UnityWebRequest.Get(worldClockUrl);
                www.SendWebRequest();
#else
                WWW www = new WWW(worldClockUrl);
#endif

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

				if (!string.IsNullOrEmpty(www.error))
				{
					// Try again to connect
					if (connectrionRetries < maxRetries)
					{
						connectrionRetries++;
						Debug.LogError("Error Loading World Clock. Retrying connection " + connectrionRetries);
						yield return StartCoroutine(InitializeDate());
					}
					else
					{
						isErrorConnect = true;
						Debug.LogError("Error Loading World Clock:" + www.error);

						errorMessage = www.error;
					}
				}
				else
				{
#if UNITY_2018_3_OR_NEWER
                    var clockJson = www.downloadHandler.text;
#else
                    var clockJson = www.text;
#endif

                    // Loads the world clock from JSON
                    worldClock = JsonUtility.FromJson<WorldClock>(clockJson);
					var dateTimeStr = worldClock.currentDateTime;

					now = DateTime.ParseExact(dateTimeStr, worldClockFMT, CultureInfo.InvariantCulture);
					// World Clock don't count the seconds. So we pick the seconds from the local machine
					now = now.AddSeconds(DateTime.Now.Second);

					var time = string.Format("{0:D4}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

					print("World Clock Time: " + time);
					isInitialized = true;
				}
			}
			else
			{
				now = DateTime.Now;

				isInitialized = true;
			}
		}

		private static T _instance;
		public static T instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<T>();
					if (_instance == null)
					{
						GameObject obj = new GameObject();
						obj.hideFlags = HideFlags.HideAndDontSave;
						_instance = obj.AddComponent<T>();
					}
				}

				return _instance;
			}
		}

		protected virtual void Awake()
		{
			if (isSingleton)
			{
				DontDestroyOnLoad(this.gameObject);
			}

			if (_instance == null)
			{
				_instance = this as T;
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
}