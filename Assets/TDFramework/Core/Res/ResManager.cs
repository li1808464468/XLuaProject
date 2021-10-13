using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using XLua;

namespace TDFramework.Resource
{

    [LuaCallCSharp]
//    [MonoSingletonPath("[Res]/[Manager]")]
    public class ResManager : MonoSingleton<ResManager>
    {
        private Dictionary<string, TextAsset> _fileCache = new Dictionary<string, TextAsset>();
        private Dictionary<string, GameObject> _objCache = new Dictionary<string, GameObject>();

        public int TestCode = 100;
        private ResManager()
        {
        }


        #region Download 资源

        /// <summary>
        /// Download Assets
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public AsyncOperationHandle DownloadDependenciesAsync(string key, bool autoReleaseHandle = false )
        {
            return Addressables.DownloadDependenciesAsync(key, autoReleaseHandle);
        }


        /// <summary>
        /// 获取需要下载的资源大小
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AsyncOperationHandle<long> GetDownloadSize(string key)
        {
            return Addressables.GetDownloadSizeAsync(key);
        }

        #endregion
        
        

        #region 异步加载资源
        /// <summary>
        ///  异步加载资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="_path">地址或者Label/param>
        /// <param name="_loaded">回调</param>
        public AsyncOperationHandle<T> LoadAssetAsync<T>(string _path, Action<T> _loaded = null)
        {
            var handle = Addressables.LoadAssetAsync<T>(_path);
            handle.Completed += (p) => 
            {
                if (p.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    _loaded?.Invoke(p.Result);
                else
                    throw new Exception("load fail: " + _path);
            };
            return handle;
        }
        #endregion

        #region 异步实例化

        /// <summary>
        /// Async Instance
        /// </summary>
        /// <param name="_path">地址或者Label</param>
        /// <param name="_loaded">回调</param>
        /// <param name="parent">Parent</param>
        /// <param name="是否生成在世界空间">instantiateInWorldSpace</param>
        /// <param name="trackHandle">跟踪节点</param>
        public AsyncOperationHandle<GameObject> InstanceAsync(string _path, Action<GameObject> _loaded = null, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
        {
            var handle = Addressables.InstantiateAsync(_path, parent, instantiateInWorldSpace, trackHandle);
            handle.Completed += (p) =>
            {
                _loaded?.Invoke(p.Result);
            };
            return handle;
        }

        #endregion





        #region  根据图集加载图片
        /// <summary>
        /// AsyncLoadSpriteByAltas
        /// </summary>
        /// <param name="path">地址或者Label</param>
        /// <param name="spriteName">图片名</param>
        /// <param name="_loaded">回调</param>
        public void LoadAsyncSpriteByAltas(string path,string spriteName, Action<Sprite> _loaded = null)
        {
            Addressables.LoadAssetAsync<SpriteAtlas>(path).Completed += (p) =>
            {
                if (p.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    _loaded?.Invoke(p.Result.GetSprite(spriteName));
                else
                    throw new Exception("load fail: " + path);
            };
        }
        #endregion

        #region 根据Label加载所有资源
        /// <summary>
        /// 根据Label加载所有资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="label">Label</param>
        /// <param name="_oneLoaded">加载完一个回调，但是并不知道是哪一个</param>
        /// <param name="completeCallBack">所有加载完回调</param>
        public void LoadAssetsAsyncByLabel<T>(string label,Action<T>_oneLoaded=null,Action<IList<T>>completeCallBack=null)
        {
            Addressables.LoadAssetsAsync<T>(label, p =>
            {
                _oneLoaded?.Invoke(p);
            }).Completed+=p=> 
            {
                if (p.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    completeCallBack?.Invoke(p.Result);
                else
                    throw new Exception("load label: " + label);
            };

        }
        #endregion

        #region 根据Label和Path进行组合标签加载
        /// <summary>
        /// 根据Label和Path进行组合标签加载
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="keys">Label和Path组成的list</param>
        /// <param name="mergeMode">MergeMode 1、默认选择第一个内容进行删选 2、并集 3、交集</param>
        /// <param name="_oneLoaded">加载完一个回调，但是并不知道是哪一个</param>
        /// <param name="completeCallBack">所有加载完回调</param>
        public void LoadAssetsAsyncByLabelOrName<T>(IList<object> keys, Addressables.MergeMode mergeMode=Addressables.MergeMode.Intersection, Action<T> _oneLoaded = null, Action<IList<T>> completeCallBack = null)
        {
            Addressables.LoadAssetsAsync<T>(keys, p =>
            {
                _oneLoaded?.Invoke(p);
            }, mergeMode).Completed += p =>
            {
                if (p.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    completeCallBack?.Invoke(p.Result);
                else
                    throw new Exception("load fail: " + keys);
            };

        }
        #endregion


        #region 卸载资源
        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">资源</param>
        public void ReleaseAsset<T>(T source)
        {
            Addressables.Release<T>(source);
           
        }

        public void ReleaseAsset<T>(AsyncOperationHandle<T> handle)
        {
            Addressables.Release<T>(handle);

        }

        public bool ReleaseInstance(GameObject obj)
        {
           return Addressables.ReleaseInstance(obj);
        }

        public bool ReleaseInstance(AsyncOperationHandle handle)
        {
            return Addressables.ReleaseInstance(handle);
        }

        public bool ReleaseInstance(AsyncOperationHandle<GameObject> handle)
        {
            return Addressables.ReleaseInstance(handle);
        }
        #endregion


//        public void LoadAllLuaFils()
//        {
//            string[] files = new[] {"Config", "GameMain", "Global"};
//
//            int fileCount = files.Length;
//            foreach (var file in files)
//            {
//                LoadAssetAsync<TextAsset>(file + ".lua", asset =>
//                {
//                    if (!_fileCache.ContainsKey(file))
//                    {
//                        _fileCache.Add(file + ".lua", asset);
//                        fileCount--;
//                        if (fileCount == 0)
//                        {
//                            FindObjectOfType<StartSceneController>().StartGame();
//                        }
//                    }
//
//                });
//            }
//            
//            
//        }



        public  TextAsset GetTextAsset(string fileName)
        {
            if (_fileCache.ContainsKey(fileName))
            {
                return _fileCache[fileName];
            }
            else
            {
                var handle = LoadAssetAsync<TextAsset>(fileName);
                handle.WaitForCompletion();

                _fileCache.Add(fileName, handle.Result);

                return _fileCache[fileName];
            }
        }
        
        
        public GameObject GetGameObject(string objName)
        {
            if (_objCache.ContainsKey(objName))
            {
                return _objCache[objName];
            }
            else
            {
                GameObject obj;
                var handle = LoadAssetAsync<GameObject>(objName);
                handle.WaitForCompletion();

                _objCache.Add(objName, handle.Result);

                return _objCache[objName];
            }
        }

        /// <summary>
        /// 自定义lua脚本不支持异步的方式加载，改为进入游戏通过标签"lua"加载所有lua脚本
        /// </summary>
        public void LoadAllLuaFils()
        {
            LoadAssetsAsyncByLabel<TextAsset>("lua", asset =>
            {
                _fileCache.Add(asset.name, asset);

            }, list =>
            {
                FindObjectOfType<StartSceneController>().StartGame();
            });
        }
        

        //#region 同步
     
        //public T LoadAsset<T>(string _path)
        //{
        //    throw new Exception();
        //}


        //public GameObject Instance(string _path)
        //{
        //    throw new Exception();
        //}

        //#endregion
    }
}
