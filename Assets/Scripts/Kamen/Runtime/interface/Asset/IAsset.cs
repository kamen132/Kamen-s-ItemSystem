/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月25日 星期三 18:10:47
* @Modify:
*/

using UnityEngine.SceneManagement;

namespace KamenGameFramewrok
{
    /// <summary>
    /// 资源载体
    /// </summary>
    public interface IAsset
    {
        /// <summary>
        /// 资源名
        /// </summary>
        /// <returns></returns>
        string AssetName();

        /// <summary>
        /// 资源
        /// </summary>
        /// <returns></returns>
        UnityEngine.Object Assets();

        /// <summary>
        /// 资源所在场景
        /// </summary>
        /// <returns></returns>
        Scene Scene();

        /// <summary>
        /// 卸载场景资源
        /// </summary>
        void UnLoadScene();

        /// <summary>
        /// 附加资源
        /// </summary>
        /// <param name="obj"></param>
        void Attack(UnityEngine.Object obj);

        /// <summary>
        /// 卸载资源
        /// </summary>
        void Release();

        /// <summary>
        /// 加载资源
        /// </summary>
        void Retain();

        /// <summary>
        /// 资源进程
        /// </summary>
        /// <returns></returns>
        int Process();
    }
}