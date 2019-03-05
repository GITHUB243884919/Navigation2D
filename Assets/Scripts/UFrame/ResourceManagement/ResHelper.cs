using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFrame.ResourceManagement;

public class ResHelper
{
    public static AssetGetter LoadAsset(string assetPath)
    {
        ResourceManager.GetInstance().Init();
        return ResourceManager.GetInstance().LoadAsset(assetPath);
    }

    public static GameObjectGetter LoadGameObject(string assetPath)
    {
        ResourceManager.GetInstance().Init();
        return ResourceManager.GetInstance().LoadGameObject(assetPath);
    }

    public static AssetGetter LoadAllAssets(string assetPath)
    {
        ResourceManager.GetInstance().Init();
        return ResourceManager.GetInstance().LoadAllAssets(assetPath);
    }

    public static void LoadAssetAsync(string assetPath, System.Action<AssetGetter> callback)
    {
        ResourceManager.GetInstance().Init();
        ResourceManager.GetInstance().LoadAssetAsync(assetPath, callback);
    }

    public static void LoadGameObjectAsync(string assetPath, System.Action<GameObjectGetter> callback)
    {
        ResourceManager.GetInstance().Init();
        ResourceManager.GetInstance().LoadGameObjectAsync(assetPath, callback);
    }

    public static void LoadAllAssetsAsync(string assetPath, System.Action<AssetGetter> callback)
    {
        ResourceManager.GetInstance().Init();
        ResourceManager.GetInstance().LoadAllAssetsAsync(assetPath, callback);
    }

    public static void RealseAllUnUse()
    {
        ResourceManager.GetInstance().Init();
        ResourceManager.GetInstance().RealseAllUnUse();
    }

    public static void DestroyGameObject(GameObject go)
    {
        ResourceManager.GetInstance().Init();
        ResourceManager.GetInstance().DestroyGameObject(go);
    }

    public static void RealseAsset(GameObject go)
    {
        ResourceManager.GetInstance().Init();
        ResourceManager.GetInstance().RealseAsset(go);
    }

    public static void LoadScene(string scenePath)
    {
        ResourceManager.GetInstance().Init();
        ResourceManager.GetInstance().LoadScene(scenePath);
    }

    public static GameObject GetPubAssetGetterGo()
    {
        return PublicAssetHolderGameObject.GetInstance().Go;

    }
}
