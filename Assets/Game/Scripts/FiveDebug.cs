using UnityEngine;

public static class FiveDebug
{
    public static bool isDebug => GameSettings.isDebug;

    public static void Log(object message, Object context)
    {
        if (!isDebug) return;
        Debug.Log(message, context);
    }

    public static void Log(object message)
    {
        if (!isDebug) return;
        Debug.Log(message);
    }

    public static void Log(string tag, object message)
    {
        if (!isDebug) return;
        Debug.Log(tag + " | " + message);
    }

    public static void Log(string tag, object message, Object context)
    {
        if (!isDebug) return;
        Debug.Log(tag + " | " + message, context);
    }

    public static void LogFormat(string format, params object[] args)
    {
        if (!isDebug) return;
        Debug.LogFormat(format, args);
    }

    public static void LogFormat(string tag, string format, params object[] args)
    {
        if (!isDebug) return;
        Debug.LogFormat(tag + " | " + format, args);
    }

    public static void LogError(object message, Object context, string calledClass = "")
    {
        if (!isDebug) return;
        Debug.LogError(message, context);
    }

    public static void LogError(object message)
    {
        if (!isDebug) return;
        Debug.LogError("<color=yellow>" + message + "</color>");
    }

    public static void LogError(string tag, object message)
    {
        if (!isDebug) return;
        Debug.LogError(tag + " | " + message);
    }

    public static void LogErrorFormat(string format, params object[] args)
    {
        if (!isDebug) return;
        Debug.LogErrorFormat(format, args);
    }

    public static void LogErrorFormat(string tag, string format, params object[] args)
    {
        if (!isDebug) return;
        Debug.LogErrorFormat(tag + " | " + format, args);
    }

    public static void LogWarning(object message, Object context, string calledClass = "")
    {
        if (!isDebug) return;
        Debug.LogWarning(message, context);
    }

    public static void LogWarning(object message, string calledClass = "")
    {
        if (!isDebug) return;
        Debug.LogWarning(message);
    }

    public static void LogWarningFormat(string format, params object[] args)
    {
        if (!isDebug) return;
        Debug.LogWarningFormat(format, args);
    }
}
