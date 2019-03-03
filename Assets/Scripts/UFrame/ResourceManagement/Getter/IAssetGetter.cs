using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFrame.ResourceManagement
{
    public interface IAssetGetter
    {
        void SetAssetHolder(AssetHolder assetholder);

        //void Release(GameObject go);
    }

}

