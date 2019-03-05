using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFrame.ResourceManagement
{
    public class AssetGetter: IAssetGetter
    {
        AssetHolder assetHolder;
        public void SetAssetHolder(AssetHolder assetHolder)
        {
            this.assetHolder = assetHolder;
        }

        public T Get<T>(GameObject go) where T : Object 
        {
            T t = assetHolder.Get<T>();

            ResourceManager.GetInstance().AddGameObjectAssetHolder(go, assetHolder);
            return t;
        }

        public object GetAll(GameObject go)
        {
            object obj = assetHolder.GetAll();
            ResourceManager.GetInstance().AddGameObjectAssetHolder(go, assetHolder);
            return obj;
        }

        //public void Release(GameObject go)
        //{
        //    ResourceManager.GetInstance().RealseAsset(assetHolder, go);
        //    assetHolder = null;
        //}
    }

}
