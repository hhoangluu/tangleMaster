using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace com.F4A.MobileThird
{
    [AddComponentMenu("F4A/IAPButton")]
    public class IAPButton : MonoBehaviour
    {
        #pragma warning disable 414

        [SerializeField]
        private string productIdAndroid = "", productIdIOS;

        public string GetProductId()
        {
#if UNITY_ANDROID
			return productIdAndroid;
#elif UNITY_IOS
            return productIdIOS;
#else
            return "";
#endif
        }

        [SerializeField]
        private Text textGems = null;
        [SerializeField]
        private Text textPrices = null;
        [SerializeField]
        private RawImage iconGem = null;
        [SerializeField]
        private Button btnBuy = null;

        private IAPProductInfo productInfo = null;

        protected virtual void Start()
        {
            productIdAndroid = productIdAndroid.ToLower();
            productIdIOS = productIdIOS.ToLower();

            if (!btnBuy && transform.Find("Buy")) btnBuy = transform.Find("Buy").GetComponent<Button>();
            if (btnBuy)
            {
                btnBuy.onClick.RemoveAllListeners();
                btnBuy.onClick.AddListener(HandleBtnBuy_Click);
            }

            DOVirtual.DelayedCall(0.1f, () =>
            {
                Init();
            });
        }

        public virtual void Init(IAPProductInfo androidInfo, IAPProductInfo iosInfo)
        {
            if (androidInfo != null)
                productIdAndroid = androidInfo.Id;
            if (iosInfo != null)
                productIdIOS = iosInfo.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Init()
        {
#if DEFINE_IAP
            if (!string.IsNullOrEmpty(GetProductId()))
            {
                productInfo = IAPManager.Instance.GetProductInfoById(GetProductId());

                if (productInfo != null)
                {
	                if (productInfo.Type == InAppProductType.NonConsumable)
                    {
                        //textGems.text = productInfo.Description;
                        //iconGem.gameObject.SetActive(false);
                        //Vector3 localPos = textGems.transform.localPosition;
                        //textGems.transform.localPosition = new Vector3(0, localPos.y, 0);
                        //textGems.fontSize = 35;
                    }
                    else
                    {
                        textGems.text = "x " + productInfo.Coin;
                        iconGem.gameObject.SetActive(true);
                        //Vector3 localPos = textGems.transform.localPosition;
                        //textGems.transform.localPosition = new Vector3(32, localPos.y, 0);
                        //textGems.fontSize = 45;
                    }
                    textPrices.text = productInfo.Price + " $";
                }
            }
            else
            {

            }
#endif
        }

        /// <summary>
        /// Handles the button buy click.
        /// </summary>
        private void HandleBtnBuy_Click()
        {
#if DEFINE_IAP
            if (productInfo != null) {
				IAPManager.Instance.BuyProductByID (productInfo.Id);
			}
#endif
        }
    }
}
