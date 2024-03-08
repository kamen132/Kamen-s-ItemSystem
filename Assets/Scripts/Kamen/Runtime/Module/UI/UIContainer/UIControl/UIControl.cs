/*
* @Author: Kamen
* @Description:
* @Date: 2023年11月06日 星期一 11:11:33
* @Modify:
*/
using UnityEngine;

namespace KamenGameFramewrok
{
    /// <summary>
    /// UI控件
    /// </summary>
    public abstract class UIControl : UIObject
    {
        public UIControl(GameObject root)
        {
            this.Root = root;
            Initialize();
        }

        protected override void OnInitialize()
        {
            // 去掉执行base.OnInitialize()  Control不进行其他操作了
            // 主要不想执行OnLocalize();
        }
    } 
}