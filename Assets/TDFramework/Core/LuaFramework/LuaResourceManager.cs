using System.Collections;
using System.Collections.Generic;
using TDFramework.Resource;
using UnityEngine;

public static class LuaResourceManager
{
    public static GameObject GetGameObject(string name)
    {
        return ResManager.Instance.GetGameObject(name);
        
        
    }
}
