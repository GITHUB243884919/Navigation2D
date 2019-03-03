using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFrame.ResourceManagement
{

    public class GameObjectGetter : IAssetGetter
    {
        AssetHolder assetHolder;
        public void SetAssetHolder(AssetHolder assetHolder)
        {
            this.assetHolder = assetHolder;
        }

        public GameObject Get()
        {
            GameObject prefab = assetHolder.Get<GameObject>();
            GameObject go = GameObject.Instantiate<GameObject>(prefab);

            ResourceManager.GetInstance().AddGameObjectAssetHolder(go, assetHolder);
            return go;
        }

        //public void Release(GameObject go)
        //{
        //    ResourceManager.GetInstance().DestroyGameObject(go);
        //    assetHolder = null;
        //}

    }

}
