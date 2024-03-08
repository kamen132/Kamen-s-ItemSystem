using System;
using System.Collections.Generic;
using KamenGameFramewrok;
using UnityEngine;

namespace Game.UI.Code
{
    public partial class UIAlertView : UIView
    {
        readonly Dictionary<string, UIItem> dictUIItem = new Dictionary<string, UIItem>();
        bool mBClickBgClose = true;
        private float mOriginalColorAlpha;
        public bool CanKeyBack { get; private set; } = true;
        private Action OnClickBgAction { get; set; } = null;

        protected override void OnInitializeComponent()
        {
            base.OnInitializeComponent();
            AddButton(_BG, OnClickedBGClose);
            mOriginalColorAlpha = mBGImg.color.a;
        }


        public void SetBgAlpha(int value)
        {
            var color = mBGImg.color;
            color.a = Mathf.Clamp01(value / 255.0f);
            mBGImg.color = color;
        }


        protected GameObject GetMatter()
        {
            return _Matter;
        }

        private void OnClickedBGClose(UIButton btn, object ex)
        {
            if (OnClickBgAction != null)
            {
                OnClickBgAction?.Invoke();
            }

            if (mBClickBgClose)
            {
                Close();
            }
        }

        public void SetClickBgAction(Action action)
        {
            OnClickBgAction = action;
        }

        public void SetBgAction(bool enable = true)
        {
            if (!enable)
            {
                SetClickBgClose(enable);
            }

            _BG.SetActive(enable);
        }

        public void SetClickBgClose(bool enable = true)
        {
            mBClickBgClose = enable;
            if (mBClickBgClose)
            {
                OnSpecialCloseType(EnumViewCloseType.ClickBg);
            }
            else
            {
                OffSpecialCloseType(EnumViewCloseType.ClickBg);
            }
        }

        protected override void OnHide()
        {
            CanKeyBack = true;
            base.OnHide();
        }

        public void SetKeyBackEnable(bool enable)
        {
            CanKeyBack = enable;
        }

        public void SetBgIsBlack(bool isBlack)
        {
            var color = mBGImg.color;
            if (isBlack)
            {
                color.a = mOriginalColorAlpha;
                mBGImg.color = color;
            }
            else
            {
                color.a = 0;
                mBGImg.color = color;
            }
        }

        public void ShowItem(bool clickBGClose, UIItem item)
        {
            mBClickBgClose = clickBGClose;
            SetName(item.GetType().Name);
            SetContentItem(item);
        }

        protected void SetName(string _str)
        {
            Root.name = _str;
        }

        private void SetContentItem(UIItem pItem)
        {
            pItem?.SetParent(_Matter);
            pItem?.Show();
        }

        private void Reset()
        {
            foreach (var item in dictUIItem)
            {
                item.Value?.Hide();
            }
        }
    }
}