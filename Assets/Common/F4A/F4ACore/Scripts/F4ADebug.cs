using System.IO;
using UnityEngine;

namespace com.F4A.MobileThird
{
    public enum ColorDebug
    {
        Black,
        White,
        Blue,
        Green,
        Yellow,
        Red,
    }

    public class F4ADebug
    {
        private static string Tag = "XMirror/ ";

        private static string PathLog = Path.Combine(DMCMobileUtils.GetStreamingAssetsPath(), "Log.txt");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public static void Log(string log)
	    {
		    Debug.Log(Tag + log);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="color"></param>
        public static void Log(string log, ColorDebug color)
        {
#if DEBUG_ENABLE && UNITY_EDITOR
            Debug.Log("<color=" + color.ToString().ToLower() + ">" + Tag + log + "</color>");
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="platform"></param>
        public static void WriteLog(string log)
        {
            Log(log);
            string time = System.DateTime.Now.ToString("yyyy:MM:d h:mm:ss tt");
            StreamWriter sw = null;
            try
            {
                if (!File.Exists(PathLog))
                    sw = File.CreateText(PathLog);
                else
                    sw = File.AppendText(PathLog);
                sw.WriteLine(time + "\t" + log);
            }
            finally
            {
                sw.Close();
                sw.Dispose();
            }
        }
    }
}