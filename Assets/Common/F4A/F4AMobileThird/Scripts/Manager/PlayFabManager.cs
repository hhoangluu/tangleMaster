using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if DEFINE_FACEBOOK_SDK
using Facebook.Unity;
#endif

#if PLAY_FAB
using PlayFab;
using PlayFab.ClientModels;
using LoginResult = PlayFab.ClientModels.LoginResult;
#endif

using System;

namespace com.F4A.MobileThird
{
	[AddComponentMenu ("F4A/PlayFabManager")]
	public class PlayFabManager : ManualSingletonMono<PlayFabManager>
	{
		public bool EnablePlayFab = false;
#if PLAY_FAB
		private string PlayfabId = "";
#endif
		protected override void Initialization ()
		{
			
		}

		public void LoginWithFacebook ()
		{
			#if PLAY_FAB && DEFINE_FACEBOOK_SDK
			Debug.Log("PlayFabManager LoginWithFacebook");;
			PlayFabClientAPI.LoginWithFacebook (new LoginWithFacebookRequest {
				CreateAccount = true,
				AccessToken = AccessToken.CurrentAccessToken.TokenString,
				TitleId = PlayFabSettings.TitleId,
			},
				LoginFacebookResultCallback, LoginFacebookErrorCallback);
			#endif
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenString"></param>
        public void LoginWithFacebook(string tokenString)
        {
#if PLAY_FAB
			Debug.Log("PlayFabManager LoginWithFacebook(string tokenString)");;
			PlayFabClientAPI.LoginWithFacebook (new LoginWithFacebookRequest {
				CreateAccount = true,
				AccessToken = tokenString,
				TitleId = PlayFabSettings.TitleId,
			},
				LoginFacebookResultCallback, LoginFacebookErrorCallback);
#endif
        }

#if PLAY_FAB
		/// <summary>
		/// The on login facebook result.
		/// param NewlyCreated
		/// </summary>
		public Action<LoginResult> OnLoginFacebookResult;
		public Action<PlayFabError> OnLoginFacebookError;

		/// <summary>
		/// Logins the facebook result callback.
		/// </summary>
		/// <param name="result">Result.</param>
		// When processing both results, we just set the message, explaining what's going on.
		private void LoginFacebookResultCallback (LoginResult result)
		{
#if PLAY_FAB
			this.PlayfabId = result.PlayFabId;
#endif
			if (OnLoginFacebookResult != null) {
				Debug.Log ("LoginFacebookResultCallback");
				OnLoginFacebookResult (result);
			}
		}

		private void LoginFacebookErrorCallback (PlayFabError result)
		{
			if(OnLoginFacebookError != null)
			{
				Debug.Log ("LoginFacebookResultCallback");
				OnLoginFacebookError(result);
			}
		}
#endif


        /// <summary>
        /// Sets the user data.
        /// </summary>
        /// <param name="data">Data.</param>
        public void SetUserData (Dictionary<string, string> data)
		{
			#if PLAY_FAB
			if(string.IsNullOrEmpty(PlayfabId)){
				return;
			}
			UpdateUserDataRequest request = new UpdateUserDataRequest()
			{
				Data = data
			};

			PlayFabClientAPI.UpdateUserData(request, (result) =>
				{
				}, (error) =>
				{
				});
			#endif
		}

		#if PLAY_FAB
		public Action<GetUserDataResult> OnGetUserDataResult;
		public Action<PlayFabError> OnGetUserDataError;
		#endif

		/// <summary>
		/// Gets the user data.
		/// </summary>
		public void GetUserData ()
		{
			#if PLAY_FAB
			if (string.IsNullOrEmpty (this.PlayfabId)) {
				return;
			}
			GetUserDataRequest request = new GetUserDataRequest () {
				PlayFabId = this.PlayfabId,
				Keys = null
			};

			PlayFabClientAPI.GetUserData (request, (result) => {
				if(OnGetUserDataResult != null){
					OnGetUserDataResult(result);
				}
			}, (PlayFabError error) => {
				
			});
			#endif
		}
	}
}