/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月25日 星期三 12:10:57
* @Modify:
*/
using System.Collections.Generic;

namespace KamenGameFramewrok
{
    /// <summary>
    /// 系统插件基类
    /// </summary>
    public abstract class Plugin : Module, IPlugin
    {
        /// <summary>
        /// 模块字典-用于查找啊
        /// </summary>
        private readonly Dictionary<string, IModule> mModuleMap = new Dictionary<string, IModule>();

        /// <summary>
        /// 模块字典-用于遍历啊
        /// </summary>
        private readonly List<IModule> mModuleList = new List<IModule>();

        /// <summary>
        /// 模块名
        /// </summary>
        private readonly string mPluginName;

        protected Plugin(string pluginName)
        {
            mPluginName = pluginName;
        }

        /// <summary>
        /// 向插件里添加模块
        /// </summary>
        /// <param name="module"></param>
        /// <typeparam name="T"></typeparam>
        public void AddModule<T>(IModule module)
        {
            string modelName = typeof(T).ToString();
            if (!mModuleMap.TryGetValue(modelName, out var moduleOld))
            {
                mModuleList.Add(module);
                mModuleMap.Add(modelName, module);
                PluginManager.Instance.AddModule(modelName, module);
            }
        }

        /// <summary>
        /// 获取插件名
        /// </summary>
        /// <returns></returns>
        public string GetPluginName()
        {
            return mPluginName;
        }

        /// <summary>
        /// 初始化插件包含的模块
        /// </summary>
        public abstract void AddModule();

        public override void BeforeInit()
        {
            foreach (var module in mModuleList)
            {
                module?.BeforeInit();
            }
        }

        public override void Init()
        {
            if (mModuleMap.Count == 0)
            {
                OnInitialized();
                return;
            }

            foreach (var module in mModuleList)
            {
                if (module != null)
                {
                    module.Init();
                    module.WaitInitAsync(() =>
                    {
                        LoadedCount++;
                        if (LoadedCount == mModuleMap.Count)
                        {
                            OnInitialized();
                        }
                    });
                }
            }
        }

        public override void AfterInit()
        {
            foreach (var module in mModuleList)
            {
                module?.AfterInit();
            }
        }

        public override void Update()
        {
            foreach (var module in mModuleList)
            {
                if (module != null && module.Initialized())
                {
                    module.Update();
                }
            }
        }

        public override void FixedUpdate()
        {
            foreach (var module in mModuleList)
            {
                if (module != null && module.Initialized())
                {
                    module.FixedUpdate();
                }
            }
        }

        public override void BeforeShut()
        {
            foreach (var module in mModuleList)
            {
                module?.BeforeShut();
            }
        }

        public override void Shut()
        {
            foreach (var module in mModuleList)
            {
                module?.Shut();
            }
        }

        public void Install()
        {
            AddModule();
        }

        public void Uninstall()
        {
            foreach (var data in mModuleMap)
            {
                PluginManager.Instance.RemoveModule(data.Key);
            }

            mModuleMap.Clear();
            mModuleList.Clear();
        }
    }
}