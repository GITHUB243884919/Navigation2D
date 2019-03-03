using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("资源管理/bundle命名")]
    static void SetBundleName()
    {
        string fileMapFullPath = UnityEngine.Application.dataPath + "/GameResources/asset-bundle.txt";
        UnityEngine.Debug.LogError(fileMapFullPath);
        string fileMapPath = fileMapFullPath.Substring(fileMapFullPath.LastIndexOf("Assets/GameResources/"));
        UnityEngine.Debug.LogError(fileMapPath);
        StreamWriter sw = new StreamWriter(fileMapFullPath);
        string swContent = "";
        string[] filePaths = AssetDatabase.GetAllAssetPaths();
        foreach (var filePath in filePaths)
        {
            //不是目录GameResources跳过
            if (!filePath.Contains("GameResources"))
            {
                continue;
            }
            //unix目录格式
            string unixFilePath = filePath.Replace('\\', '/');
            
            //去Assets/GameResources/
            string fileFindPath = unixFilePath.Replace("Assets/GameResources/", "");

            if (fileFindPath.Contains("scene_nav2d/NavMesh"))
            {
                continue;
            }
            
            //没有扩展名的算目录
            int lastSplitIndex = fileFindPath.LastIndexOf('.');
            if (lastSplitIndex <= 0)
            {
                continue;
            }

            ////去扩展名
            //fileFindPath = fileFindPath.Substring(0, lastSplitIndex);
            ////转小写
            //fileFindPath = fileFindPath.ToLower();
            string bundleName;
            //去扩展名
            fileFindPath = fileFindPath.Substring(0, lastSplitIndex);
            if (fileFindPath.Contains("textures/unitylogo"))
            {
                bundleName = GetGroupBundleName(fileFindPath);
            }
            else
            {
                bundleName = GetSingleBundleName(fileFindPath);
            }

            //最后设置bundle名称
            //UnityEngine.Debug.LogError(filePath);
            var importer = AssetImporter.GetAtPath(filePath);
            importer.assetBundleName = bundleName;

            //记录asset和bundle对应关系
            swContent += (fileFindPath + "," + bundleName + "\r\n");
        }
        //对于打包到一起的,单独写对应关系
        //swContent += ("textures/unitylogo" + "," + "textures/unitylogo" + "\r\n");
        
        //不用writeline写配置文件。不同平台换行不一致，runtime解析换行不好判断
        sw.Write(swContent);
        sw.Flush();
        sw.Dispose();
        AssetDatabase.Refresh();
        var importerFileMap = AssetImporter.GetAtPath(fileMapPath);
        importerFileMap.assetBundleName = "asset-bundle";
        AssetDatabase.Refresh();
    }

    static string GetSingleBundleName(string filePath)
    {

        //转小写
        filePath = filePath.ToLower();
        return filePath;
    }

    static string GetGroupBundleName(string filePath)
    {
        int posGroup = filePath.LastIndexOf('/');
        filePath = filePath.Substring(0, posGroup);
        //转小写
        filePath = filePath.ToLower();
        return filePath;
    }

    [MenuItem("资源管理/创建bundle/StandaloneWindows")]
    static void BuildAllAssetBundles_Standalone()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/Bundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        AssetDatabase.Refresh();
    }

    [MenuItem("资源管理/创建bundle/安卓")]
    static void BuildAllAssetBundles_Android()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/Bundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);
        AssetDatabase.Refresh();
    }

    [MenuItem("资源管理/创建bundle/IOS")]
    static void BuildAllAssetBundles_IOS()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/Bundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.iOS);
        AssetDatabase.Refresh();
    }
}