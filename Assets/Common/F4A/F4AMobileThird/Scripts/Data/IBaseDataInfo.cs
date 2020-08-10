namespace com.F4A.MobileThird
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 
    /// </summary>
    public interface IBaseDataInfo
    {
        object GetData();
        void SetData(string contentData);
        void RegisterSaveData();
    }
}