using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFrame.ResourceManagement
{    
    public interface IResourceLoader
    {
        void Init();
        AssetGetter LoadAsset(string assetPath);

        GameObjectGetter LoadGameObject(string assetPath);

        AssetGetter LoadAllAssets(string assetPath);

        void LoadAssetAsync(string assetPath, System.Action<AssetGetter> callback);

        void LoadGameObjectAsync(string assetPath, System.Action<GameObjectGetter> callback);

        void LoadAllAssetsAsync(string assetPath, System.Action<AssetGetter> callback);

        void RealseAllUnUse();

        void DestroyGameObject(GameObject go);

        void RealseAsset(GameObject go);

        void AddGameObjectAssetHolder(GameObject go, AssetHolder assetHolder);

        void LoadScene(string scenePath);
    }
}

