#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UFrame.ResourceManagement
{
    public partial class AssetDatabaseLoader : IResourceLoader
    {
        /// <summary>
        /// 不带扩展名-带扩展名
        /// </summary>
        Dictionary<string, string> pathMaps = new Dictionary<string, string>();
        public void Init()
        {
            LoadFileMap();
        }

        void LoadFileMap()
        {
            pathMaps.Clear();

            string[] filePaths = AssetDatabase.GetAllAssetPaths();
            foreach (var filePath in filePaths)
            {
                string unixFilePath = filePath.Replace('\\', '/');
                string fileFindPath = "";
                if (filePath.Contains("GameResources"))
                {
                    fileFindPath = unixFilePath.Replace("Assets/GameResources/", "");
                    int lastSplitIndex = fileFindPath.LastIndexOf('.');
                    //没有扩展名的算目录
                    if (lastSplitIndex <= 0)
                    {
                        continue;
                    }
                    fileFindPath = fileFindPath.Substring(0, lastSplitIndex);
                    fileFindPath = fileFindPath.ToLower();
                    if (pathMaps.ContainsKey(fileFindPath))
                    {
#if DEBUG && !PROFILER
                        Debug.LogErrorFormat("文件{0}的文件夹有同名文件 有可能导致加载错误 请处理!!!", fileFindPath);
#endif

                    }
                    pathMaps[fileFindPath] = unixFilePath;
                }
            }
            
            pathMaps["textures/unitylogo"] = "GameResources/textures/unitylogo";

        }

        string GetAssetPathWithExtend(string assetPath)
        {
            //if (assetPath.EndsWith(".lua"))
            //{
            //    return assetPath;
            //}
            string result = null;
            pathMaps.TryGetValue(assetPath, out result);
            return result;
        }


        //同步
        public  AssetGetter LoadAsset(string assetPath)
        {
            string loadPath = GetAssetPathWithExtend(assetPath.ToLower());
            Object obj = AssetDatabase.LoadAssetAtPath<Object>(loadPath);
            AssetHolder assetHolder = new AssetHolder(obj);
            AssetGetter getter = new AssetGetter();
            getter.SetAssetHolder(assetHolder);
            return getter;
        }

        public  AssetGetter LoadAllAssets(string assetPath)
        {
            string loadPath = GetAssetPathWithExtend(assetPath.ToLower());

            string pullPath = System.IO.Path.Combine(Application.dataPath, loadPath.ToLower());
            pullPath = pullPath.Replace('\\', '/');
            string [] files = System.IO.Directory.GetFiles(pullPath);
            List<Object> objs = new List<Object>();
            for (int i = 0; i < files.Length; ++i)
            {
                if (files[i].Contains(".meta"))
                {
                    continue;
                }
                //Debug.LogError(files[i]);
                //Debug.LogError(Application.dataPath.Replace('\\', '/'));
                Debug.LogError(files[i].Substring(files[i].Replace('\\', '/').LastIndexOf("/")));

                var o = AssetDatabase.LoadAssetAtPath<Object>("Assets/" + loadPath + files[i].Substring(files[i].Replace('\\', '/').LastIndexOf("/")));
                objs.Add(o);
            }

            AssetHolder assetHolder = new AssetHolder(objs.ToArray());
            AssetGetter getter = new AssetGetter();
            getter.SetAssetHolder(assetHolder);
            return getter;
        }

        public  GameObjectGetter LoadGameObject(string assetPath)
        {
            string loadPath = GetAssetPathWithExtend(assetPath.ToLower());
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(loadPath);

            AssetHolder assetHolder = new AssetHolder(prefab);
            GameObjectGetter getter = new GameObjectGetter();
            getter.SetAssetHolder(assetHolder);
            return getter;
        }

        //异步

        public  void LoadAssetAsync(string assetPath, System.Action<AssetGetter> callback)
        {
            string loadPath = GetAssetPathWithExtend(assetPath.ToLower());
            Object obj = AssetDatabase.LoadAssetAtPath<Object>(loadPath);
            AssetHolder assetHolder = new AssetHolder(obj);
            AssetGetter getter = new AssetGetter();
            getter.SetAssetHolder(assetHolder);
            callback(getter);
        }

        public  void LoadAllAssetsAsync(string assetPath, System.Action<AssetGetter> callback)
        {
            string loadPath = GetAssetPathWithExtend(assetPath.ToLower());
            Object[] objs = AssetDatabase.LoadAllAssetsAtPath(loadPath);
            AssetHolder assetHolder = new AssetHolder(objs);
            AssetGetter getter = new AssetGetter();
            getter.SetAssetHolder(assetHolder);
            callback(getter);
        }

        public  void LoadGameObjectAsync(string assetPath, System.Action<GameObjectGetter> callback)
        {
            string loadPath = GetAssetPathWithExtend(assetPath.ToLower());
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(loadPath);

            AssetHolder assetHolder = new AssetHolder(prefab);
            GameObjectGetter getter = new GameObjectGetter();
            getter.SetAssetHolder(assetHolder);
            callback(getter);
        }

        public  void RealseAllUnUse()
        {

        }

        public  void DestroyGameObject(GameObject go)
        {
            GameObject.Destroy(go);
        }

        public  void AddGameObjectAssetHolder(GameObject go, AssetHolder assetHolder)
        {

        }

        public  void RealseAsset(GameObject go)
        {
        }

        public void LoadScene(string scenePath)
        {
            scenePath = "GameResources/" + scenePath;
            UnityEngine.SceneManagement.SceneManager.LoadScene(scenePath);
        }
    }

}
#endif
