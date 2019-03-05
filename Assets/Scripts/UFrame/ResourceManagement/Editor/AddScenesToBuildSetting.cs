using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UFrame;
public class AddScenesToBuildSetting
{
    [MenuItem("UFrame框架/资源管理/开发模式/添加场景")]
    static void AddAllScenes()
    {
        List<string> scenesPath = new List<string>();

        string scenesPathRoot = "Assets/" + UFrameConst.Scene_Dir;
        foreach (var path in Directory.GetFiles(scenesPathRoot, "*.unity", SearchOption.AllDirectories))
        {
            scenesPath.Add(path.Replace("\\", "/"));
        }

        EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[scenesPath.Count];
        for (int i = 0; i < newSettings.Length; i++)
        {
            newSettings[i] = new EditorBuildSettingsScene(scenesPath[i], true);
        }
        EditorBuildSettings.scenes = newSettings;
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
