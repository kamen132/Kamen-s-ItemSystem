/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月29日 星期日 00:10:37
* @Modify:
*/

using System;
using UnityEngine;
namespace KamenGameFramewrok
{
    public class UIView : UIContainer
    {
        public bool IsFullScreen { get; private set; }
        public UILayer Layer { get; private set;}
        public void Init(UILayer layer,GameObject parent,bool isFull)
        {
            Layer = layer;
            IsFullScreen = isFull;
            Initialize();
            SetParent(parent);
            OnLoad();
        }
        
        public virtual void OnPause()
        {   
            //暂停状态
        }
        
        public virtual void OnResume()
        {
            //回到栈顶时
            
        }
        public virtual void OnLoad()
        {
            
        }

        public virtual void UnLoad()
        {
            
        }
        public void Close()
        {
            Destroy();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnLoad();
        }
        
        
        // 特殊关闭类型
        [Flags]
        public enum EnumViewCloseType
        {
            None = 1 << 0,
            ClickBg = 1 << 1,
            Esc = 1 << 2,
            All = ClickBg | Esc,
        }
        
        private EnumViewCloseType mSpecialCloseTypes = EnumViewCloseType.All;

        public void SetSpecialCloseType(EnumViewCloseType type)
        {
            mSpecialCloseTypes = type;
        }

        public void OnSpecialCloseType(EnumViewCloseType types)
        {
            mSpecialCloseTypes |= types;
        }

        public void OffSpecialCloseType(EnumViewCloseType types)
        {
            mSpecialCloseTypes &= ~types;
        }
        public bool CanSpecialClose(EnumViewCloseType type)
        {
            return (mSpecialCloseTypes & type) == type;
        }
        
    }
}
