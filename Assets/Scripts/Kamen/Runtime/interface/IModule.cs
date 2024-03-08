/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月25日 星期三 11:10:06
* @Modify:
*/

using System;

namespace KamenGameFramewrok
{
    /// <summary>
    /// 系统模块接口  外界通过KamenGame访问接口
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// 初始化完成标识
        /// </summary>
        /// <returns></returns>
        bool Initialized();

        /// <summary>
        /// 初始化前
        /// </summary>
        /// <returns></returns>
        void BeforeInit();

        /// <summary>
        /// 初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 等待初始化完成
        /// </summary>
        /// <param name="callBack"></param>
        void WaitInitAsync(Action callBack);

        /// <summary>
        /// 初始化完成之后操作
        /// </summary>
        void AfterInit();

        /// <summary>
        /// 每帧更新
        /// </summary>
        void Update();

        /// <summary>
        /// 每帧更新后
        /// </summary>
        void FixedUpdate();

        /// <summary>
        /// 关闭前
        /// </summary>
        void BeforeShut();

        /// <summary>
        /// 关闭后
        /// </summary>
        void Shut();
    }
}