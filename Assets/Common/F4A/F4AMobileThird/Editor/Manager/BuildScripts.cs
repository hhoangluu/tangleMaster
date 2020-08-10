using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildScripts
{
    /*
    [MenuItem("Build/All")]
    static void RunBuilds()
    {
        var gameKeys = GetGameKeys();

        var currDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        var cmdArgs = Environment.GetCommandLineArgs();

        var output = cmdArgs.Where(d => d.StartsWith(OutputCommandLineArg))
                            .Select(d => d.Substring(OutputCommandLineArg.Length))
                            .FirstOrDefault()
                            ?? "output";

        var games = (Environment.GetEnvironmentVariable(EnvironmentGames) ?? String.Empty)
                                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        if (!games.Contains(output))
        {
            return;
        }

        var publisherString = (Environment.GetEnvironmentVariable(EnvironmentPublisher) ?? String.Empty);
        var publisher = publisherString.Split(Splitters, StringSplitOptions.RemoveEmptyEntries);

        var envString = (Environment.GetEnvironmentVariable(EnvironmentBuildType) ?? String.Empty);
        var envs = envString.Split(Splitters, StringSplitOptions.RemoveEmptyEntries);

        var envScenePath = (Environment.GetEnvironmentVariable(EnvironmentScenePath) ?? String.Empty);
        var scenepath = envScenePath.Split(Splitters, StringSplitOptions.RemoveEmptyEntries);

        Debug.Log("Starting build for " + output + " " + envString);

        if (Directory.Exists(OutputPath))
            Directory.Delete(OutputPath, true);

        var processes = new List<System.Diagnostics.Process>();

        for (int i = 0; i < envs.Length; i++)
        {
            CopyDll(envs[i]);
            System.Threading.Thread.Sleep(2000);
            for (int index = 0; index < publisher.Length; index++)
            {
                Build(scenepath, currDir, publisher[index], envs[i], output);

                Debug.Log("Compacting " + envs[i] + " " + output);

                var proc = Compact(publisher[index], envs[i], output, gameKeys);
                if (proc != null)
                    processes.Add(proc);
            }
        }

        if (processes.Count > 0)
        {
            Debug.Log("Waiting for compenc to finish");

            foreach (var p in processes)
            {
                // wait for 5 min
                if (!p.WaitForExit(5 * 60 * 1000))
                {
                    Debug.Log("compenc wait timeout, skip for next one");
                }
            }
        }

        Debug.Log("Finished build");
    }

    private static System.Diagnostics.Process Compact(string publisher, string env, string output, Dictionary<string, string> gameKeys)
    {
        string password;
        var compenc = Environment.GetEnvironmentVariable(EnvironmentCompEnc);
        var targetPath = Environment.GetEnvironmentVariable(EnvironmentPackageOutput);

        if (String.IsNullOrEmpty(compenc) || String.IsNullOrEmpty(targetPath) || !gameKeys.TryGetValue(output, out password))
        {
            return null;
        }

        var outputPath = Path.Combine(OutputPath, Path.Combine(publisher, env));
        outputPath = Path.Combine(outputPath, output.ToLower());

        //delete debug file
        string[] debugFiles = Directory.GetFiles(outputPath, "*.pdb");
        for (int i = 0; i < debugFiles.Length; i++)
        {
            File.Delete(debugFiles[i]);
        }

        targetPath = Path.Combine(targetPath, DateTime.Now.ToString("yyyyMMdd"));
        targetPath = Path.Combine(targetPath, Path.Combine(publisher, GetBuildPackageName(env)));
        if (!Directory.Exists(targetPath))
            Directory.CreateDirectory(targetPath);

        var envCompEncFlag = (Environment.GetEnvironmentVariable(EnvironmentComEncFlag) ?? "false");
        bool compEncFlag;
        Boolean.TryParse(envCompEncFlag, out compEncFlag);

        if (compEncFlag)
        {
            //Dlls
            string dataOutputPath = Path.Combine(outputPath, Path.Combine(output.ToLower() + "_Data", "Managed"));
            string dataTargetPath = Path.Combine(outputPath, Path.Combine(output.ToLower() + "_Data", "mData.dat"));

            Debug.Log("Data Path " + dataOutputPath + " " + dataTargetPath);
            System.Diagnostics.Process dataCompenc = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                Arguments = Path.Combine(Directory.GetCurrentDirectory(), dataOutputPath) + " -p" + password + " -o" + Path.Combine(Directory.GetCurrentDirectory(), dataTargetPath) + " -b64",
                FileName = compenc,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
            });

            dataCompenc.WaitForExit();
            if (Directory.Exists(dataOutputPath))
            {
                Directory.Delete(dataOutputPath, true);
            }
            //Streaming  Assets
            string assetsOutputPath = Path.Combine(outputPath, Path.Combine(output.ToLower() + "_Data", "StreamingAssets"));
            string assetsTargetPath = Path.Combine(outputPath, Path.Combine(output.ToLower() + "_Data", "mData1.dat"));

            Debug.Log("Assets Path " + assetsOutputPath + " " + assetsTargetPath);
            System.Diagnostics.Process assetsCompenc = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                Arguments = Path.Combine(Directory.GetCurrentDirectory(), assetsOutputPath) + " -p" + password + " -o" + Path.Combine(Directory.GetCurrentDirectory(), assetsTargetPath) + " -b64",
                FileName = compenc,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
            });

            assetsCompenc.WaitForExit();
            if (Directory.Exists(assetsOutputPath))
            {
                Directory.Delete(assetsOutputPath, true);
            }

            targetPath = Path.Combine(targetPath, output.ToLower() + ".dat");
            Debug.Log("Running compenc: " + outputPath + " " + targetPath);

            return System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                Arguments = Path.Combine(Directory.GetCurrentDirectory(), outputPath) + " -p" + password + " -o" + targetPath + " -b64",
                FileName = compenc,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
            });
        }
        else
        {
            targetPath = Path.Combine(targetPath, output.ToLower());
            CopyAll(new DirectoryInfo(outputPath), new DirectoryInfo(targetPath));
            return null;
        }

    }

    private static Dictionary<string, string> GetGameKeys()
    {
        var filename = Environment.GetEnvironmentVariable(EnvironmentGameKeys);

        Debug.Log("Getting game-keys " + filename);

        if (String.IsNullOrEmpty(filename))
            return new Dictionary<string, string>();

        return File.ReadAllLines(filename)
                   .Where(d => !String.IsNullOrEmpty(d))
                   .Select(d => d.Split('\t'))
                   .ToDictionary(d => d[0], d => d[1], StringComparer.OrdinalIgnoreCase);
    }

    private static void CopyDll(string env)
    {
        var dllsPath = Environment.GetEnvironmentVariable(DllPathName);
        string builtEnv = GetBuildDllPackageName(env);
        string absoluteDllPaths = string.Format(@"{0}\{1}\{2}", dllsPath, builtEnv, VersionFolderName);

        CopyAll(new DirectoryInfo(absoluteDllPaths), new DirectoryInfo(string.Format(@"{0}\Plugins", Application.dataPath)));
    }

    //[MenuItem("Build/HD")]
    //static void RunBuildHD()
    //{
    //    var currDir = new DirectoryInfo(Directory.GetCurrentDirectory());
    //    Debug.Log("CURRENT " + currDir);
    //    Build(null, currDir, "hd", "output");
    //    Compact("", "web", "output", null);
    //    Compact("", "web", "output", null);
    //}

    //[MenuItem("Build/SD")]
    //static void RunBuildSD()
    //{
    //    var currDir = new DirectoryInfo(Directory.GetCurrentDirectory());
    //    Build(null, currDir, "sd", "output");
    //}

    //[MenuItem("Build/Machine")]
    //static void RunBuildMachine()
    //{
    //    var currDir = new DirectoryInfo(Directory.GetCurrentDirectory());
    //    Build(null, currDir, "machine", "output");
    //}

    //[MenuItem("Build/Tab")]
    //static void RunBuildTab()
    //{
    //    var currDir = new DirectoryInfo(Directory.GetCurrentDirectory());
    //    Build(null, currDir, "tab", "output");
    //}

    //[MenuItem("Build/Android")]
    //static void RunBuildAndroid()
    //{
    //    var currDir = new DirectoryInfo(Directory.GetCurrentDirectory());
    //    Build(null, currDir, "android", "output");
    //}

    //[MenuItem("Build/Web")]
    //static void RunBuildWeb()
    //{
    //    var currDir = new DirectoryInfo(Directory.GetCurrentDirectory());
    //    Build(null, currDir, "web", "output");
    //    //Build(scenepath, currDir, envs[i], output);
    //    Compact("", "web", "output", null);
    //}

    [MenuItem("Build/WebGL")]
    static void RunBuildWeb()
    {
        PlayerSettings.SetPropertyString("emscriptenArgs", "-s ALLOW_MEMORY_GROWTH=1", BuildTargetGroup.WebGL);
        var currDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        Build(null, currDir, "joker", "webgl", "WebGL");

    }

    private static void Build(string[] envScenePaths, DirectoryInfo currDir, string publisher, string env, string output)
    {
        Debug.Log("Building for " + env + " " + publisher);

        BuildTarget buildTarget;
        buildTarget = GetBuildTarget(env);

        CopyEnvironment(currDir, env);

        ApplySetting(publisher, env);

        var outputPath = Path.Combine(OutputPath, Path.Combine(publisher, env));
        outputPath = Path.Combine(outputPath, output.ToLower());

        if (buildTarget == BuildTarget.StandaloneWindows || buildTarget == BuildTarget.StandaloneWindows64)
        {
            outputPath = Path.Combine(outputPath, output.ToLower() + ".exe");
        }
        else if (buildTarget == BuildTarget.WebGL)
        {
            outputPath = Path.Combine(outputPath, "WebGL");
        }

        if (envScenePaths == null)
        {
            Debug.LogError("Scence Path not available, temporary use enable build setting");
            envScenePaths = EditorBuildSettings.scenes.Where(d => d.enabled).Select(d => d.path).ToArray();
            Debug.Log("Scene " + envScenePaths[0]);
        }

        EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);
        var error = BuildPipeline.BuildPlayer(envScenePaths, outputPath, buildTarget, BuildOptions.None);
        if (!String.IsNullOrEmpty(error))
        {
            Debug.LogError("Error in building for " + env + "\r\n" + error);
        }

        Debug.Log("Finished build for " + env);
    }

    private static void CopyEnvironment(DirectoryInfo currDir, string env)
    {
        var copyPath = Path.Combine(Directory.GetCurrentDirectory(), TeamCityBasePath);
        copyPath = Path.Combine(copyPath, env);
        var dirInfo = new DirectoryInfo(copyPath);
        if (dirInfo.Exists)
        {
            Debug.LogWarning("Copying... " + dirInfo);
            CopyAll(dirInfo, currDir);
        }
    }

    private static void ApplySetting(string publisher, string env)
    {
        BuildTargetGroup targetGroup = BuildTargetGroup.Standalone;
        int[] iconSize = new int[0];
        string defineSymbol = "";

        switch (env)
        {
            case "sd":
                targetGroup = BuildTargetGroup.Standalone;
                QualitySettings.masterTextureLimit = 3;
                QualitySettings.vSyncCount = 0;
                iconSize = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.Standalone);
                defineSymbol = "SD";
                break;
            case "hd":
                targetGroup = BuildTargetGroup.Standalone;
                QualitySettings.masterTextureLimit = 0;
                QualitySettings.vSyncCount = 1;
                iconSize = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.Standalone);
                defineSymbol = "HD";
                break;
            case "tab":
                targetGroup = BuildTargetGroup.Standalone;
                QualitySettings.masterTextureLimit = 3;
                QualitySettings.vSyncCount = 0;
                iconSize = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.Standalone);
                defineSymbol = "TAB";
                break;
            case "machine":
                targetGroup = BuildTargetGroup.Standalone;
                QualitySettings.masterTextureLimit = 3;
                QualitySettings.vSyncCount = 0;
                iconSize = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.Standalone);
                defineSymbol = "MACHINE";
                break;
            case "hd_split":
                targetGroup = BuildTargetGroup.Standalone;
                QualitySettings.masterTextureLimit = 0;
                QualitySettings.vSyncCount = 1;
                iconSize = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.Standalone);
                defineSymbol = "SPLIT_SCREEN";
                break;
            default:
                QualitySettings.masterTextureLimit = 0;
                QualitySettings.vSyncCount = 1;
                iconSize = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.Standalone);
                break;
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, GetScriptDefineSymbol(targetGroup, defineSymbol));
        PlayerSettings.showUnitySplashScreen = false;
        PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.Disabled;
        
        Texture2D icon = Resources.Load<Texture2D>(string.Format("Icon_{0}", publisher.ToLower()));

        var icons = new Texture2D[iconSize.Length];
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i] = icon;
        }

        PlayerSettings.SetIconsForTargetGroup(targetGroup, icons);
    }

    private static string GetScriptDefineSymbol(BuildTargetGroup targetGroup, string env)
    {
        string[] scriptDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup).Split(';');
        if (scriptDefineSymbols.Length <= 0)
        {
            return env.ToUpper();
        }
        else
        {
            List<string> mergeEnv = new List<string>();
            for (int i = 0; i < scriptDefineSymbols.Length; i++)
            {
                Debug.Log("script sysmbol " + scriptDefineSymbols[i]);
                bool isExist = false;
                for (int j = 0; j < envs.Length; j++)
                {
                    if (scriptDefineSymbols[i] == envs[j])
                    {
                        isExist = true;
                    }
                }
                if (!isExist)
                    mergeEnv.Add(scriptDefineSymbols[i]);
            }

            mergeEnv.Add(env.ToUpper());
            return string.Join(";", mergeEnv.ToArray());
        }
    }

    private static BuildTarget GetBuildTarget(string env)
    {
        BuildTarget buildTarget;
        switch (env)
        {
            case "web":
                buildTarget = BuildTarget.WebPlayer;
                break;
            case "webgl":
                buildTarget = BuildTarget.WebGL;
                PlayerSettings.SetPropertyString("emscriptenArgs", "-s ALLOW_MEMORY_GROWTH=1", BuildTargetGroup.WebGL);
#if UNITY_5_3
                PlayerSettings.stripEngineCode = false;
#else
                PlayerSettings.strippingLevel = StrippingLevel.Disabled;
#endif
                break;
            case "android":
                buildTarget = BuildTarget.Android;
                break;
            case "ios":
                buildTarget = BuildTarget.iOS;
                break;
            default:
                buildTarget = BuildTarget.StandaloneWindows;
                break;
        }

        return buildTarget;
    }

    private static BuildTargetGroup GetBuildTargetGroup(string env)
    {
        BuildTargetGroup buildTarget;
        switch (env)
        {
            case "web":
                buildTarget = BuildTargetGroup.WebPlayer;
                break;
            case "webgl":
                buildTarget = BuildTargetGroup.WebGL;
                break;
            case "android":
                buildTarget = BuildTargetGroup.Android;
                break;
            case "ios":
                buildTarget = BuildTargetGroup.iOS;
                break;
            default:
                buildTarget = BuildTargetGroup.Standalone;
                break;
        }

        return buildTarget;
    }

    private static string GetBuiltTargetGroup(string env)
    {
        switch (env)
        {
            case "web":
                return "web";
            case "webgl":
                return "webgl";
            case "ios":
            case "android":
                return "mobile";
            case "hd":
                return "desktop";
            case "hd_split":
                return "desktop-split";
            case "sd":
                return "desktop-sd";
            case "tab":
                return "tab";
            case "machine":
                return "desktop-m";
        }

        return "desktop";
    }

    private static string GetBuildPackageName(string env)
    {
        switch (env)
        {
            case "web":
                return "web";
            case "webgl":
                return "webgl";
            case "ios":
            case "android":
                return "mobile";
            case "hd":
                return "desktop";
            case "hd_split":
                return "desktop-split";
            case "sd":
                return "desktop-sd";
            case "tab":
                return "tab";
            case "machine":
                return "desktop-m";
        }

        return "desktop";
    }

    private static string GetBuildDllPackageName(string env)
    {
        switch (env)
        {
            case "webgl":
                return "webgl";
        }

        return "desktop";
    }

    private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
        Debug.LogWarning("Target Dir " + target.FullName + " " + target.Name);
        Directory.CreateDirectory(target.FullName);

        Debug.LogWarning("Source Files Count" + source.FullName + " " + source.Name);
        // Copy each file into the new directory.
        foreach (FileInfo fi in source.GetFiles())
        {
            Debug.Log("Copy From " + fi.FullName);
            Debug.Log(string.Format(@"Copying {0}\{1}", target.FullName, fi.Name));
            string targetPath = Path.Combine(target.FullName, fi.Name);
            if (File.Exists(targetPath))
            {
                FileInfo targetFile = new FileInfo(targetPath);
                targetFile.Delete();
            }
            File.Copy(fi.FullName, targetPath, true);
        }

        // Copy each subdirectory using recursion.
        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir =
                target.CreateSubdirectory(diSourceSubDir.Name);
            CopyAll(diSourceSubDir, nextTargetSubDir);
        }
    }

    private const string TeamCityBasePath = "teamcity";
    private const string OutputPath = "publish";

    private const string OutputCommandLineArg = "-out:";

    private const string EnvironmentBuildType = "BUILD_TYPE";
    private const string EnvironmentPublisher = "PUPLISHER";

    private const string EnvironmentGameKeys = "GAME_KEYS";
    private const string EnvironmentGames = "GAMES";
    private const string EnvironmentCompEnc = "COMPENC";
    private const string EnvironmentComEncFlag = "ENABLE_COMPENC";
    private const string EnvironmentPackageOutput = "PACKAGE_OUT";
    private const string EnvironmentScenePath = "SCENE_PATH";

    private const string DllPathName = "DLL_PATH";
#if UNITY_5_3
    private const string VersionFolderName = "5.3.2";
#else
    private const string VersionFolderName = "5.1.3";
#endif

    private static readonly string[] envs = new[] { "HD", "SD", "TAB", "MACHINE", "SPLIT_SCREEN" };
    private static readonly char[] Splitters = new[] { ';' };
*/
}
