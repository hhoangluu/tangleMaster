using UnityEngine;
#pragma warning disable

public class FiveSingleton<T> : MonoBehaviour
{
    private static T _instance;
    public static T instance => _instance;

    protected virtual void Awake()
    {
        //Debug.LogError("Awake T: " + this.name + " | " + instance + " | " + this.GetHashCode());
        _instance = GetComponent<T>();
    }
}

public class DontDestroyOnLoadSingleton<T> : MonoBehaviour
{
    private static T _instance;
    public static T instance => _instance;

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = GetComponent<T>();

        if (transform.root == this.transform)
            DontDestroyOnLoad(this);
    }
}

public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance;
    public static T instance => _instance ?? (_instance = Resources.Load<T>(typeof(T).Name));
}