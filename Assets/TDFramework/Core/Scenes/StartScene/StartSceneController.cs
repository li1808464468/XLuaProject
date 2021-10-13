using System.Collections;
using System.Collections.Generic;
using LuaFramework;
using TDFramework;
using TDFramework.Resource;
using TDFramework.Tool;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using XLua;

public class StartSceneController : MonoBehaviour
{
    private const string mustLabel = "must";
        public UISlider ProgressSlider;
        public UILabel totalSize;
        public UIButton startButton;
        public UILabel progressLabel;
        private AsyncOperationHandle downloadHandle;
        private bool downloadStart = false;
        
        // Start is called before the first frame update
        void Start()
        {
//            Caching.ClearCache();
//            Addressables.ClearResourceLocators();
            
            ProgressSlider.transform.gameObject.SetActive(false);
            startButton.transform.gameObject.SetActive(false);
            StartCoroutine(ChekAssets());
        }
    
        // Update is called once per frame
        void Update()
        {
            if (!downloadStart)
            {
                return;
            }

            if (downloadHandle.OperationException != null)
            {
                Debug.LogError(downloadHandle.OperationException.Message);   
            }


            Debug.LogWarning("Complete = " + downloadHandle.PercentComplete);
            progressLabel.text = TDTools.FloatRetainN(downloadHandle.PercentComplete * 100) + "%";
            ProgressSlider.value = downloadHandle.PercentComplete;
        }
        
    
        /// <summary>
        /// 获取更新资源大小
        /// </summary>
        /// <returns></returns>
        IEnumerator ChekAssets()
        {
            var loadSize = ResManager.Instance.GetDownloadSize(mustLabel);
            yield return loadSize;
            if (loadSize.Result == 0)
            {
                OnCompleted();
                yield break;
            }
            
    
            /// TODO 如果数据小于1M提示更新大小为0 需要修改下
            totalSize.text = "Downloader Size: " + TDTools.FloatRetainN(loadSize.Result / 1000000.0f)  + "M";
            startButton.gameObject.SetActive(true);
        }
        
        /// <summary>
        /// 点击按钮开始下载资源
        /// </summary>
        public void  StartDownloader()
        {
            ProgressSlider.gameObject.SetActive(true);
            StartCoroutine(StartDownloaderResources());
            
        }
    
    
        /// <summary>
        /// 开始下载资源
        /// </summary>
        /// <returns></returns>
        IEnumerator StartDownloaderResources()
        {
            downloadStart = true;

            downloadHandle = ResManager.Instance.DownloadDependenciesAsync(mustLabel, true);
            downloadHandle.Completed += OnCompleted;
            yield return downloadHandle;
    
        }
    
    
        /// <summary>
        /// 下载资源回调
        /// </summary>
        /// <param name="o"></param>
        private void OnCompleted(AsyncOperationHandle o)
        {
            downloadStart = false;
            OnCompleted();
        }
    
    
        /// <summary>
        /// 资源更新完毕方法
        /// </summary>
        void OnCompleted()
        {
            ResManager.Instance.LoadAllLuaFils();


//            LuaManager.Instance.LuaGameStart();
//            StartGame();

        }

        public void StartGame()
        {
            // 启动Lua 环境
            LuaManager.Instance.LuaGameStart();
        }
    
        [CSharpCallLua]
        public void EntranceGameScene()
        {
            Debug.Log("切换场景");
            SceneManager.LoadSceneAsync("GameScene", new LoadSceneParameters(LoadSceneMode.Single));
            
        }
}
