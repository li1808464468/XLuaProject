﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;
using LuaFramework;
using TDFramework.Resource;
using XLua.LuaDLL;

namespace TDFramework
{
    [System.Serializable]
    public class Injection
    {
        public string name;
        public GameObject value;
    }
    
    [LuaCallCSharp]
    public class TDLuaBehaviour : MonoBehaviour
    {
        public string LuaName;
        private TextAsset luaScript;
        public Injection[] injections;

        internal static LuaEnv luaEnv;
        internal static float lastGCTime = 0;
        internal const float GCInterval = 1;//1 second 

        private Action luaStart;
        private Action luaUpdate;
        private Action luaOnDestroy;
        private Action luaOnEnable;
        private Action luaOnDisable;

        public LuaTable scriptEnv;

        void Awake()
        {
            luaEnv = LuaManager.Instance.GetLuaEnv(); //all lua behaviour shared one luaenv only!
            scriptEnv = luaEnv.NewTable();

            // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();

            scriptEnv.Set("self", this);
            foreach (var injection in injections)
            {
                scriptEnv.Set(injection.name, injection.value);
            }
            LuaManager.Instance.LoadScript(LuaName, LuaName,scriptEnv);

            // 通过映射将lua方法和Action 绑定
            Action luaAwake = scriptEnv.Get<Action>("awake");
            scriptEnv.Get("start", out luaStart);
            scriptEnv.Get("update", out luaUpdate);
            scriptEnv.Get("ondestroy", out luaOnDestroy);
            scriptEnv.Get("onenable", out luaOnEnable);
            scriptEnv.Get("ondisable", out luaOnDisable);


            if (luaAwake != null)
            {
                luaAwake();
            }
        }

        // Use this for initialization
        void Start()
        {
            if (luaStart != null)
            {
                luaStart();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (luaUpdate != null)
            {
                luaUpdate();
            }
            if (Time.time - TDLuaBehaviour.lastGCTime > GCInterval)
            {
                luaEnv.Tick();
                TDLuaBehaviour.lastGCTime = Time.time;
            }
        }

        void OnDestroy()
        {
            if (luaOnDestroy != null)
            {
                luaOnDestroy();
            }
            luaOnDestroy = null;
            luaUpdate = null;
            luaStart = null;
            scriptEnv.Dispose();
            injections = null;
        }

        private void OnEnable()
        {
            luaOnEnable?.Invoke();
        }

        private void OnDisable()
        {
            luaOnDisable?.Invoke();
        }
    }


}
