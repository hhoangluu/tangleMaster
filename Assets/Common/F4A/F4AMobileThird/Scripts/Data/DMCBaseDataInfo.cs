namespace com.F4A.MobileThird
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DMCBaseDataInfo<T> : IBaseDataInfo where T : class, new()
    {
        protected string _nameData;
        public string NameData
        {
            set { _nameData = value; }
            get
            {
                if (string.IsNullOrEmpty(_nameData)) _nameData = DataInfo.ToString();
                return _nameData;
            }
        }

        private T _dataInfo;
        public T DataInfo
        {
            set { _dataInfo = value; }
            get
            {
                if (_dataInfo == null) _dataInfo = new T();
                return _dataInfo;
            }
        }

        public virtual object GetData()
        {
            return DataInfo;
        }

        public virtual void RegisterSaveData()
        {
            
        }

        public virtual void SetData(string contentData)
        {
            if (string.IsNullOrEmpty(contentData))
            {
                DataInfo = new T();
            }
            else
            {
                DataInfo = JsonConvert.DeserializeObject<T>(contentData);
            }
        }
    }
}