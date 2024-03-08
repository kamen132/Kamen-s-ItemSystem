/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月27日 星期五 23:10:15
* @Modify:
*/
using UnityEngine;
namespace KamenGameFramewrok
{
    public interface IUIManager : IModule                
    {
        void CloseAllView();
        void OnViewClosed(UIView view);
        KVIEW CreateView<KVIEW>(UILayer layer, bool isFullScreen) where KVIEW : UIView, new();
        KVIEW CreateView<KVIEW>(GameObject parentContainer, bool isFullScreen) where KVIEW : UIView, new();
        UICustomAlert<CONTENT_ITEM> CreateCustomAlert<CONTENT_ITEM>(UILayer layer,bool isFullScreen) where CONTENT_ITEM : UIItem, new();
        KVIEW CreateActionBarView<KVIEW>(UILayer layer = UILayer.ActionBarView, bool isFullScreen = false) where KVIEW : UIActionBarBaseView, new();
        void ReorderCanvas(int index, UILayer maxLayer);
        bool AutoResume { get; }
    }
}
