using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Main : MonoBehaviour
{
    public Button btn1;
    public Button btn2;
    public Button btn3;
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

        btn3.onClick.AddListener(() =>
        {
            ResHelper.LoadScene("scenes/scripts_from_file");
        });
    }

    // Update is called once per frame
    void Update () {
		
	}
}

