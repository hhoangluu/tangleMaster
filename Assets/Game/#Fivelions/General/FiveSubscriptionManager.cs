using UnityEngine;
using Five.String;

namespace FiveSubscriptionManager
{
    public interface ISubscriptionManager
    {
        SubscriptionManager subscriptionManager { get; }
    }

    public class SubscriptionManager
    {
        private bool _isBusy;
        public bool isBusy => _isBusy;

        private bool debug;
        private string name = "";
        private int _subsCount;
        public int subsCount
        {
            get { return _subsCount; }
            set
            {
                _subsCount = value;
                _isBusy = _subsCount != 0;

                if (debug)
                    Debug.Log(name.Color(Color.cyan) + " | " + isBusy.ToString().Color(isBusy ? Color.red : Color.green) + " | " + subsCount.ToString().Color(subsCount == 0 ? Color.green : Color.red));
            }
        }

        public void DebugMode(bool isOn, string name) { this.name = name; debug = isOn; }

        //public SubscriptionManager(string name, bool isOn)
        //{
        //    this.name = name;
        //    this.debug = isOn;
        //}
    }
}