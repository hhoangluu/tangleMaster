namespace com.F4A.MobileThird{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System;
	
	public class SF4AListenerManager : Singleton<SF4AListenerManager> {
		public enum SF4AListenType
		{
			// iap
			IAP_BUY_SUCCEEDED,
			IAP_BUY_FAILED,
		}
		
		public Dictionary<SF4AListenType, SGroupListenter> listeners = new Dictionary<SF4AListenType, SGroupListenter>();
	
		public void Register(SF4AListenType type, Action<object> action)
		{
			if (!listeners.ContainsKey(type))
			{
				listeners.Add(type, new SGroupListenter());
			}
			if (listeners[type] != null)
			{
				listeners[type].Attach(action);
			}
		}
	
		public void Unregister(SF4AListenType type, Action<object> action)
		{
			if (listeners.ContainsKey(type) && listeners[type] != null)
			{
				listeners[type].Detach(action);
			}
		}
	
		public void UnregisterAll(Action<object> action)
		{
			foreach (SF4AListenType key in listeners.Keys)
			{
				Unregister(key, action);
			}
		}
	
		public void Broadcast(SF4AListenType type, object value = null)
		{
			if (listeners.ContainsKey(type) && listeners[type] != null)
			{
				listeners[type].Broadcast(value);
			}
		}
	
		public void Clear()
		{
			listeners.Clear();
		}
	}
	
	public class SGroupListenter
	{
	    List<Action<object>> actions = new List<Action<object>>();
	
	    public void Broadcast(object value)
	    {
		    for (int i = 0; i < actions.Count; i++)
		    {
			    actions[i](value);
		    }
	    }
	
	    public void Attach(Action<object> action)
	    {
		    for (int i = 0; i < actions.Count; i++)
		    {
			    if (actions[i] == action)
				    return;
		    }
		    actions.Add(action);
	    }
	
	    public void Detach(Action<object> action)
	    {
		    for (int i = 0; i < actions.Count; i++)
		    {
			    if (actions[i] == action)
			    {
				    actions.Remove(action);
				    break;
			    }
		    }
	    }
	}
}