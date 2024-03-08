/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月25日 星期三 12:10:13
* @Modify:
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KamenGameFramewrok
{
    /// <summary>
    /// 框架启动脚本 依赖Mono
    /// </summary>
    public class GameApp : MonoBehaviour
    {
        protected IPluginManager PluginManager;
        private float mStartTime;
        private LocalResourceConfig mResourceConfig;
        [SerializeField] public PlayerInput PlayerInput;

        public static GameApp Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// 游戏模块节点
        /// </summary>
        private Dictionary<AppRootType, GameObject> mAppRootMap = new Dictionary<AppRootType, GameObject>();

        protected virtual void OnInitialized()
        {
            foreach (AppRootType rootType in Enum.GetValues(typeof(AppRootType)))
            {
                GetAppRoot(rootType);
            }
        }

        /// <summary>
        /// 添加插件
        /// </summary>
        protected virtual void OnAddPlugin()
        {

        }

        /// <summary>
        /// 所有系统初始化完成
        /// </summary>
        protected virtual void OnAfterInitialized()
        {

        }

        /// <summary>
        /// 获取模块节点
        /// </summary>
        /// <param name="rootType"></param>
        /// <returns></returns>
        public GameObject GetAppRoot(AppRootType rootType)
        {
            if (!mAppRootMap.TryGetValue(rootType, out var targetRoot))
            {
                targetRoot = mResourceConfig.GetAppRoot(rootType);
                targetRoot.name = targetRoot.name.Replace("(Clone)", "");
                targetRoot.transform.SetParent(transform.GetChild(0));
                mAppRootMap.Add(rootType, targetRoot);
            }

            return targetRoot;
        }

        private void Start()
        {
            mStartTime = Time.realtimeSinceStartup;
            KLogger.Log("启动时间" + mStartTime, Color.green);
            PluginManager = KamenGameFramewrok.PluginManager.Instance;
            mResourceConfig = Resources.Load<LocalResourceConfig>("Game/Framework/LocalResources");
            OnAddPlugin();
            Init();
        }

        void Update()
        {
            PluginManager?.Update();
        }

        void FixedUpdate()
        {
            PluginManager?.FixedUpdate();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            PluginManager.BeforeInit();
            PluginManager.Init();
            PluginManager.WaitInitAsync(() =>
            {
                OnInitialized();
                PluginManager.AfterInit();
                OnAfterInitialized();
                KLogger.Log($"初始化耗时: {Time.realtimeSinceStartup - mStartTime} ms");
            });
        }

        private void OnDestroy()
        {
            foreach (var root in mAppRootMap)
            {
                Destroy(root.Value);
            }

            mAppRootMap.Clear();
            PluginManager?.BeforeShut();
            PluginManager?.Shut();
            PluginManager = null;
        }

        protected virtual void OnApplicationQuit()
        {
            if (PluginManager == null) return;
            PluginManager.BeforeShut();
            PluginManager.Shut();
            PluginManager = null;
        }
    }
}