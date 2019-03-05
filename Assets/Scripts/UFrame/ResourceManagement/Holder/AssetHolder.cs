using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFrame.ResourceManagement
{
    public class AssetHolder
    {
        /// <summary>
        /// 引用asset的对象
        /// </summary>
        HashSet<GameObject> refences = new HashSet<GameObject>();

        //Object asset = null;
        object asset = null;

        public AssetHolder(object asset)
        {
            this.asset = asset;
        }

        public T Get<T>() where T : class
        {
            return asset as T;
        }

        public object GetAll()
        {
            return asset;
        }

        public void AddRefence(GameObject refence)
        {
            refences.Add(refence);
        }

        public void RemoveRefence(GameObject refence)
        {
            refences.Remove(refence);
        }

        public void Release()
        {
            asset = null;
        }

        public bool CouldRealse()
        {
            return refences.Count <= 0;
        }

        /// <summary>
        /// 移除已经为空的引用
        /// </summary>
        public void RemoveNullRefence()
        {
            refences.RemoveWhere((obj) => { return obj == null; });
        }

        /// <summary>
        /// 是否只存在这个引用
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool OnlyRefence(GameObject go)
        {
            return refences.Count == 1 && refences.Contains(go);
        }

    }

}

