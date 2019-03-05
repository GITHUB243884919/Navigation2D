using UnityEngine;
using LuaInterface;
using System.IO;
using System.Text;

namespace UFrame.ToLua
{
    /// <summary>
    /// 接管Tolua的读取lua，从UFrame的资源管理模块读取lua
    /// beZip 无效
    /// </summary>
    public class LuaLoader : LuaFileUtils
    {
        public LuaLoader()
        {
            instance = this;
            beZip = false;
        }

        public override byte[] ReadFile(string fileName)
        {
            if (!fileName.EndsWith(".lua"))
            {
                fileName += ".lua";
            }
#if !RES_AB
            string path = FindFile(fileName);
            byte[] str = null;

            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
#if !UNITY_WEBPLAYER
                str = File.ReadAllBytes(path);
#else
                    throw new LuaException("can't run in web platform, please switch to other platform");
#endif
            }

            return str;

#else
            fileName += ".bytes";
            var getter = ResHelper.LoadAsset(fileName);
            var res = getter.Get<TextAsset>(ResHelper.GetPubAssetGetterGo());
            return res.bytes;
#endif
        }

    }
}

