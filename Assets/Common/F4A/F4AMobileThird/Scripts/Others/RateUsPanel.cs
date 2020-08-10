namespace com.F4A.MobileThird
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	
	public class RateUsPanel : MonoBehaviour {
		public static event System.Action OnRateUs = delegate {};
		public const string KeyRateUs = "RateUs";
		[SerializeField]
		private Text textDescription = null;
		
		public void ShowPanel(){
			if(!CPlayerPrefs.GetBool(KeyRateUs, false)){
				gameObject.SetActive(true);
			}
		}
		
		public void HandleBtnRateUs_Click(){
			SocialManager.Instance.OpenRateGame();
			if(OnRateUs != null)
				OnRateUs();
			CPlayerPrefs.SetBool(KeyRateUs, true);
			HidePanel();
		}
		
		public void HidePanel(){
			gameObject.SetActive(false);
		}
	}
}