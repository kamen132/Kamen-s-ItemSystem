/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月29日 星期日 10:10:30
* @Modify:
*/

using System.Collections.Generic;
using Game.UI.Code;

namespace KamenGameFramewrok
{
    public class UIActionBarBaseView : UIView
    {

        private readonly List<UIAlertView> mAccompanyingAlert = new List<UIAlertView>();
        
        protected UIActionBarBaseView()
        {
            
        }
        
        
        public override void OnPause()
        {   
            //暂停状态
        }

        public override void OnResume()
        {
            
        }

        protected override void OnShow()
        {
            base.OnShow();
            ShowAllAccompanyingAlert();
        }
        protected virtual bool SetMicrosoftLandScapeBorderShow()
        {
            return true;
        }
        

        protected override void OnHide()
        {
            base.OnHide();
            HideAllAccompanyingAlert();
        }

        public override void UnLoad()
        {
            base.UnLoad();
            ClearAccompanyingAlert();
        }
        
        
        #region 伴生Alert逻辑

        public void AddAccompanyingAlert(UIAlertView alert)
        {
            mAccompanyingAlert.Add(alert);
        }

        public void RemoveAccompanyingAlert(UIAlertView alert)
        {
            mAccompanyingAlert.Remove(alert);
        }

        public int GetAccompanyingAlertCount()
        {
            return mAccompanyingAlert.Count;
        }

        private void ClearAccompanyingAlert()
        {
            for (int i = 0; i < mAccompanyingAlert.Count; i++)
            {
                mAccompanyingAlert[i].Destroy();
            }
            mAccompanyingAlert.Clear();
        }

        private void HideAllAccompanyingAlert()
        {
            for (int i = 0; i < mAccompanyingAlert.Count; i++)
            {
                mAccompanyingAlert[i].Hide();
            }
        }
        private void ShowAllAccompanyingAlert()
        {
            for (int i = 0; i < mAccompanyingAlert.Count; i++)
            {
                mAccompanyingAlert[i].Show();
            }
        }
        #endregion
    }
}
