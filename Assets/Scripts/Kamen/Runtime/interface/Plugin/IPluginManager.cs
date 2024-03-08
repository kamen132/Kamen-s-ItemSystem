/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月25日 星期三 12:10:52
* @Modify:
*/
namespace KamenGameFramewrok
{
    /// <summary>
    /// 插件管理器接口
    /// </summary>
    public interface IPluginManager : IModule
    {
        /// <summary>
        /// 获取模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetModule<T>() where T : IModule;
        
        /// <summary>
        /// 注册插件
        /// </summary>
        /// <param name="plugin"></param>
        void Registered(IPlugin plugin);
        
        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="module"></param>
        void AddModule(string moduleName, IModule module);
        
        /// <summary>
        /// 移除模块
        /// </summary>
        /// <param name="moduleName"></param>
        void RemoveModule(string moduleName);
    }
}