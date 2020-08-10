using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.F4A.MobileThird
{
    public partial class ShephertzManager
    {
        [Header("Administrator")]
        [SerializeField]
        private bool enableAdministrator = false;
        [SerializeField]
        private string giftCodeCreate = "";
        [SerializeField]
        private int gemRewardGiftCodeCreate = 5;

        public void SaveOrUpdateGiftCode(string code)
        {
#if DEFINE_SHEPHERTZ
			string json = "{\"code\":\"" + code 
				+ "\",\"active\":\"false\",\"gem\":"+gemRewardGiftCodeCreate+"}";
			SaveOrUpdateDocumentByKeyValue("code", code, json, collectionNameGiftCode);
#endif
        }

        public void SaveGiftcodeToServer()
        {
#if UNITY_EDITOR
			if (enableAdministrator) {
				SaveOrUpdateGiftCode (giftCodeCreate);
			}
#endif
        }
    }
}