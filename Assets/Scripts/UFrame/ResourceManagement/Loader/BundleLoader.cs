using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UFrame.ResourceManagement
{
    /// <summary>
    /// 限制
    /// 禁用GameObject.Instance 无法跟踪引用
    /// 禁用GameObject.Destroy  无法移除引用
    /// 销毁GO用DestroyGameObject()
    /// 释放资源用ReleaseAsset()
    /// 
    /// 没有销毁一个资源对应的bundle的接口，而是提供销毁所有没有使用的资源对应的接口
    /// 
    /// AssetHolder记录每个资源（包括未实例化的GameObject资源）的宿主GameObject
    /// BundleHolder记录每个bundle被引用的资源
    /// 
    /// LoadAllAssets的限制，只能LoadAll资源类型（非GameObject类型资源），以整体的形式计数，使用公共Gameobject做为持有者
    /// LoadAllAssets的参数的意思是这个目录，因为打包时，把这个目录下的资源打成一个bundle
    /// 这样对于Assetdatabase版本的Loader就是加载这个目录下的所有资源
    /// 
    /// 同一个目录下资源不能同名，开发环境是用AseetDatabase，底层API需要扩展名。
    /// 一个bundle下的资源不能同名
    /// 所有的资源目录，名称只能小写，用下划线分隔，安卓和ios大小写敏感，容易在PC上没事，换到移动平台出问题
    /// 
    /// </summary>
    public partial class BundleLoader : IResourceLoader
    {
        AssetBundleManifest manifest;

        string innerBundleRootPath = "";
        string outerBundleRootPath = "";

        /// <summary>
        /// asset和bundle的map关系
        /// </summary>
        Dictionary<string, string> assetMap = new Dictionary<string, string>();

        /// <summary>
        /// 资源名称对应AssetHolder
        /// </summary>
        Dictionary<string, AssetHolder> nameAssetHolders = new Dictionary<string, AssetHolder>();

        /// <summary>
        /// 引用的Go对应HashSet<AssetHolder>，销毁的时候用
        /// </summary>
        Dictionary<GameObject, HashSet<AssetHolder>> goAssetHolders = new Dictionary<GameObject, HashSet<AssetHolder>>();

        /// <summary>
        /// Bundle名称对应BundleHolder
        /// </summary>
        Dictionary<string, BundleHolder> bundleHolders = new Dictionary<string, BundleHolder>();

        /// <summary>
        /// 无用资源，每次清理时候重新构建一次
        /// </summary>
        List<string> unUseAssets = new List<string>();

        List<GameObject> unUseGameObject = new List<GameObject>();

        public enum E_LoadAsset
        {
            LoadSingle,
            LoadAll,
        }

#region 初始化assetbundle配置文件
        public  void Init()
        {
            string ApplicationStreamingPath = Application.streamingAssetsPath;
            innerBundleRootPath = Path.Combine(ApplicationStreamingPath, UFrameConst.Bundle_Root_Dir);
            outerBundleRootPath = Path.Combine(Application.persistentDataPath, UFrameConst.Bundle_Root_Dir);
            Loadmanifest();
            LoadAssetMap();
        }

        void Loadmanifest()
        {
            string bundlePath = GetBundlePath(UFrameConst.Bundle_Root_Dir + UFrameConst.Bundle_Extension);
            var bundle = AssetBundle.LoadFromFile(bundlePath);
            manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            bundle.Unload(false);
        }

        void LoadAssetMap()
        {
            string bundlePath = UFrameConst.Asset_Bundle_Txt_Name;
            bundlePath = Path.GetFileNameWithoutExtension(bundlePath);
            bundlePath += UFrameConst.Bundle_Extension;
            bundlePath = GetBundlePath(bundlePath);
            var bundle = AssetBundle.LoadFromFile(bundlePath);
            var txt = bundle.LoadAsset<TextAsset>(Path.GetFileNameWithoutExtension(bundlePath));
            string strTxt = txt.text;

            //确认asset-bundle是用“\r\n”换行，如果不是会出问题
            string[] line = strTxt.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < line.Length; ++i)
            {
                string[] temp = line[i].Split(',');
                assetMap.Add(temp[0], temp[1]);
            }
            bundle.Unload(true);
        }


        /// <summary>
        /// Todo 减少IO
        /// 应该用个配置文件来判断，是否是包外路径
        /// 比如有一个包外的文件列表，如果在这个列表里，就返回沙盒路径
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        string GetBundlePath(string bundleName)
        {
            string outerPath = Path.Combine(outerBundleRootPath, bundleName);
            string innerPath = Path.Combine(innerBundleRootPath, bundleName);

            if (File.Exists(outerPath))
            {
                return outerPath;
            }

            return innerPath;
        }

        string GetBundleName(string assetName)
        {
            string result;
            assetMap.TryGetValue(assetName, out result);
            if (string.IsNullOrEmpty(result))
            {
                //Debug.LogError("资源[" + assetName + "]没有对应的assetbundle");
                throw new System.Exception("资源[" + assetName + "]没有对应的assetbundle");
            }
            return result;
        }

#endregion

#region 释放接口
        /// <summary>
        /// 释放接口，释放所有未被引用的资源。
        /// 1.把go中，所有资源Holder可以释放的情况下，移除go（销毁go时会释放，但是释放资源时不会，所以这里要统一处理）
        /// 2.把引用计数为0的资源引用取出。本类的DestroyXX减少资源引用计数。
        /// 3.然后看看是否能把对应的bundle能释放 
        /// </summary>
        public void RealseAllUnUse()
        {
            unUseGameObject.Clear();
            bool could = true;
            foreach (var kv in goAssetHolders)
            {
                could = true;
                HashSet<AssetHolder> assetholders = kv.Value;
                foreach (var item in assetholders)
                {
                    if (!item.CouldRealse())
                    {
                        could = false;
                        break;
                    }
                }
                if (could)
                {
                    unUseGameObject.Add(kv.Key);
                }
            }

            for (int i = 0, iMax = unUseGameObject.Count; i < iMax; ++i)
            {
                goAssetHolders[unUseGameObject[i]].Clear();
                Debug.LogError("remove" + unUseGameObject[i].name);
                goAssetHolders.Remove(unUseGameObject[i]);
            }

            unUseAssets.Clear();
            foreach (var kv in nameAssetHolders)
            {
                if (kv.Value.CouldRealse())
                {
                    unUseAssets.Add(kv.Key);
                }
            }

            for (int i = 0, iMax = unUseAssets.Count; i < iMax; ++i)
            {
                string bundleName = GetBundleName(unUseAssets[i]);
                BundleHolder bundleHolder = null;
                if (bundleHolders.TryGetValue(bundleName, out bundleHolder))
                {
                    bundleHolder.RemoveRefence(unUseAssets[i]);
                    if (bundleHolder.CouldRealse())
                    {
                        bundleHolder.Release();
                        bundleHolders.Remove(bundleName);
                    }
                }

                string[] dependencies = this.manifest.GetAllDependencies(bundleName);
                for (int k = 0, kMax = dependencies.Length; k < kMax; ++k)
                {
                    BundleHolder dependBundleHolder = null;
                    if (bundleHolders.TryGetValue(dependencies[k], out dependBundleHolder))
                    {
                        dependBundleHolder.RemoveRefence(unUseAssets[i]);
                        if (dependBundleHolder.CouldRealse())
                        {
                            dependBundleHolder.Release();
                            bundleHolders.Remove(dependencies[k]);
                        }
                    }
                }

                nameAssetHolders[unUseAssets[i]].Release();
                nameAssetHolders.Remove(unUseAssets[i]);
            }
        }

        public  void DestroyGameObject(GameObject go)
        {
            //1.去资源引用
            foreach (var item in goAssetHolders[go])
            {
                item.RemoveRefence(go);
            }
            //////已经销毁的对象在维护完资源引用后不用再持有
            //goAssetHolders[go].Clear();
            //Debug.LogError("remove " + go.name);
            //goAssetHolders.Remove(go);


            //2.销毁对象
            GameObject.Destroy(go);
        }

        public  void RealseAsset(GameObject go)
        {
            //1.去资源引用
            HashSet<AssetHolder> assetHolders = null;
            if (!goAssetHolders.TryGetValue(go, out assetHolders))
            {
                return;
            }

            foreach (var item in assetHolders)
            {
                item.RemoveRefence(go);
            }

        }
        #endregion

#region 同步/异步公用函数
        /// <summary>
        /// 维护资源和GameObject的引用关系
        /// </summary>
        /// <param name="go"></param>
        /// <param name="assetHolder"></param>
        public void AddGameObjectAssetHolder(GameObject go, AssetHolder assetHolder)
        {
            //记录资源引用
            assetHolder.AddRefence(go);

            HashSet<AssetHolder> assetHolders = null;
            if (!goAssetHolders.TryGetValue(go, out assetHolders))
            {
                assetHolders = new HashSet<AssetHolder>();
                goAssetHolders.Add(go, assetHolders);
            }
            assetHolders.Add(assetHolder);
        }
#endregion

    }
}

