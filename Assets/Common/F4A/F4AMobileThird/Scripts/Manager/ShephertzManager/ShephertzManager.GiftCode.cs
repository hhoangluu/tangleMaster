using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.F4A.MobileThird
{
	public partial class ShephertzManager
	{
		[Header("GIFT CODE")]
		[SerializeField]
		private string collectionNameGiftCode = "GiftCode";

		public void FindGiftCode(string code){
			FindDocumentByKeyValue ("code", code, collectionNameGiftCode, EStorageCallBackType.FindGiftCode);
		}

		public void SaveOrUpdateGiftCode(string code, string json){
			SaveOrUpdateDocumentByKeyValue ("code", code, json, collectionNameGiftCode);
		}
	}
}
