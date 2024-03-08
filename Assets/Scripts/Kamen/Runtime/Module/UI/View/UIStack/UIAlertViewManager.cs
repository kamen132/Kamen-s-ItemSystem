/*
* @Author: Kamen
* @Description:
* @Date: 2023年11月06日 星期一 11:11:24
* @Modify:
*/
using Game.UI.Code;

namespace KamenGameFramewrok
{
    public class UIAlertViewManager : UIStackManager
    {
        public UIAlertViewManager():base(0, false)
        {

        }
        protected override void OnResumeTopView()
        {
            if (mStackViews.Count > 0)
            {
                int top = mStackViews.Count - 1;
                UIAlertView ui = mStackViews[top] as UIAlertView;
                if (ui.IsVisible)
                {
                    ui.OnResume();
                }
            }
        }
    }
}