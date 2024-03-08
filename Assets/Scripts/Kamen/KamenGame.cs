/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月27日 星期五 23:10:07
* @Modify:
*/

using System.IO;
using cfg;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Serialization;

namespace KamenGameFramewrok
{
    /// <summary>
    /// 游戏系统调用接口
    /// </summary>
    public class KamenGame : MonoSingleton<KamenGame>
    {
        /// <summary>
        /// 资源模块
        /// </summary>
        public IAssetModule AssetModule => PluginManager.Instance.GetModule<IAssetModule>();
        
        /// <summary>
        /// UI管理器
        /// </summary>
        public IUIManager UIManager => PluginManager.Instance.GetModule<IUIManager>();
        
        /// <summary>
        /// 实体管理器
        /// </summary>
        public IEntityManager EntityManager =>PluginManager.Instance.GetModule<IEntityManager>();

        /// <summary>
        /// 输入管理器
        /// </summary>
        public IInputManager InputManager => PluginManager.Instance.GetModule<IInputManager>();
        
        public Tables Config { get; private set; }
        
        /// <summary>
        /// 本地模式开关
        /// </summary>
        [FormerlySerializedAs("IsUseLocalUIAssets")] public bool isUseLocalUIAssets = true;
        
        protected override void AwakeEx()
        {
            base.AwakeEx();
            string gameConfDir = Application.streamingAssetsPath + "/Config"; //"<outputDataDir>"; // 替换为gen.bat中outputDataDir指向的目录
            Config = new cfg.Tables(file => JSON.Parse(File.ReadAllText($"{gameConfDir}/{file}.json")));
        }
        
        public GraphicLevel GetGraphicLevel()
        {
            return (GraphicLevel)PlayerPrefs.GetInt("GraphicLevel", 1);
        }
    }
    
    public enum GraphicLevel
    {
        NONE,
        LOW,
        MEDIUM,
        HIGH
    }
}
