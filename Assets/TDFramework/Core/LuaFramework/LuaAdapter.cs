using System;
using System.Collections;
using System.Collections.Generic;
using TDFramework.Resource;
using UnityEngine;
using XLua;

namespace LuaFramework
{
    public class LuaAdapter
    {
        
        private static LuaAdapter Instance = new LuaAdapter();

        // lua环境
        private LuaEnv _luaEnv = null;


        private LuaAdapter()
        {
            
        }

        public static LuaAdapter GetInstance()
        {
            return Instance;
        }


        /// <summary>
        /// 获取 Lua 环境
        /// </summary>
        /// <returns></returns>
        public LuaEnv GetLuaEnv()
        {
            if (_luaEnv == null)
            {
                Debug.LogError("Lua Env 初始化失败");
            }

            return _luaEnv;
        }

        
        /// <summary>
        /// 销毁 Lua 环境
        /// </summary>
        public void Dispose()
        {
            if (_luaEnv != null)
            {
                try
                {
                    _luaEnv.Dispose();
                    _luaEnv = null;
                }
                catch (Exception ex)
                {
                    string msg = string.Format("xLua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                    Debug.LogError(msg, null);
                }
            }
        }

        
        /// <summary>
        /// 执行 Lua 代码
        /// </summary>
        /// <param name="chunk"></param>`
        /// <param name="chunkName"></param>
        /// <param name="env"></param>
        public void DoString(string chunk, string chunkName = "chunk", LuaTable env = null)
        {
            if (_luaEnv != null)
            {
                try
                {
                    _luaEnv.DoString(chunk, chunkName, env);
                }
                catch (Exception ex)
                {
                    string msg = string.Format("Lua Dostring exception : {0}\n {1}", ex.Message, ex.StackTrace);
                    Debug.LogError(msg);
                }
            }
            else
            {
                Debug.LogError("Lua Env 为空");
            }
        }

        /// <summary>
        /// 执行 Lua 代码
        /// </summary>
        /// <param name="luaScriptName"></param>
        public void LoadScript(string luaScriptName)
        {
            DoString(string.Format("require('{0}')", luaScriptName));
        }
        

        /// <summary>
        /// 重新执行 Lua 代码
        /// </summary>
        /// <param name="luaScriptName"></param>
        public void ReloadScript(string luaScriptName)
        {
//            DoString(string.Format("package.loaded['{0}'] = nil", luaScriptName));
            
            LoadScript(luaScriptName);
        }


        /// <summary>
        /// 调用 Lua 方法
        /// </summary>
        /// <param name="luaScriptName">lua脚本名字</param>
        /// <param name="luaMethodName">lua方法名</param>
        /// <param name="args">可变参数数组</param>
        /// <returns></returns>
        public object[] CallLuaFunction(string luaScriptName, string luaMethodName, params object[] args)
        {
            LuaTable luaTable = _luaEnv.Global.Get<LuaTable>(luaScriptName);
            LuaFunction luaFunction = luaTable.Get<LuaFunction>(luaMethodName);
            return luaFunction.Call(args);
        }


        
        
        
        
        
    }

}


