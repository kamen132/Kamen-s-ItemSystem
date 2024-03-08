/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月25日 星期三 12:10:06
* @Modify:
*/

using System;
using System.Collections.Generic;

namespace KamenGameFramewrok
{
    public class PluginManager : MonoSingleton<PluginManager>, IPluginManager
    {
        /// <summary>
        /// 系统初始化时间
        /// </summary>
        public Int64 InitTime { get; private set; }
        /// <summary>
        /// 系统现在时间
        /// </summary>
        public Int64 NowTime  { get; private set; }
        
        private readonly Dictionary<string, IPlugin> mPluginMap = new Dictionary<string, IPlugin>();
        private readonly Dictionary<string, IModule> mModuleMap = new Dictionary<string, IModule>();
        private bool mIsInitialized;
        private int mLoadedCount;
        private Action mCompleteAction;

        public T GetModule<T>() where T : IModule
        {
            IModule module = FindModule(typeof(T).ToString());
            return (T) module;
        }

        public IModule FindModule(string moduleName)
        {
            if (mModuleMap.TryGetValue(moduleName, out var module))
            {
                return module;
            }

            return new Module();
        }

        public void Registered(IPlugin plugin)
        {
            mPluginMap.Add(plugin.GetPluginName(), plugin);
            plugin.Install();
        }

        public void AddModule(string moduleName, IModule module)
        {
            if (!mModuleMap.TryGetValue(moduleName, out var moduleOld))
            {
                mModuleMap.Add(moduleName, module);
            }
        }

        public void RemoveModule(string moduleName)
        {
            if (mModuleMap.TryGetValue(moduleName, out var moduleOld))
            {
                mModuleMap.Remove(moduleName);
            }
        }

        public bool Initialized()
        {
            return mIsInitialized;
        }

        public void BeforeInit()
        {
            foreach (IPlugin plugin in mPluginMap.Values)
            {
                plugin?.BeforeInit();
            }
        }

        private void OnInitialized()
        {
            mIsInitialized = true;
            mCompleteAction?.Invoke();
        }

        public void Init()
        {
            InitTime = DateTime.Now.Ticks / 10000;
            foreach (IPlugin plugin in mPluginMap.Values)
            {
                if (plugin != null)
                {
                    plugin.Init();
                    plugin.WaitInitAsync(() =>
                    {
                        mLoadedCount++;
                        if (mLoadedCount == mPluginMap.Count)
                        {
                            OnInitialized();
                        }
                    });
                }
            }
        }

        public void WaitInitAsync(Action complete)
        {
            mCompleteAction += complete;
            if (mIsInitialized)
            {
                complete();
            }
        }

        public void AfterInit()
        {
            foreach (IPlugin plugin in mPluginMap.Values)
            {
                plugin?.AfterInit();
            }
        }

        public void Update()
        {
            NowTime = DateTime.Now.Ticks / 10000;
            foreach (IPlugin plugin in mPluginMap.Values)
            {
                plugin?.Update();
            }
        }

        public void FixedUpdate()
        {
            foreach (IPlugin plugin in mPluginMap.Values)
            {
                plugin?.FixedUpdate();
            }
        }

        public void BeforeShut()
        {
            foreach (IPlugin plugin in mPluginMap.Values)
            {
                plugin?.BeforeShut();
            }
        }

        public void Shut()
        {
            foreach (IPlugin plugin in mPluginMap.Values)
            {
                plugin?.Shut();
            }

            mPluginMap.Clear();
            mPluginMap.Clear();
        }
    }
}