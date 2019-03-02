using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFrame.ResourceManagement
{
    public class BundleHolder
    {
        /// <summary>
        /// 引用bundle的对象
        /// </summary>
        HashSet<string> refences = new HashSet<string>();

        AssetBundle bundle = null;
//#if UNITY_EDITOR
//        string name;
//#endif 
        public BundleHolder(AssetBundle bundle)
        {
            this.bundle = bundle;
//#if UNITY_EDITOR
//            name = bundle.name;
//#endif 
        }

        public AssetBundle Get() 
        {
            return bundle;
        }

        public void AddRefence(string refence)
        {
            refences.Add(refence);
        }

        public void RemoveRefence(string refence)
        {
            refences.Remove(refence);
        }

        public void Release()
        {
//#if UNITY_EDITOR
//            Debug.LogError("UnloadBundle " + name);
//#endif 

            bundle.Unload(true);
            bundle = null;
        }

        public bool CouldRealse()
        {
            return refences.Count <= 0;
        }

        ///// <summary>
        ///// 移除已经为空的引用
        ///// </summary>
        //public void RemoveNullRefence()
        //{
        //    refences.RemoveWhere((obj) => { return obj == null; });
        //}

        ///// <summary>
        ///// 是否只存在这个引用
        ///// </summary>
        ///// <param name="go"></param>
        ///// <returns></returns>
        //public bool OnlyRefence(GameObject go)
        //{
        //    return refences.Count == 1 && refences.Contains(go);
        //}

    }

}

