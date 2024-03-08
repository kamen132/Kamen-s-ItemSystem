/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月25日 星期三 12:10:00
* @Modify:
*/
namespace KamenGameFramewrok
{
    /// <summary>
    /// 插件接口
    /// </summary>
    public interface IPlugin : IModule
    {
        /// <summary>
        /// 获取组件名
        /// </summary>
        /// <returns></returns>
        string GetPluginName();

        /// <summary>
        /// 装载组件
        /// </summary>
        void Install();

        /// <summary>
        /// 卸载组件
        /// </summary>
        void Uninstall();
    }
}