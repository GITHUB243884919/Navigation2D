using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UFrame.ResourceManagement;
public class TestRes : MonoBehaviour
{

    // Use this for initialization
    //IEnumerator Start()
    void Start()
    {
        ResourceManager.GetInstance().Init();
        #region BundleTest
        StartCoroutine(TestAll());
        //StartCoroutine(TestAll2());
        //StartCoroutine(TestAsync());
        //StartCoroutine(TestAsync2());
        //StartCoroutine(TestScene());

        #endregion

        #region AssetDatabaseTest
        //TestDatbase();
        #endregion

    }

    #region BundleTest
    IEnumerator TestAll()
    {
        AssetGetter ag1 = ResourceManager.GetInstance().LoadAllAssets(
            "textures/unitylogo");
        Object[] objs = ag1.GetAll(gameObject) as Object[];
        Debug.LogError(objs.Length);

        GameObjectGetter gg1 = ResourceManager.GetInstance().LoadGameObject(
            "prefabs/mycube");
        GameObject go1 = gg1.Get();

        AssetGetter ag2 = ResourceManager.GetInstance().LoadAsset(
            "materials/mymaterial");
        Material m = ag2.Get<Material>(gameObject);

        GameObjectGetter gg2 = ResourceManager.GetInstance().LoadGameObject(
            "prefabs/mycube-parent");
        GameObject go2 = gg2.Get();

        //ag1.Release(gameObject);
        //gg1.Release(go1);
        //ag2.Release(gameObject);
        //gg2.Release(go2);
        yield return new WaitForSeconds(5);
        ResourceManager.GetInstance().DestroyGameObject(go1);
        ResourceManager.GetInstance().DestroyGameObject(go2);
        ResourceManager.GetInstance().RealseAsset(gameObject);
        ResourceManager.GetInstance().RealseAllUnUse();

        yield return new WaitForSeconds(10);
        yield return null;
    }

    IEnumerator TestAll2()
    {
        AssetGetter ag1 = ResourceManager.GetInstance().LoadAllAssets(
            "textures/unitylogo/unitylogo");

        Object[] objs = ag1.GetAll(gameObject) as Object[];
        Debug.LogError(objs.Length);

        GameObjectGetter gg1 = ResourceManager.GetInstance().LoadGameObject(
            "prefabs/mycube");
        GameObject go1 = gg1.Get();

        AssetGetter ag2 = ResourceManager.GetInstance().LoadAsset(
            "materials/mymaterial");

        Material m = ag2.Get<Material>(gameObject);

        GameObjectGetter gg2 = ResourceManager.GetInstance().LoadGameObject(
            "prefabs/mycube-parent");
        GameObject go2 = gg2.Get();

        yield return new WaitForSeconds(10);
    }

    IEnumerator TestAsync()
    {
        ResourceManager.GetInstance().LoadGameObjectAsync(
            "MyCube-Parent",
            (getter) =>
            {
                //GameObject go1 = (getter as GameObjectGetter).Get();
                GameObject go1 = getter.Get();
            });


        ResourceManager.GetInstance().LoadAssetAsync(
            "MyMaterial",
            //BundleLoader.E_LoadAsset.LoadSingle,
            (getter) =>
            {
                //Material m = (getter as AssetGetter).Get<Material>(gameObject);
                Material m = getter.Get<Material>(gameObject);
            });


        ResourceManager.GetInstance().LoadAllAssetsAsync(
            "unitylogo",
            //BundleLoader.E_LoadAsset.LoadAll,
            (getter) =>
            {
                //Object[] objs = (getter as AssetGetter).GetAll(gameObject) as Object[];
                Object[] objs = getter.GetAll(gameObject) as Object[];
                Debug.LogError(objs.Length);
            });


        yield return null;
    }

    IEnumerator TestAsync2()
    {
        ResourceManager.GetInstance().LoadGameObjectAsync(
            "prefabs/mycube-parent",
            (getter) =>
            {
                //GameObject go1 = (getter as GameObjectGetter).Get();
                GameObject go1 = getter.Get();
            });


        ResourceManager.GetInstance().LoadAssetAsync(
            "materials/mymaterial",
            //BundleLoader.E_LoadAsset.LoadSingle,
            (getter) =>
            {
                //Material m = (getter as AssetGetter).Get<Material>(gameObject);
                Material m = getter.Get<Material>(gameObject);
            });


        ResourceManager.GetInstance().LoadAllAssetsAsync(
            "textures/unitylogo/unitylogo",
            //BundleLoader.E_LoadAsset.LoadAll,
            (getter) =>
            {
                //Object[] objs = (getter as AssetGetter).GetAll(gameObject) as Object[];
                Object[] objs = getter.GetAll(gameObject) as Object[];
                Debug.LogError(objs.Length);
            });

        ResourceManager.GetInstance().LoadGameObjectAsync(
            "prefabs/mycube",
            (getter) =>
            {
                //GameObject go1 = (getter as GameObjectGetter).Get();
                GameObject go1 = getter.Get();
            });

        yield return null;
    }

    IEnumerator TestScene()
    {


        //UnityEngine.SceneManagement.SceneUtility.
        //UnityEngine.SceneManagement.SceneManager.CreateScene()

        

        //BundleLoader.GetInstance().LoadAsset<Object>("TestAB_Scene", gameObject);
        //UnityEngine.SceneManagement.SceneManager.LoadScene("TestAB_Scene");

        //BundleLoader.GetInstance().RealseAllUnUse();
        yield return null;
    }
    #endregion

    #region AssetDatabaseTest
    void TestDatbase()
    {
        GameObjectGetter gogetter = ResourceManager.GetInstance().LoadGameObject("prefabs/cube");
        GameObject go = gogetter.Get();
        AssetGetter assetgetter = ResourceManager.GetInstance().LoadAsset("materials/mymaterial");
        Material m = assetgetter.Get<Material>(gameObject);
    }
    #endregion

    // Update is called once per frame
    void Update () {
		
	}
}
