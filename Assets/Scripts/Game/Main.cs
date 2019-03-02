using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Main : MonoBehaviour {

    public Button btn1;
    public Button btn2;
    // Use this for initialization
    void Start ()
    {
        btn1.onClick.AddListener(() =>
        {
            var getter = ResHelper.LoadGameObject("prefabs/cube");
            getter.Get();
        });

        btn2.onClick.AddListener(() =>
        {
            ResHelper.LoadScene("scenes/scene_nav2d");
        });
    }

    // Update is called once per frame
    void Update () {
		
	}
}
//public class LoadScene : MonoBehaviour
//{
//    private AssetBundle myLoadedAssetBundle;
//    private string[] scenePaths;

//    // Use this for initialization
//    void Start()
//    {
//        myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/scenes");
//        scenePaths = myLoadedAssetBundle.GetAllScenePaths();
//    }

//    void OnGUI()
//    {
//        if (GUI.Button(new Rect(10, 10, 100, 30), "Change Scene"))
//        {
//            Debug.Log("Scene2 loading: " + scenePaths[0]);
//            SceneManager.LoadScene(scenePaths[0], LoadSceneMode.Single);
//        }
//    }
//}
