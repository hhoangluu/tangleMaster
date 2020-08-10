using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.F4A.MobileThird
{
    public class UIWebViewScrollController : MonoBehaviour
    {
        [SerializeField]
        private GameObject rowAppPrefab = null;
        //[SerializeField]
        //private SpawnPool rowAppPool = null;
        [SerializeField]
        private RectTransform rectContent = null;
        [SerializeField]
        private RectTransform rectBackgroundViewport = null;
        [SerializeField]
        private RectTransform rectFrontgroundViewport = null;
        [SerializeField]
        private Button btnClose = null;

        public bool ShowWebViewScroll(List<WebViewRowData> listData)
        {
            if (listData == null || listData.Count == 0)
            {
                return false;
            }
            else
            {
                gameObject.SetActive(true);
                DespawnAllRowApp();
                foreach (WebViewRowData data in listData)
                {
                    if (data.IsCorrectData())
                    {
                        var row = SpawnRowApp();
                        if (row)
                        {
                            UIRowAppController rowAppController = row.GetComponent<UIRowAppController>();
                            if (rowAppController)
                            {
                                rowAppController.SetupRowApp(data);
                            }
                            rowAppController.OnDownload -= HandleBtnClose_Click;
                            rowAppController.OnDownload += HandleBtnClose_Click;
                        }
                    }
                }

                //// fix size content and scroll
                //if(rectContent && rectContent.GetComponent<GridLayoutGroup>() != null){
                //	var grid = rectContent.GetComponent<GridLayoutGroup>();
                //	int y = ((int)grid.cellSize.y + (int)grid.spacing.y) * listData.Count + grid.padding.top + grid.padding.bottom;
                //	var sizeDelta = rectBackgroundViewport.sizeDelta;
                //    if(y > 1050){
                //        sizeDelta.y = 1050;
                //    }
                //    else{
                //        sizeDelta.y = y;
                //    }
                //    rectBackgroundViewport.sizeDelta = sizeDelta + new Vector2(0, 150);
                //    rectFrontgroundViewport.sizeDelta = sizeDelta + new Vector2(0, 150);

                //    //var sizeScroll = scrollRect.sizeDelta;
                //    //sizeScroll.y = sizeDelta.y;
                //    //scrollRect.sizeDelta = sizeScroll;
                //}
                return true;
            }
        }

        public void HideWebViewScroll()
        {
            gameObject.SetActive(false);
        }

        private GameObject SpawnRowApp()
        {
            //if (rowAppPrefab)
            //{
                //if (rowAppPool)
                //{
                //    var row = rowAppPool.Spawn(rowAppPrefab);
                //    return row.gameObject;
                //}
                //else
                //{
                //    return Instantiate<GameObject>(rowAppPrefab);
                //}
            //}
            //else
            {
                return null;
            }
        }

        private void DespawnRowApp(Transform xtransform)
        {
            //if (rowAppPool)
            //{
            //    if (rowAppPool.IsSpawned(xtransform))
            //    {
            //        rowAppPool.Despawn(xtransform);
            //    }
            //    else
            //    {
            //        xtransform.gameObject.SetActive(false);
            //    }
            //}
            //else
            //{
            //    xtransform.gameObject.SetActive(false);
            //}
        }

        private void DespawnAllRowApp()
        {
            //if (rowAppPool)
            //{
            //    rowAppPool.DespawnAll();
            //}
        }

        public GameObject CreateRowAppController()
        {
            var row = Instantiate<GameObject>(rowAppPrefab);
            return row;
        }

        int countClickBtnClose = 0;
        public void HandleBtnClose_Click()
        {
            countClickBtnClose++;
            if (countClickBtnClose >= 3)
            {
                AdsManager.Instance.LoadFileDataWebView();
                countClickBtnClose = 0;
            }
            HideWebViewScroll();
        }
    }
}