using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class F4ABuildPostprocessor : MonoBehaviour
{
    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
        Debug.Log("@LOG OnPostprocessBuild target:" + buildTarget + "/path:" + pathToBuiltProject);
        if (buildTarget == BuildTarget.iOS)
        {
            Debug.Log("[PushNotificationsPostBuildScript] ProcessPostBuild - iOS - Adding Push Notification capabilities.");
#if UNITY_IOS

            // get XCode project path
            string pbxPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);

#if DEFINE_FIREBASE_MESSAGING
            // Add linked frameworks
            PBXProject pbxProject = new PBXProject();
            pbxProject.ReadFromString(File.ReadAllText(pbxPath));
            string targetName = PBXProject.GetUnityTargetName();
            string targetGUID = pbxProject.TargetGuidByName(targetName);
            pbxProject.AddFrameworkToProject(targetGUID, "UserNotifications.framework", false);
            File.WriteAllText(pbxPath, pbxProject.WriteToString());

            // Add required capabilities: Push Notifications, and Remote Notifications in Background Modes
            var isDevelopment = Debug.isDebugBuild;
            var capabilities = new ProjectCapabilityManager(pbxPath, "app.entitlements", "Unity-iPhone");
            capabilities.AddPushNotifications(isDevelopment);
            capabilities.AddBackgroundModes(BackgroundModesOptions.RemoteNotifications);
            capabilities.WriteToFile();
#endif

#if DEFINE_IRONSOURCE && !DEFINE_ADMOB
            // Replace with your iOS AdMob app ID. Your AdMob app ID will look
            // similar to this sample ID: ca-app-pub-3940256099942544~1458002511
            string appId = "ca-app-pub-3940256099942544~1458002511"; // Replace with your Admob APP id

            string plistPath = Path.Combine(path, "Info.plist");
            PlistDocument plist = new PlistDocument();

            plist.ReadFromFile(plistPath);
            plist.root.SetString("GADApplicationIdentifier", appId);
            File.WriteAllText(plistPath, plist.WriteToString());
#endif
#endif
        }
    }
}