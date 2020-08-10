using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.UI;

#if DEFINE_FACEBOOK_SDK
using Facebook.Unity;
#endif

namespace com.F4A.MobileThird
{
    public partial class SocialManager
    {
        public static event Action OnLoginFacebookSuccessed = delegate { };
        public static event Action OnLoginFacebookFailed = delegate { };
        public static event Action OnLogoutFacebookCompleted = delegate { };

        //public static event Action OnFetchProfile = delegate { };

        public static event Action OnShareFacebookComplete = delegate { };
        public static event Action OnShareFacebookError = delegate { };

        public static event Action OnInitFacebookCompleted = delegate { };

        public static event Action OnGetInfoFacebookUserCompleted = delegate { };

        public static event Action OnGetPictureFacebook = delegate { };

#if DEFINE_FACEBOOK_SDK
        public static event Action<IAppRequestResult> OnAppRequest = delegate { };
        public static event Action<IGraphResult> OnGetPlayerInfo = delegate { };
        public static event Action<IGraphResult> OnGetFriends = delegate { };
        public static event Action<IGraphResult> OnGetInvitableFriends = delegate { };
        public static event Action<IGraphResult> OnLoadImageWithFbId = delegate { };
        public static event Action<IGraphResult> OnGetScoresCallback = delegate { };
#endif

#pragma warning disable 219
        private string strQueryFields = "";
        public Dictionary<string, object> _dicFacebookUserDetails = new Dictionary<string, object>();
        private Texture2D _fbAvatarTexture = null;

        [HideInInspector]
        public string facebookUserId = "";

        #region FACEBOOK Methods

        public bool EnableFacebook()
        {
#if DEFINE_FACEBOOK_SDK
            return true;
#else
            return false;
#endif
        }

        public bool IsInitFacebook()
        {
#if DEFINE_FACEBOOK_SDK
            return FB.IsInitialized;
#else
            return false;
#endif
        }

        public bool IsLoginFacebook()
        {
#if DEFINE_FACEBOOK_SDK
            return FB.IsLoggedIn;
#else
            return false;
#endif
        }

        #region Methods Support
        public void LoadInfoUserFacebook(Text facebookNameText)
        {
            if (facebookNameText) facebookNameText.text = GetFieldFacebook(EFieldQueryType.Name).ToString();
        }

        public void LoadAvatarFacebook(Image facebookAvatar)
        {
            if (facebookAvatar && _fbAvatarTexture)
            {
                facebookAvatar.sprite = Sprite.Create(_fbAvatarTexture, new Rect(0, 0, socialSettingInfo.widthAvatarFacebook, socialSettingInfo.heightAvatarFacebook), new Vector2());
            }
        }
        #endregion

#if DEFINE_FACEBOOK_SDK
        public AccessToken GetFacebookAccessToken()
        {
            return AccessToken.CurrentAccessToken;
        }
#endif

        public bool HasFacebookAccessTokenString()
        {
#if DEFINE_FACEBOOK_SDK
            return FB.IsInitialized && FB.IsLoggedIn && AccessToken.CurrentAccessToken != null
                && !string.IsNullOrEmpty(AccessToken.CurrentAccessToken.TokenString);
#else
            return false;
#endif
        }

        public string GetFacebookAccessTokenString()
        {
#if DEFINE_FACEBOOK_SDK
            if (HasFacebookAccessTokenString()) return AccessToken.CurrentAccessToken.TokenString;
#endif
            return string.Empty;
        }

        private void InitFacebook()
        {
#if DEFINE_FACEBOOK_SDK
            //IsLoginFacebook = false;
            strQueryFields = "";
            int len = socialSettingInfo.facebookQueryFields.Length;
            for (int i = 0; i < len; i++)
            {
                if (i != len - 1)
                    strQueryFields += socialSettingInfo.facebookQueryFields[i] + ",";
                else
                    strQueryFields += socialSettingInfo.facebookQueryFields[i];
            }
            if (string.IsNullOrEmpty(strQueryFields))
            {
                strQueryFields = "id,email, name,first_name,last_name,picture.width(120).height(120)";
            }
            Debug.Log("strQueryFields: " + strQueryFields);
#if UNITY_ANDROID || UNITY_IOS
            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(HandleInitFacebookCompleted, HandleHideUnity);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }
#endif
        }

