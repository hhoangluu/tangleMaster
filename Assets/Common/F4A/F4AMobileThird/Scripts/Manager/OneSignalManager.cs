namespace com.F4A.MobileThird
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	
	public class OneSignalManager : SingletonMono<OneSignalManager> {
		[SerializeField]
		private string oneSignalID = "oneSignalID";
		[SerializeField]
		private string oneSignalProjectNumber = "oneSignalProjectNumber";
		private void InitOneSignal(){
			#if F4A_ONE_SIGNAL
			OneSignal.StartInit(oneSignalID, oneSignalProjectNumber)
				.Settings(new Dictionary<string, bool>() {
					{ OneSignal.kOSSettingsAutoPrompt, true },
					{ OneSignal.kOSSettingsInAppLaunchURL, true } })
			.EndInit();
			#endif
		}
	}
}