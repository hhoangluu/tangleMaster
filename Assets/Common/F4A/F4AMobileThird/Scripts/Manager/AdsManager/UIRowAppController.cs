using UnityEngine;
using UnityEngine.UI;

namespace com.F4A.MobileThird
{
    public class UIRowAppController : MonoBehaviour
    {
        public event System.Action OnDownload = delegate { };
        [SerializeField]
        private RawImage backgroundApp = null;
        [SerializeField]
        private RawImage iconApp = null;
        [SerializeField]
	    private Text textNameApp = null, textDescriptionApp = null;
	    [SerializeField]
	    private Button btnDownload = null;
	    
	    private WebViewRowData data = null;

        public void SetupRowApp(WebViewRowData data)
	    {
		    this.data = data;
            if (data.wwwIconApp != null && data.wwwIconApp.texture != null)
            {
                iconApp.texture = data.wwwIconApp.texture;
            }
            textNameApp.text = data.offname;
            textDescriptionApp.text = data.description;
	    }
        
	    public void HandleBtnDownload_Click(){
	    	if(this.data != null){
	    		DMCMobileUtils.OpenURL(this.data.link);
	    	}
            if(OnDownload != null)
            {
                OnDownload();
            }
	    }
    }
}