        private void HandleInitFacebookCompleted()
        {
            if (FB.IsInitialized)
            {
                // Signal an app activation App Event
                FB.ActivateApp();
                // Continue with Facebook SDK
                // ...
                OnInitFacebookCompleted?.Invoke();
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isGameShown"></param>
        private void HandleHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }

        /// <summary>
        /// Logins the Facebook.
        /// </summary>
        /// <param name="isGetInfo">If set to <c>true</c> is get info.</param>
        public void LoginFacebook()
        {
            //LoginFacebook(new List<string>() { "public_profile", "email", "user_friends" });
            LoginFacebook(socialSettingInfo.facebookPermisions.ToList());
        }

        /// <summary>
        /// Logins the Facebook.
        /// </summary>
        /// <param name="permissions">Permissions.</param>
        /// <param name="isGetInfo">If set to <c>true</c> is get info.</param>
        public void LoginFacebook(List<string> permissions)
        {
#if DEFINE_FACEBOOK_SDK
	        FB.LogInWithReadPermissions(permissions, this.HandleLoginFacebookCallback);
#endif
        }


#if DEFINE_FACEBOOK_SDK
        /// <summary>
        /// Handles the result.
        /// </summary>
        /// <param name="result">Result.</param>
        protected void HandleLoginFacebookCallback(IResult result)
        {
            if (result == null)
            {
                if (OnLoginFacebookFailed != null)
                {
                    OnLoginFacebookFailed();
                }
                return;
            }

            // Some platforms return the empty string instead of null.
			if (!string.IsNullOrEmpty(result.Error) || result.Cancelled || string.IsNullOrEmpty(result.RawResult))
            {
				if (OnLoginFacebookFailed != null)
                {
                    OnLoginFacebookFailed();
                }
            }
            else if (!string.IsNullOrEmpty(result.RawResult))
            {
                if (IsLoginFacebook())
                {
                    AccessToken aToken = AccessToken.CurrentAccessToken;
                    if(aToken != null) facebookUserId = aToken.UserId;
                }
                OnLoginFacebookSuccessed?.Invoke();
                GetInfoFacebookUser ();
            }
            else
            {
                OnLoginFacebookFailed();
            }
        }

        private void GetInfoFacebookUser_CallBack(IGraphResult result)
        {
            _dicFacebookUserDetails = (Dictionary<string, object>)result.ResultDictionary;
            OnGetInfoFacebookUserCompleted?.Invoke();
            GetPlayerPicture();
        }
#endif

        public void GetInfoFacebookUser()
        {
#if DEFINE_FACEBOOK_SDK
            FB.API("/me?fields=" + strQueryFields, HttpMethod.GET, GetInfoFacebookUser_CallBack, new Dictionary<string, string>() { });
#endif
        }

        //        public void Share(int score)
        //        {
        //#if DEFINE_FACEBOOK_SDK
        //            if (!FB.IsLoggedIn)
        //            {
        //                MobileThirdManager.Instance.Log("not logged, logging");
        //            }
        //            else
        //            {
        //                FB.FeedShare(
        //                    link: new Uri("http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? AccessToken.CurrentAccessToken.UserId : "guest")),
        //                    linkCaption: "I just scored " + score + " points! Try to beat me!"
        //                //picture: "https://fbexternal-a.akamaihd.net/safe_image.php?d=AQCzlvjob906zmGv&w=128&h=128&url=https%3A%2F%2Ffbcdn-photos-h-a.akamaihd.net%2Fhphotos-ak-xtp1%2Ft39.2081-0%2F11891368_513258735497916_1832270581_n.png&cfs=1"
        //                );
        //            }
        //#endif
        //        }

        public void ShareFacebookLink()
        {
#if DEFINE_FACEBOOK_SDK
            string linkStore = socialSettingInfo.GetUrlGameOnStore();
            string nameGame = BuildManager.Instance.GetGameName();
            string description = "Let's me play this " + nameGame + "!";
            FB.ShareLink(new Uri(linkStore), nameGame, description, new System.Uri(socialSettingInfo.linkImageShareFB), ShareFacebookLink_Callback);
#endif
        }

#if DEFINE_FACEBOOK_SDK
	    protected void ShareFacebookLink_Callback(IShareResult result)
	    {
		    if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
		    {
                OnShareFacebookComplete?.Invoke();
		    }
	    }
#endif

        public void ShareFacebook()
        {
#if DEFINE_FACEBOOK_SDK
            string linkStore = socialSettingInfo.GetUrlGameOnStore();
            string nameGame = BuildManager.Instance.GetGameName();
            FB.FeedShare(
                "",
                new System.Uri(linkStore),
                nameGame,
                "Share game " + nameGame,
                "Let's me play this " + nameGame + "!",
	            new System.Uri(socialSettingInfo.linkImageShareFB),
                "",
                CallBackShare);
#endif
        }

        public void ShareFacebook(int score)
        {
            string linkStore = socialSettingInfo.GetUrlGameOnStore();
            string nameGame = BuildManager.Instance.GetGameName();
#if DEFINE_FACEBOOK_SDK
            FB.FeedShare(
                "",
                new System.Uri(linkStore),
                nameGame,
                "Share game " + nameGame,
                "Join with me and play " + nameGame + ". My score is " + score + "! How about you?",
                new System.Uri(socialSettingInfo.linkImageShareFB),
                "",
                CallBackShare);
#endif
        }

        public void ShareFacebook(string linkDescription)
        {
            string linkStore = socialSettingInfo.GetUrlGameOnStore();
            string nameGame = BuildManager.Instance.GetGameName();
#if DEFINE_FACEBOOK_SDK
            FB.FeedShare(
                "",
                new System.Uri(linkStore),
                nameGame,
                "Share game " + nameGame,
                "Join with me and play " + nameGame + ". " + linkDescription,
                new System.Uri(socialSettingInfo.linkImageShareFB),
                "",
                CallBackShare);
#endif
        }

#if DEFINE_FACEBOOK_SDK
        private void CallBackShare(IShareResult result)
        {
            if ("".Equals(result) || result.Cancelled)
            {
                if (OnShareFacebookError != null)
                    OnShareFacebookError();
            }
            else
            {
                if (OnShareFacebookComplete != null)
                    OnShareFacebookComplete();
            }
        }
#endif

#region App Request
//        /// <summary>
//        /// Prompt the player to send a Game Request to their friends with Friend Smash!
//        /// </summary>
//        public void AppRequest()
//        {
//#if DEFINE_FACEBOOK_SDK
//            List<string> recipient = null;
//            string title, message, data = string.Empty;

//            // Check to see if we have played a game against a friend yet during this session
//            //if (GameStateManager.Score != 0 && GameStateManager.FriendID != null)
//            //{
//            //    // We have played a game -- lets send a request to the person we just smashed
//            //    title = "Friend Smash Challenge!";
//            //    message = "I just smashed you " + GameStateManager.Score.ToString() + " times! Can you beat it?";
//            //    recipient = new List<string>() { GameStateManager.FriendID };
//            //    data = "{\"challenge_score\":" + GameStateManager.Score.ToString() + "}";
//            //}
//            //else
//            //{
//            //    // We have not played a game against a friend -- lets open an invite request
//            //    title = "Play Friend Smash with me!";
//            //    message = "Friend Smash is smashing! Check it out.";
//            //}

//            title = "Play " + BuildManager.Instance.GetGameName() + " with me!";
//            message = BuildManager.Instance.GetGameName() + " is smashing! Check it out.";

//            // Prompt user to send a Game Request using FB.AppRequest
//            // https://developers.facebook.com/docs/unity/reference/current/FB.AppRequest
//            FB.AppRequest(
//                message,
//                recipient,
//                null,
//                null,
//                null,
//                data,
//                title,
//                HandleAppRequestCallback);
//#endif
//        }

        public void AppRequest(List<string> listSend)
        {
#if DEFINE_FACEBOOK_SDK
            List<string> recipient = new List<string>();
            string title, message, data = string.Empty;
            title = "Match 3 Game!";
            message = "Best game is here. Check it out!";
            recipient = new List<string>();
            recipient = listSend;
            FB.AppRequest(
                message,
                recipient,
                null,
                null,
                null,
                data,
                title,
                HandleAppRequestCallback
            );
#endif
        }

#if DEFINE_FACEBOOK_SDK
        private static void HandleAppRequestCallback(IAppRequestResult result)
        {
            if(OnAppRequest != null)
            {
                OnAppRequest(result);
            }

            //// Error checking
            //Debug.Log("AppRequestCallback");
            //if (result.Error != null)
            //{
            //    Debug.LogError(result.Error);
            //    return;
            //}
            ////Debug.Log(result.RawResult);

            //// Check response for success - show user a success popup if so
            //object obj;
            //if (result.ResultDictionary.TryGetValue("cancel", out obj))
            //{
            //    Debug.Log("Request cancel");
            //}
            //else if (result.ResultDictionary.TryGetValue("request", out obj))
            //{
            //    //            PopupScript.SetPopup("Request Sent", 3f);
            //    Debug.Log("Request sent");
            //}
        }
#endif
#endregion


#region PlayerInfo
        // Once a player successfully logs in, we can welcome them by showing their name
        // and profile picture on the home screen of the game. This information is returned
        // via the /me/ endpoint for the current player. We'll call this endpoint via the
        // SDK and use the results to personalize the home screen.
        //
        // Make a Graph API GET call to /me/ to retrieve a player's information
        // See: https://developers.facebook.com/docs/graph-api/reference/user/
        public void GetPlayerInfo()
        {
#if DEFINE_FACEBOOK_SDK
            string queryString = "/me?fields=id,first_name,picture.width(120).height(120)";
            FB.API(queryString, HttpMethod.GET, HandleGetPlayerInfoCallback);
#endif
        }

        // In the above request it takes two network calls to fetch the player's profile picture.
        // If we ONLY needed the player's profile picture, we can accomplish this in one call with the /me/picture endpoint.
        //
        // Make a Graph API GET call to /me/picture to retrieve a players profile picture in one call
        // See: https://developers.facebook.com/docs/graph-api/reference/user/picture/
        public void GetPlayerPicture()
        {
#if DEFINE_FACEBOOK_SDK
            FB.API(GraphUtil.GetPictureQuery("me", socialSettingInfo.widthAvatarFacebook, socialSettingInfo.heightAvatarFacebook), HttpMethod.GET, HandleGetPlayerPicture);
#endif
        }

#if DEFINE_FACEBOOK_SDK
        //public void GetPlayerInfo(FacebookDelegate<IGraphResult> callback)
        //{
        //    string queryString = "/me?fields=id,first_name,picture.width(120).height(120)";
        //    FB.API(queryString, HttpMethod.GET, callback);
        //}

        private void HandleGetPlayerInfoCallback(IGraphResult result)
        {
            if(OnGetPlayerInfo != null)
            {
                OnGetPlayerInfo(result);
            }

            //Debug.Log("GetPlayerInfoCallback");
            //if (result.Error != null)
            //{
            //    Debug.LogError(result.Error);
            //    return;
            //}
            ////Debug.Log(result.RawResult);

            //// Save player name
            //string name;
            //if (result.ResultDictionary.TryGetValue("first_name", out name))
            //{
            //    //            GameStateManager.Username = name;
            //}

            ////Fetch player profile picture from the URL returned
            //string playerImgUrl = GraphUtil.DeserializePictureURL(result.ResultDictionary);
            //GraphUtil.LoadImgFromURL(playerImgUrl, delegate (Texture pictureTexture)
            //{
            //    // Setup the User's profile picture
            //    if (pictureTexture != null)
            //    {
            //        //                GameStateManager.UserTexture = pictureTexture;
            //    }

            //    // Redraw the UI
            //    //            GameStateManager.CallUIRedraw();
            //});
        }

        private void HandleGetPlayerPicture(IGraphResult result)
        {
            if (string.IsNullOrEmpty(result.Error) && result.Texture != null)
            {
                _fbAvatarTexture = result.Texture;
                OnGetPictureFacebook?.Invoke();
            }

            //Debug.Log("PlayerPictureCallback");
            //if (result.Error != null)
            //{
            //    Debug.LogError(result.Error);
            //    return;
            //}
            //if (result.Texture == null)
            //{
            //    Debug.Log("PlayerPictureCallback: No Texture returned");
            //    return;
            //}

            ////            // Setup the User's profile picture
            ////            GameStateManager.UserTexture = result.Texture;
            ////            
            ////            // Redraw the UI
            ////            GameStateManager.CallUIRedraw();
        }
#endif
#endregion

#region Friends
        // We can fetch information about a player's friends via the Graph API user edge /me/friends
        // This endpoint returns an array of friends who are also playing the same game.
        // See: https://developers.facebook.com/docs/graph-api/reference/user/friends
        //
        // We can use this data to provide a set of real people to play against, showing names
        // and pictures of the player's friends to make the experience feel even more personal.
        //
        // The /me/friends edge requires an additional permission, user_friends. Without
        // this permission, the response from the endpoint will be empty. If we know the user has
        // granted the user_friends permission but we see an empty list of friends returned, then
        // we know that the user has no friends currently playing the game.
        //
        // Note:
        // In this instance we are making two calls, one to fetch the player's friends who are already playing the game
        // and another to fetch invite able friends who are not yet playing the game. It can be more performant to batch 
        // Graph API calls together as Facebook will parallelize independent operations and return one combined result.
        // See more: https://developers.facebook.com/docs/graph-api/making-multiple-requests
        //
        public void GetFriends()
        {
#if DEFINE_FACEBOOK_SDK
            string queryString = "/me/friends?fields=id,first_name,picture.width(128).height(128)&limit=100";
            FB.API(queryString, HttpMethod.GET, HandleGetFriendsCallback);
#endif
        }

        // We can fetch information about a player's friends who are not yet playing our game
        // via the Graph API user edge /me/invitable_friends
        // See more about Invite able Friends here: https://developers.facebook.com/docs/games/invitable-friends
        //
        // The /me/invitable_friends edge requires an additional permission, user_friends.
        // Without this permission, the response from the endpoint will be empty.
        //
        // Edge: https://developers.facebook.com/docs/graph-api/reference/user/invitable_friends
        // Nodes returned are of the type: https://developers.facebook.com/docs/graph-api/reference/user-invitable-friend/
        // These nodes have the following fields: profile picture, name, and ID. The ID's returned in the Invite able Friends
        // response are not Facebook IDs, but rather an invite tokens that can be used in a custom Game Request dialog.
        //
        // Note! This is different from the following Graph API:
        // https://developers.facebook.com/docs/graph-api/reference/user/friends
        // Which returns the following nodes:
        // https://developers.facebook.com/docs/graph-api/reference/user/
        //
        public void GetInvitableFriends()
        {
#if DEFINE_FACEBOOK_SDK
            string queryString = "/me/invitable_friends?fields=id,first_name,picture.width(128).height(128)&limit=25";
            FB.API(queryString, HttpMethod.GET, HandleGetInvitableFriendsCallback);
#endif
        }

#if DEFINE_FACEBOOK_SDK
        private void HandleGetFriendsCallback(IGraphResult result)
        {
            if(OnGetFriends != null)
            {
                OnGetFriends(result);
            }

            //Debug.Log("GetFriendsCallback");
            //if (result.Error != null)
            //{
            //    Debug.LogError(result.Error);
            //    return;
            //}
            //Debug.Log(result.RawResult);

            //// Store /me/friends result
            //object dataList;
            //if (result.ResultDictionary.TryGetValue("data", out dataList))
            //{
            //    var friendsList = (List<object>)dataList;
            //    CacheFriends(friendsList);
            //}
        }

        private void HandleGetInvitableFriendsCallback(IGraphResult result)
        {
            if (OnGetInvitableFriends != null)
            {
                OnGetInvitableFriends(result);
            }

            //Debug.Log("GetInvitableFriendsCallback");
            //if (result.Error != null)
            //{
            //    Debug.LogError(result.Error);
            //    return;
            //}
            //Debug.Log(result.RawResult);

            //// Store /me/invitable_friends result
            //object dataList;
            //if (result.ResultDictionary.TryGetValue("data", out dataList))
            //{
            //    var invitableFriendsList = (List<object>)dataList;
            //    foreach (object Friend in invitableFriendsList)
            //    {
            //        var entry = (Dictionary<string, object>)Friend;
            //        var user = (Dictionary<string, object>)entry["first_name"];
            //        string userId = (string)user["id"];
            //    }
            //}
        }
#endif
#endregion

#region Scores
        // Fetch leaderboard scores from Scores API
        // Scores API documentation: https://developers.facebook.com/docs/games/scores
        //
        // With player scores being written to the Graph API, we now have a data set on
        // which to build a social leader board. By calling the /app/scores endpoint for
        // your app, with a user access token, you get back a list of the current player's
        // friends' scores, ordered by score.
        //
        public void GetScores()
        {
#if DEFINE_FACEBOOK_SDK
            FB.API("/app/scores?fields=score,user.limit(20)", HttpMethod.GET, HandleGetScoresCallback);
#endif
        }

#if DEFINE_FACEBOOK_SDK
        private void HandleGetScoresCallback(IGraphResult result)
        {
            if(OnGetScoresCallback != null)
            {
                OnGetScoresCallback(result);
            }

            //Debug.Log("GetScoresCallback");
            //if (result.Error != null)
            //{
            //    Debug.LogError(result.Error);
            //    return;
            //}
            //Debug.Log(result.RawResult);

            //// Parse scores info
            //var scoresList = new List<object>();

            //object scoresh;
            //if (result.ResultDictionary.TryGetValue("data", out scoresh))
            //{
            //    scoresList = (List<object>)scoresh;
            //}

            //// Parse score data
            //HandleScoresData(scoresList);

            //// Redraw the UI
            ////        GameStateManager.CallUIRedraw();
        }
#endif

        //private static void HandleScoresData(List<object> scoresResponse)
        //{
        //    //        var structuredScores = new List<object>();
        //    //        foreach(object scoreItem in scoresResponse) 
        //    //        {
        //    //            // Score JSON format
        //    //            // {
        //    //            //   "score": 4,
        //    //            //   "user": {
        //    //            //      "name": "Chris Lewis",
        //    //            //      "id": "10152646005463795"
        //    //            //   }
        //    //            // }
        //    //
        //    //            var entry = (Dictionary<string,object>) scoreItem;
        //    //            var user = (Dictionary<string,object>) entry["user"];
        //    //            string userId = (string)user["id"];
        //    //            
        //    //            if (string.Equals(userId, AccessToken.CurrentAccessToken.UserId))
        //    //            {
        //    //                // This entry is the current player
        //    //                int playerHighScore = GraphUtil.GetScoreFromEntry(entry);
        //    //                Debug.Log("Local players score on server is " + playerHighScore);
        //    //                if (playerHighScore < GameStateManager.Score)
        //    //                {
        //    //                    Debug.Log("Locally overriding with just acquired score: " + GameStateManager.Score);
        //    //                    playerHighScore = GameStateManager.Score;
        //    //                }
        //    //                
        //    //                entry["score"] = playerHighScore.ToString();
        //    //                GameStateManager.HighScore = playerHighScore;
        //    //            }
        //    //            
        //    //            structuredScores.Add(entry);
        //    //            if (!GameStateManager.FriendImages.ContainsKey(userId))
        //    //            {
        //    //                // We don't have this players image yet, request it now
        //    //                LoadFriendImgFromID (userId, pictureTexture =>
        //    //                {
        //    //                    if (pictureTexture != null)
        //    //                    {
        //    //                        GameStateManager.FriendImages.Add(userId, pictureTexture);
        //    //                        GameStateManager.CallUIRedraw();
        //    //                    }
        //    //                });
        //    //            }
        //    //        }
        //    //
        //    //        GameStateManager.Scores = structuredScores;
        //}

        // Graph API call to fetch friend picture from user ID returned from FBGraph.GetScores()
        //
        // Note: /me/invitable_friends returns invite tokens instead of user ID's,
        // which will NOT work with this /{user-id}/picture Graph API call.
        private void LoadFriendImgFromID(string userID, Action<Texture> callback)
        {
#if DEFINE_FACEBOOK_SDK
            FB.API(GraphUtil.GetPictureQuery(userID, 128, 128),
                   HttpMethod.GET,
                   delegate (IGraphResult result)
                   {
                       if (result.Error != null)
                       {
                           Debug.LogError(result.Error + ": for friend " + userID);
                           return;
                       }
                       if (result.Texture == null)
                       {
                           Debug.Log("LoadFriendImg: No Texture returned");
                           return;
                       }
                       callback(result.Texture);
                   });
#endif
        }
#endregion

#region Load Image
        public void LoadImageWithFbId(string facebookId)
        {
#if DEFINE_FACEBOOK_SDK
            FB.API(GraphUtil.GetPictureQuery(facebookId, 32, 32), HttpMethod.GET, HandleLoadImageWithFbId);

            //Debug.Log ("userID: " + userID);

            //FB.API(GraphUtil.GetPictureQuery(userID, 32, 32),
            //       HttpMethod.GET,
            //       delegate (IGraphResult result)
            //       {
            //           if (result.Error != null)
            //           {
            //               Debug.Log("error");
            //               return;
            //           }
            //           if (result.Texture == null)
            //           {
            //               Debug.Log("null");
            //               return;
            //           }
            //           if (allHighScore[i] != null)
            //           {
            //               allHighScore[i].transform.GetChild(1).GetComponentInChildren<RawImage>().texture = result.Texture;
            //           }
            //           count++;
            //           //Debug.Log ("count: " + count);
            //       });
#endif
        }

#if DEFINE_FACEBOOK_SDK
        private void HandleLoadImageWithFbId(IGraphResult result)
        {
            if(OnLoadImageWithFbId != null)
            {
                OnLoadImageWithFbId(result);
            }
        }
#endif

        //void GetITexture(IGraphResult result)
        //{
        //    if (result.Error != null)
        //    {
        //        Debug.Log("error");
        //        return;
        //    }
        //    if (result.Texture == null)
        //    {
        //        Debug.Log("null");
        //        return;
        //    }

        //    //allHighScore[count].transform.GetChild(2).GetComponent<RawImage>().texture = result.Texture;
        //    leaderObjctr[count].icon.texture = result.Texture;
        //    count++;
        //    //Debug.Log ("count: " + count);
        //}
#endregion

        public void LogoutFB()
        {
#if DEFINE_FACEBOOK_SDK
            FB.LogOut();
#endif
        }

        public object GetFieldFacebook(EFieldQueryType type)
        {
            switch (type)
            {
                case EFieldQueryType.Id:
                    if (_dicFacebookUserDetails.ContainsKey("id"))
                        return _dicFacebookUserDetails["id"].ToString();
                    return "";
                case EFieldQueryType.Name:
                    if (_dicFacebookUserDetails.ContainsKey("name"))
                        return _dicFacebookUserDetails["name"].ToString();
                    return "";
                case EFieldQueryType.LastName:
                    if (_dicFacebookUserDetails.ContainsKey("last_name"))
                        return _dicFacebookUserDetails["last_name"].ToString();
                    return "";
                case EFieldQueryType.FirstName:
                    if (_dicFacebookUserDetails.ContainsKey("first_name"))
                        return _dicFacebookUserDetails["first_name"].ToString();
                    return "";
                case EFieldQueryType.Email:
                    if (_dicFacebookUserDetails.ContainsKey("email"))
                        return _dicFacebookUserDetails["email"].ToString();
                    return "";
                case EFieldQueryType.Link:
                    if (_dicFacebookUserDetails.ContainsKey("link"))
                        return _dicFacebookUserDetails["link"].ToString();
                    return "";
                case EFieldQueryType.Gender:
                    if (_dicFacebookUserDetails.ContainsKey("gender"))
                        return _dicFacebookUserDetails["gender"].ToString();
                    return "";
                case EFieldQueryType.Picture:
                    if (_dicFacebookUserDetails.ContainsKey("picture"))
                    {
                        return _dicFacebookUserDetails["picture"];
                    }
                    return null;
                default:
                    return "";
            }
        }

#endregion

#region Take screenshot

	    public void TakeScreenshot(){
	    	StartCoroutine(AsynTakeScreenshot());
	    }
	    
	    private IEnumerator AsynTakeScreenshot()
	    {
		    yield return new WaitForEndOfFrame();
#if DEFINE_FACEBOOK_SDK
		    var width = Screen.width;
		    var height = Screen.height;
		    var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		    // Read screen contents into the texture
		    tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		    tex.Apply();
		    byte[] screenshot = tex.EncodeToPNG();

		    var wwwForm = new WWWForm();
		    wwwForm.AddBinaryData("image", screenshot, "InteractiveConsole.png");
            wwwForm.AddField("message", "I did a thing!  Did I do this right?");

		    FB.API("me/photos", HttpMethod.POST, HandleTakePhotoResult, wwwForm);
#endif
	    }

#if DEFINE_FACEBOOK_SDK
	    protected void HandleTakePhotoResult(IResult result)
	    {
		    if (result == null)
		    {
			    return;
		    }

		    // Some platforms return the empty string instead of null.
		    if (!string.IsNullOrEmpty(result.Error))
		    {
			    Debug.Log("Error Response:\n" + result.Error);
		    }
		    else if (result.Cancelled)
		    {
			    Debug.Log("Cancelled Response:\n" + result.RawResult);
		    }
		    else if (!string.IsNullOrEmpty(result.RawResult))
		    {
			    Debug.Log("Success Response:\n" + result.RawResult);
		    }
		    else
		    {
			    Debug.Log("Empty Response\n");
		    }
	    }
#endif
#endregion


#region Share Native
        private bool _isProcessingShareNative = false;

        public void ShareNative(bool isShareScreen)
        {
            ShareNative("", "", "", "", isShareScreen);
        }

        public void ShareNative()
        {
            ShareNative("", "", "", "");
        }

        public void ShareNative(string idAppShare)
        {
            ShareNative("", "", "", idAppShare);
        }

        public void ShareNative(string filePath, string msg, string title, string idAppShare, bool isShareScreen = true)
        {
            if (!_isProcessingShareNative)
            {
                StartCoroutine(IEShareScreenshot(filePath, msg, title, idAppShare, isShareScreen));
            }
        }

        public IEnumerator IECaptureScreen(System.Action<string> onCompleted)
        {
            yield return new WaitForEndOfFrame();

            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();

            string filePath = Path.Combine(Application.temporaryCachePath, "CaptureScreen.png");
            File.WriteAllBytes(filePath, texture.EncodeToPNG());
            // To avoid memory leaks
            Destroy(texture);

            yield return null;

            onCompleted?.Invoke(filePath);
            Debug.Log("IECaptureScreen filePath:" + filePath);
        }

        private IEnumerator IEShareScreenshot(string filePath = "", string msg = "", string title = "",
            string idAppShare = "", bool isShareScreen = true)
        {
            yield return new WaitForEndOfFrame();

            yield return StartCoroutine(IECaptureScreen((string path) =>
            {
                filePath = path;
            }));

            string linkStore = socialSettingInfo.GetUrlGameOnStore();
            string nameGame = BuildManager.Instance.GetGameName();
            if(string.IsNullOrEmpty(msg)) msg = "Join with me and play " + nameGame + " at here " + linkStore;
            if (string.IsNullOrEmpty(title)) title = nameGame;

            var share = new NativeShare();

            if (!string.IsNullOrEmpty(idAppShare) && NativeShare.TargetExists(idAppShare))
            {
                share.SetTarget(idAppShare);
            }
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)) share.AddFile(filePath);

            share.SetSubject(title).SetText(msg).Share();

            // Share on WhatsApp only, if installed (Android only)
            //if( NativeShare.TargetExists( "com.whatsapp" ) )
            //	new NativeShare().AddFile( filePath ).SetText( "Hello world!" ).SetTarget( "com.whatsapp" ).Share();
            _isProcessingShareNative = false;
        }
#endregion

    }
    public enum EFieldQueryType
    {
        Name,
        FirstName,
        LastName,
        Email,
        Id,
        Link,
        Gender,
        Picture,
    }
}