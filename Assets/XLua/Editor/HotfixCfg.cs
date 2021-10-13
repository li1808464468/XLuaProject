using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LuaFramework.Hotfix;
using UnityEngine;
using XLua;

public static class HotfixCfg
{
    
    
//    [Hotfix]
//    public static List<Type> by_field = new List<Type>()
//    {
//        typeof(TestHotfix),
//        typeof(GenericClass<>),
//    };

    [Hotfix]
    public static List<Type> by_property
    {
        get
        {
            return (from type in Assembly.Load("Assembly-CSharp").GetTypes()
                where type.Namespace == "LuaFramework.Hotfix"
                select type).ToList();
        }
    }
    

}