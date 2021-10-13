using System;
using System.Collections;
using System.Collections.Generic;
using TDFramework;
using TDFramework.Resource;
using UnityEngine;
using XLua;

namespace LuaFramework
{
    public class LuaManager : MonoSingleton<LuaManager>
    {

        // Lua 端是否启动
        private bool isGameStart = false;

        public bool IsGameStart => isGameStart;

        #region Lua 环境
        private LuaEnv _luaEnv = null;
        private Dictionary<string, byte[]> luaFileCache = new Dictionary<string, byte[]>();
        #endregion
       


        #region Lua 框架核心脚本及接口
        // lua 入口脚本名字
        private const string luaStartScript = "GameMain";
        // lua 入口方法名
        private const string luaStartFunc = "GameMain:Start()";
        // lua 热补丁脚本
        private const string luaHotfixScript = "GameHotFix";
        // lua 热补丁注册
        private const string luaHotfixStartFunc = "GameHotFix:Start()";
        // lua 热补丁反注册
        private const string luaHotfixStopFunc = "GameHotFix:Stop()";
        // lua 游戏退出时清理
        private const string luaAppQuitFunc = "GameMain:OnApplicationQuit()";
        // lua 游戏退出时清理
        private const string luaAppPauseFunc = "GameMain:OnApplicationPause()";
        #endregion

        public override void OnSingletonInit()
        {
            isGameStart = false;
            _luaEnv = new LuaEnv();
            if (_luaEnv != null)
            {
                _luaEnv.AddLoader(CustomLoader);
            }
            else
            {
                Debug.LogError("Lua Env 创建失败");
            }

            
        }
        
        
        
        /// <summary>
        /// 执行热补丁
        /// </summary>
        /// <param name="restart">是否重启虚拟机</param>
        public void StartHotfix(bool restart = false)
        {
            if (_luaEnv == null)
            {
                return;
            }

            if (restart)
            {
                StopHotfix();
                ReloadScript(luaHotfixScript);
            }
            else
            {
                LoadScript(luaHotfixScript);
            }
            
            DoString(luaHotfixStartFunc);
            
        }
        
        
        /// <summary>
        /// 反注册热补丁
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public void StopHotfix()
        {
            DoString(luaHotfixStopFunc);
        }

        
        /// <summary>
        /// 获取 Lua 环境
        /// </summary>
        /// <returns></returns>
        public LuaEnv GetLuaEnv()
        {
            return _luaEnv;
        }
        
        
        /// <summary>
        /// 调用 Adapter 的 ReloadScript 重新执行 Lua 代码，供外部模块调用
        /// </summary>
        /// <param name="luaScriptName">Lua脚本名称</param>
        /// <returns>
        /// </returns>
        public void ReloadScript(string luaScriptName)
        {
            LoadScript(luaScriptName);
        }
        
        
        /// <summary>
        /// 执行 Lua 代码
        /// </summary>
        /// <param name="luaScriptName"></param>
        public void LoadScript(string luaScriptName,string chunkName = "chunk", LuaTable env = null)
        {
            DoString(string.Format("require('{0}')", luaScriptName), chunkName, env);
        }
        
        

       

        /// <summary>
        /// 启动Lua端游戏逻辑
        /// </summary>
        public void LuaGameStart()
        {
            if (GetLuaEnv() != null)
            {
                LoadScript(luaStartScript);
                DoString(luaStartFunc);
                isGameStart = true;
            }
        }
        
        /// <summary>
        /// 热更反注册
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public void OnDisable()
        {

            
        }
        
        
        /// <summary>
        /// 调用 Adapter 的 SafeDoString 执行 Lua 代码，捕获异常
        /// </summary>
        /// <param name="chunk">Lua代码</param>
        /// <param name="chunkName">发生error时的debug显示信息中使用，指明某某代码块的某行错误</param>
        /// <param name="env">当前代码块</param>
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
        /// 游戏退出时执行Lua端清理逻辑
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            
            if (isGameStart && GetLuaEnv() != null)
            {
//                DoString(luaAppQuitFunc);
            }
        }
        
        
        /// <summary>
        /// 游戏暂停时执行Lua端逻辑
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        private void OnApplicationPause()
        {
            if (isGameStart && GetLuaEnv() != null)
            {
//                DoString(luaAppPauseFunc);
            }
        }
        
        
        
        
        /// <summary>
        /// 自定义Load lua 文件delegate
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private byte[] CustomLoader(ref string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("加载Lua 脚本失败");

                return null;
            }

            fileName += ".lua";



            return ResManager.Instance.GetTextAsset(fileName).bytes;

        }
    }

}

