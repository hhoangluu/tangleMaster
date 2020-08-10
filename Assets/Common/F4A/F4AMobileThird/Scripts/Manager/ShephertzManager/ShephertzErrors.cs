using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.F4A.MobileThird
{
    /// <summary>
    /// Error codes returned by App42 API
    /// </summary>
    public enum ShephertzErrorCode
    {
        BAD_REQUEST = 1400, //- The Request parameters are invalid.
        UNAUTHORIZED = 1401, //- Client is not authorized.
        INTERNAL_SERVER_ERROR = 1500, //- Internal Server Error. Please try again.
        NOT_FOUND_1 = 3800, //- Twitter App Credentials(ConsumerKey / ConsumerSecret) does not exist.
        NOT_FOUND_2 = 3802, //- Twitter User Access Credentials does not exist. Please use linkUserTwitterAccount API to link the User Twitter account.
        //3803 - BAD REQUEST - The Twitter Access Credentials are invalid." + &lt;Exception Message>.
        //3804 - NOT FOUND - Facebook App Credentials(ConsumerKey/ConsumerSecret) does not exist.
        //3805 - BAD REQUEST - The Facebook Access Credentials are invalid + &lt; Received Facebook Exception Message>.
        //3806 - NOT FOUND - Facebook User Access Credentials does not exist. Please use linkUserFacebookAccount API to link the User facebook account.
        //3807 - NOT FOUND - LinkedIn App Credentials(ApiKey/SecretKey) does not exist.
        //3808 - BAD REQUEST - The Access Credentials are invalid + &lt; Exception Message>.
        //3809 - NOT FOUND - LinkedIn User Access Credentials does not exist. Please use linkUserLinkedInAccount API to link the User LinkedIn account.
        //3810 - NOT FOUND - Social App Credentials do not exist.
        //3811 - NOT FOUND - User Social Access Credentials do not exist. Please use linkUserXXXXXAccount API to link the User Social account.
    }
    public class ShephertzErrors
    {
    }
}