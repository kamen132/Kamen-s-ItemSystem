/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月27日 星期五 16:10:04
* @Modify:
*/
using UnityEngine;

namespace KamenGameFramewrok
{
    /// <summary>
    /// UI组件接口
    /// </summary>
    public interface IUIObject
    {
        /// <summary>
        /// UI是否存在
        /// </summary>
        bool IsAlive { get; }
        /// <summary>
        /// 是否可见
        /// </summary>
        bool IsVisible { get; }
        /// <summary>
        /// UI根节点
        /// </summary>
        GameObject Root { get; }
        /// <summary>
        /// 根节点RectTransForm
        /// </summary>
        RectTransform UITransform { get; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        void Initialize();
        /// <summary>
        /// 销毁
        /// </summary>
        void Destroy();
        /// <summary>
        /// 弹出
        /// </summary>
        void Show();
        /// <summary>
        /// 隐藏
        /// </summary>
        void Hide();
    }
}