using UnityEngine;
using System;

namespace com.F4A.MobileThird
{
    /// <summary>
    /// Singleton base class
    /// </summary>
    public class Singleton<T> where T : class, new()
    {
        private static readonly T singleton = new T();

        public static T instance
        {
            get
            {
                return singleton;
            }
        }
    }

    public class ManualSingleton<T> where T : class, new()
    {
        private static T singleton = null;

        public static T instance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new T();
                }
                return singleton;
            }
        }
    }

    /// <summary>
    /// Singleton for mono behavior object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T singleton;

        public static bool IsInstanceValid() { return singleton != null; }

        void Reset()
        {
            gameObject.name = typeof(T).Name;
        }

        public static T Instance
        {
            get
            {
                if (SingletonMono<T>.singleton == null)
                {
                    SingletonMono<T>.singleton = (T)FindObjectOfType(typeof(T));
                    if (SingletonMono<T>.singleton == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = "[@" + typeof(T).Name + "]";
                        SingletonMono<T>.singleton = obj.AddComponent<T>();
                    }
                }

                return SingletonMono<T>.singleton;
            }
        }
    }

    /// <summary>
    /// Singleton for mono behavior object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ManualSingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        void Reset()
        {
            gameObject.name = typeof(T).Name;
        }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)(MonoBehaviour)this;
                Initialization();
            }
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        protected abstract void Initialization();
    }

    public abstract class SingletonMonoDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; protected set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)(MonoBehaviour)this;
                DontDestroyOnLoad(this.gameObject);
                Initialization();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        protected abstract void Initialization();
    }
}