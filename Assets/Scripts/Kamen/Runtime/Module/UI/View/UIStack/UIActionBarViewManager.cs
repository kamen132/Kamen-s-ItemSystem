/*
* @Author: Kamen
* @Description:
* @Date: 2023年11月06日 星期一 11:11:15
* @Modify:
*/
namespace KamenGameFramewrok
{
    public class UIActionBarViewManager : UIStackManager
    {
        public UIActionBarViewManager() : base(1, true)
        {

        }

        public int ActionBarViewCount => mStackViews.Count - 1;

        public override void PushView(UIView ui)
        {
            foreach (var uiView in mStackViews)
            {
                uiView.Hide();
            }

            base.PushView(ui);
        }

        private UIActionProvider mCurUIActionProvider = null;
        /// <summary>
        /// 获取ActionBar
        /// </summary>
        /// <typeparam name="KVIEW"></typeparam>
        /// <returns></returns>
        public KVIEW GetActionBarView<KVIEW>() where KVIEW : UIActionBarBaseView
        {
            for (int i = 0; i < mStackViews.Count; i++)
            {
                if (mStackViews[i] is KVIEW)
                {
                    return mStackViews[i] as KVIEW;
                }
            }

            return null;
        }
    }
}