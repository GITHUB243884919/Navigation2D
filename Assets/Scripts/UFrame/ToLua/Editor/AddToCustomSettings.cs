using UnityEngine;
using System;
using System.Collections.Generic;
using LuaInterface;
using LuaFramework;
using UnityEditor;

using BindType = ToLuaMenu.BindType;
using UnityEngine.UI;
using System.Reflection;

namespace UFrame.ToLua
{
    public class AddToCustomSettings
    {
        public static void AddDelegateType()
        {
            //CustomSettings._DT(typeof(System.Func<int, int>));
        }

        public static void AddBindType()
        {
            //CustomSettings._GT(typeof(LuaMessageCenter)),
        }
    }
}

