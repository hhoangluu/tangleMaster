using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

#if DEFINE_GAMESERVICES
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
#endif

namespace com.F4A.MobileThird
{
    public partial class SocialManager
	{
        public static event Action<bool> OnAuthenticateGameServices = delegate { };
        public static event Action<bool> OnSignOutGameServices = delegate { };

        private bool _isLoginGameServiceSuccess = false;
        public bool IsLoginGameServiceSuccess
        {
            get { return _isLoginGameServiceSuccess; }
            set { _isLoginGameServiceSuccess = value; }
        }

        private bool _isShowWhenLogin = false;

        public void InitGameServices()
        {
#if DEFINE_GAMESERVICES
#if UNITY_ANDROID
            //PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            //// enables saving game progress.
            //.EnableSavedGames()
            //// registers a callback to handle game invitations received while the game is not running.
            //.WithInvitationDelegate(WithInvitationDelegate)
            //// registers a callback for turn based match notifications received while the
            //// game is not running.
            //        .WithMatchDelegate(WithMatchDelegate)
            //// requests the email address of the player be available.
            //// Will bring up a prompt for consent.
            //        .RequestEmail()
            //        // requests a server auth code be generated so it can be passed to an
            //        //  associated back end server application and exchanged for an OAuth token.
            //        .RequestServerAuthCode(false)
            //        // requests an ID token be generated.  This OAuth token can be used to
            //        //  identify the player to other services such as Firebase.
            //        .RequestIdToken()
            //.Build()
            //;
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            //Activate the Google Play Games platform
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
#endif
            if (socialSettingInfo.IsStartGameServicesWhenStart)
            {
                SigninSocialServices();
            }
#endif
        }

        public void SigninSocialServices()
        {
#if DEFINE_GAMESERVICES
            try
            {
                Social.localUser.Authenticate(ProcessAuthentication);
            }
            catch (Exception e)
            {
                Debug.Log("SocialManager.SigninSocialServices " + e.ToString());
            }
#endif
        }

        private void ProcessAuthentication(bool success)
        {
#if DEFINE_GAMESERVICES
            try
            {
                Debug.Log("SocialManager.ProcessAuthentication success: " + success);
                IsLoginGameServiceSuccess = success;
                OnAuthenticateGameServices?.Invoke(success);
#if UNITY_ANDROID
                if (!success)
                {
#if UNITY_ANDROID
                    F4ACoreManager.Instance.Toast("Can't login Google Play Services. Please check again or login again!");
#elif UNITY_IOS
	                F4ACoreManager.Instance.Toast("Can't login Game Center. Please check again or login again!");
#endif
                }
                else if(_isShowWhenLogin)
                {
                    _isShowWhenLogin = false;
                    DOVirtual.DelayedCall(0.2f, () => 
                    {
                        ShowLeaderBoard();
                    });
                }
#endif
            }
            catch (Exception e)
            {
                Debug.Log("SocialManager.ProcessAuthentication e: " + e.ToString());
                IsLoginGameServiceSuccess = false;
                OnAuthenticateGameServices?.Invoke(false);
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void ShowLeaderBoard(string id, bool isShowWhenLogin = true)
        {
#if DEFINE_GAMESERVICES
#if UNITY_ANDROID
            if (Social.Active != null && ((PlayGamesPlatform)Social.Active) != null && 
                ((PlayGamesPlatform)Social.Active).IsAuthenticated() && IsLoginGameServiceSuccess)
            {
	            ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(id);
            }

#elif UNITY_IOS
	        id = id.Replace('-', '_');
	        if(Social.Active != null && Social.localUser.authenticated 
                && IsLoginGameServiceSuccess)
	        {
	        	Social.Active.ShowLeaderboardUI();
	        }
#endif
            else
            {
                _isShowWhenLogin = isShowWhenLogin;
                SigninSocialServices();
            }
#endif
        }

        /// <summary>
        /// Shows All Available Leaderborad
        /// </summary>
        public void ShowLeaderBoard(bool isShowWhenLogin = true)
        {
#if DEFINE_GAMESERVICES
#if UNITY_ANDROID
            if (Social.Active != null && ((PlayGamesPlatform)Social.Active) != null
                && ((PlayGamesPlatform)Social.Active).IsAuthenticated() && IsLoginGameServiceSuccess)
            {
                ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI();
            }
#elif UNITY_IOS
			if(Social.Active != null && Social.localUser.authenticated 
                && IsLoginGameServiceSuccess)
			{
				Social.Active.ShowLeaderboardUI();
			}
#endif
            else
            {
                _isShowWhenLogin = isShowWhenLogin;
                SigninSocialServices();
            }
#endif
        }

        public void SetLeaderBoard(string id, long value)
		{
			SetLeaderBoard(id, value, null);
        }
        
		public void SetLeaderBoard(string id, long value, System.Action<bool> OnSetLeaderBoardCompleted)
		{
#if DEFINE_GAMESERVICES
            EventsManager.Instance.LogEvent("SetLeaderBoard", new Dictionary<string, object>
            {
                { "id", id },
                { "value", value}
            });

#if UNITY_IOS
			id = id.Replace('-', '_');
#endif
			if (Social.localUser.authenticated)
			{
				Social.ReportScore(value, id, (bool success) =>
				{
					OnSetLeaderBoardCompleted?.Invoke(success);
				});
			}
#endif
        }

        /// <summary>
        /// On Logout of your Google+ Account
        /// </summary>
        public void LogOutServices()
        {
#if DEFINE_GAMESERVICES
#if UNITY_ANDROID
            ((PlayGamesPlatform)Social.Active).SignOut();
            OnSignOutGameServices?.Invoke(true);
#elif UNITY_IOS
#endif
#endif
        }

        public void UnlockAchievement(string id)
        {
#if DEFINE_GAMESERVICES
#if UNITY_IOS
	        id = id.Replace('-', '_');
#endif
            Social.ReportProgress(id, 100, success => { });
#endif
        }

        public void IncrementAchievement(string id, int stepsToIncrement)
        {
#if DEFINE_GAMESERVICES
#if UNITY_ANDROID
	        PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success => { });
#elif UNITY_IOS
	        id = id.Replace('-', '_');
#endif
#endif
        }

        public void ShowAchievementsUI()
        {
#if DEFINE_GAMESERVICES
#if UNITY_ANDROID
            if (Social.Active != null && ((PlayGamesPlatform)Social.Active) != null && ((PlayGamesPlatform)Social.Active).IsAuthenticated() && IsLoginGameServiceSuccess)
            {
                ((PlayGamesPlatform)Social.Active).ShowAchievementsUI();
            }
            else
            {
                SigninSocialServices();
            }
#elif UNITY_IOS
            if (Social.Active != null && IsLoginGameServiceSuccess)
            {
                Social.ShowAchievementsUI();
            }
            else
            {
                SigninSocialServices();
            }
#endif
#endif
        }

        /// <summary>
        /// Checks the unlock achievement.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="valueCurrent">Level current.</param>
        /// <param name="valueUnlock">Level unlock.</param>
        public void CheckUnlockAchievement(string id, int valueCurrent, int valueUnlock)
        {
            if(!CPlayerPrefs.GetBool(id, false) && valueCurrent >= valueUnlock)
            {
                CPlayerPrefs.SetBool(id, true);
                UnlockAchievement(id);
            }
        }
    }
}
