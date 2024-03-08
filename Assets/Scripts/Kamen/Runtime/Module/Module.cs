/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月25日 星期三 12:10:52
* @Modify:
*/
using System;

namespace KamenGameFramewrok
{
    /// <summary>
    /// 模块基类
    /// </summary>
    public class Module : IModule
    {
        public bool IsInitialized = false;
        private Action mCompleteAction;
        protected int LoadedCount = 0;
        public virtual bool Initialized()
        {
            return IsInitialized;
        }

        public virtual void BeforeInit()
        {

        }

        public virtual void Init()
        {
            OnInitialized();
        }

        protected void OnInitialized()
        {
            IsInitialized = true;
            mCompleteAction?.Invoke();
        }

        public virtual void WaitInitAsync(Action callBack)
        {
            mCompleteAction += callBack;
            if (IsInitialized)
            {
                callBack?.Invoke();
            }
        }

        public virtual void AfterInit()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void FixedUpdate()
        {

        }

        public virtual void BeforeShut()
        {

        }

        public virtual void Shut()
        {
            IsInitialized = false;
        }
    }
}