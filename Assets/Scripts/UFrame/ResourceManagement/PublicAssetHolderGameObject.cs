using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFrame.Common;
namespace UFrame.ResourceManagement
{
    public class PublicAssetHolderGameObject : SingletonMono<PublicAssetHolderGameObject>
    {
        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        public override void OnDestroy()
        {
            ResHelper.RealseAsset(Go);
            base.OnDestroy();
        }

        GameObject go;
        public GameObject Go
        {
            get
            {
                if (go == null)
                {
                    go = gameObject;
                }

                return go;
            }
        }

    }
}


