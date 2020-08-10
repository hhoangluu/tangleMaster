namespace com.F4A.MobileThird
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BaseDataController : MonoBehaviour /*, IBaseDataInfo*/
    {
        [SerializeField]
        private DMCCoreDataInfo _coreDataInfo = new DMCCoreDataInfo();
        public DMCCoreDataInfo CoreDataInfo
        {
            get { return _coreDataInfo; }
        }

        [SerializeField]
        private DMCCheatComponent _cheatComponent = new DMCCheatComponent();
        public DMCCheatComponent CheatComponent
        {
            get { return _cheatComponent; }
            set { _cheatComponent = value; }
        }

        public virtual void Init() { }

        public virtual string GetData()
        {
            return "";
        }

        public virtual void RegisterSaveData() { }

        public virtual void LoadData(string contentData) { }

        protected virtual void Start()
        {
            
        }
    }
}