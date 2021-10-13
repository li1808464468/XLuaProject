using System.Collections;
using System.Collections.Generic;
using LuaFramework;
using TDFramework.Resource;
using TDFramework.Tool;
using UnityEditor;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using XLua;


namespace TDFramework
{
    public class LoadingSceneController : Singleton<LoadingSceneController>
    {

        LoadingSceneController()
        {
            Debug.Log("Init LoadingSceneController");
        }



        public void LoadGameScene()
        {
            SceneManager.LoadSceneAsync("GameScene", new LoadSceneParameters(LoadSceneMode.Single));
        }
        
        
        
    
    }

}

