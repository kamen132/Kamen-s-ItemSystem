/*
* @Author: Kamen
* @Description:
* @Date: 2023年11月06日 星期一 11:11:38
* @Modify:
*/
using Game.UI.Code;

namespace KamenGameFramewrok
{
    /// <summary>
    /// UIItem适配器
    /// </summary>
    /// <typeparam name="CONTENT_ITEM"></typeparam>
    public class UICustomAlert<CONTENT_ITEM> : UIAlertView where CONTENT_ITEM : UIItem, new()
    {
        public CONTENT_ITEM Content { get; private set; }
        public UIActionBarBaseView AccompanyingView { get; private set; }

        protected override void OnInitializeComponent()
        {
            base.OnInitializeComponent();
            Content = AddSubItemToNode<CONTENT_ITEM>(GetMatter());
            SetName(Content.GetType().Name);
        }

        public UICustomAlert()
        {

        }

        public UICustomAlert(UIActionBarBaseView accompanyingView)
        {
            AccompanyingView = accompanyingView;
            //todo Kamen
            AccompanyingView?.AddAccompanyingAlert(this);
        }

        public override void UnLoad()
        {
            base.UnLoad();
            //todo Kamen
            AccompanyingView?.RemoveAccompanyingAlert(this);
        }

        public override void OnResume()
        {
            base.OnResume();
            Content?.OnReset();
        }
    }
}
