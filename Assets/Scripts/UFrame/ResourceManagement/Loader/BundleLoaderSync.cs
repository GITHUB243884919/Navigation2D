using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UFrame.ResourceManagement
{
    public partial class BundleLoader : IResourceLoader
    {
        public  AssetGetter LoadAllAssets(string assetName)
        {
            return LoadAsset<AssetGetter>(assetName, E_LoadAsset.LoadAll);
        }

        public  GameObjectGetter LoadGameObject(string assetName)
        {
            return LoadAsset<GameObjectGetter>(assetName, E_LoadAsset.LoadSingle);
        }

        public  AssetGetter LoadAsset(string assetName)
        {
            return LoadAsset<AssetGetter>(assetName, E_LoadAsset.LoadSingle);
        }

        T LoadAsset<T>(string assetName, E_LoadAsset eLoadAsset)
            where T : IAssetGetter, new()
        {
            T getter;
            string bundleName = GetBundleName(assetName);
            if (LoadAssetFromNameAssetHolder(assetName, bundleName, out getter))
            {
                return getter;
            }

            AssetBundle bundle = LoadBundle(assetName, bundleName);
            if (bundle == null)
            {
                return getter;
            }

            //所有bundle加载完毕
            AssetHolder assetHolder = null;
            switch (eLoadAsset)
            {
                case E_LoadAsset.LoadSingle:
                    int index = assetName.LastIndexOf("/");
                    string assetNameInBundle = assetName.Substring(index + 1);
                    //Debug.LogError(assetName + " " + assetNameInBundle);

                    assetHolder = new AssetHolder(bundle.LoadAsset(assetNameInBundle));
                    break;
                case E_LoadAsset.LoadAll:
                    assetHolder = new AssetHolder(bundle.LoadAllAssets());
                    break;
            }
            getter.SetAssetHolder(assetHolder);

            nameAssetHolders.Add(assetName, assetHolder);

            return getter;
        }

        public void LoadScene(string assetName)
        {
            string bundleName = GetBundleName(assetName);
            AssetBundle bundle = LoadBundle(assetName, bundleName);
            string[] scenes = bundle.GetAllScenePaths();
            //Debug.LogError(scenes[0]);
            UnityEngine.SceneManagement.SceneManager.LoadScene(scenes[0]);
        }

        bool LoadAssetFromNameAssetHolder<T>(string assetName, string bundleName, out T getter)
            where T : IAssetGetter, new()
        {
            // 1 从nameAssetHolders获取资源
            AssetHolder assetHolder = null;
            bundleName = GetBundleName(assetName);
            string[] dependencies = null;
            getter = new T();
            if (nameAssetHolders.TryGetValue(assetName, out assetHolder))
            {
                getter.SetAssetHolder(assetHolder);

                //记录Bundle引用
                //nameAssetHolders有,那么bundleHolders必有
                bundleHolders[bundleName].AddRefence(assetName);
                dependencies = manifest.GetAllDependencies(bundleName);
                for (int i = 0, iMax = dependencies.Length; i < iMax; ++i)
                {
                    bundleHolders[dependencies[i]].AddRefence(assetName);
                }
                return true;
            }

            return false;
        }

        AssetBundle LoadBundle(string assetName, string bundleName)
        {
            AssetBundle bundle = null;
            BundleHolder bundleHolder = null;
            // 没有加载过bundle
            string bundlePath = GetBundlePath(bundleName);
            if (!bundleHolders.TryGetValue(bundleName, out bundleHolder))
            {
                //Debug.LogError("LoadFromFile [" + bundlePath + "]");
                bundle = AssetBundle.LoadFromFile(bundlePath);
                //存bundleHolder
                bundleHolder = new BundleHolder(bundle);
                bundleHolder.AddRefence(assetName);
                bundleHolders.Add(bundleName, bundleHolder);
            }
            else
            {
                bundleHolder.AddRefence(assetName);
                bundle = bundleHolder.Get();
            }

            // 加载依赖的bundle
            string [] dependencies = manifest.GetAllDependencies(bundleName);
            for (int i = 0, iMax = dependencies.Length; i < iMax; ++i)
            {
                BundleHolder dependBundleHolder = null;
                if (bundleHolders.TryGetValue(dependencies[i], out dependBundleHolder))
                {
                    //已经存在的bundle只增加引用
                    dependBundleHolder.AddRefence(assetName);
                    continue;
                }
                //没加载过的
                bundlePath = GetBundlePath(dependencies[i]);
                //AssetBundle dependBundle = AssetBundle.LoadFromFile(Path.Combine(bundleRootPath, dependencies[i]));
                AssetBundle dependBundle = AssetBundle.LoadFromFile(bundlePath);
                dependBundleHolder = new BundleHolder(dependBundle);
                dependBundleHolder.AddRefence(assetName);
                //存bundleHolder
                bundleHolders.Add(dependencies[i], dependBundleHolder);
            }

            return bundle;
        }

    }


}
