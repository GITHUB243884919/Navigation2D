using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RunCoroutine : MonoBehaviour
{
	static GameObject crtGo = null;
    static RunCoroutine runCoroutine = null;
	public static Coroutine Run(IEnumerator routine)
	{
		GetInst();
		return runCoroutine.StartCoroutine (routine);
	}
	//mayby to crash
	public static void Stop(Coroutine runCrt)
	{
        if (null == runCrt)
			return;
        if (runCoroutine)
        {
            runCoroutine.StopCoroutine(runCrt);
        }

	}
	
	public static void GetInst ()
	{
		if (null == crtGo) {
			crtGo = GameObject.Find ("CrtRunner");
			crtGo = new GameObject ("CrtRunner");
            runCoroutine = crtGo.GetComponent<RunCoroutine>();
            if(!runCoroutine)
            {
                runCoroutine = crtGo.AddComponent<RunCoroutine>();
            }

            if (Application.isPlaying) {
				GameObject.DontDestroyOnLoad (crtGo);
			}

		}
	}
	
	public static void Release()
	{
		if(crtGo != null)
		{
			GameObject.DestroyImmediate(crtGo);
		}
		
		crtGo = null;
	}
}
