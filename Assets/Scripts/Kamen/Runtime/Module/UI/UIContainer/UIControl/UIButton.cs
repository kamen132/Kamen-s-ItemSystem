/*
* @Author: Kamen
* @Description:
* @Date: 2023年11月06日 星期一 11:11:25
* @Modify:
*/
using System;
using UnityEngine;
using UnityEngine.UI;

namespace KamenGameFramewrok
{
    /// <summary>
    /// 按钮封装控件
    /// </summary>
    public class UIButton : UIControl
    {
        private readonly Text mButtonText;
        private readonly Button mButton;
        /// <summary>
        /// 按钮点击事件
        /// </summary>
        private Action<UIButton, object> mOnClicked;
        /// <summary>
        /// 附加参数
        /// </summary>
        public object Extra { get; set; }
        
        /// <summary>
        /// 按钮的声音
        /// </summary>
        private string mButtonAudioName = "click_button";

        /// <summary>
        /// 新建UIButton
        /// </summary>
        public UIButton(Button button, Action<UIButton, object> call = null, Action<UIButton> afterCreate = null) : base(button.gameObject)
        {
            this.mButton = button;
            this.mOnClicked = call;
            mButton.onClick.AddListener(OnButtonClicked);

            this.mButtonText = button.GetComponentInChildren<Text>();
            
            CheckGraphic();

            afterCreate?.Invoke(this);
        }

        /// <summary>
        /// 新建UIButton
        /// </summary>
        public UIButton(GameObject gameObject, Action<UIButton, object> call = null, Action<UIButton> afterCreate = null) : base(gameObject)
        {
            this.mButton = gameObject.GetComponent<Button>();
            this.mOnClicked = call;
            
            if (null == mButton)
            {
                mButton = gameObject.AddComponent<Button>();
            }

            mButton.onClick.AddListener(OnButtonClicked);

            this.mButtonText = mButton.GetComponentInChildren<Text>();

            CheckGraphic();

            afterCreate?.Invoke(this);
        }
        
        public void SetTargetGraphic(Graphic graphic)
        {
            mButton.targetGraphic = graphic;
        }


        private void CheckGraphic()
        {
            Graphic graphic = mButton.GetComponent<Graphic>();

            if (null == graphic)
            {
                graphic = mButton.gameObject.AddComponent<NoDrawcallRect>();
                graphic.raycastTarget = true;
            }
        }

        /// <summary>
        /// 设置是否启用按钮
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                if (mButton != null)
                {
                    return mButton.interactable;
                }
                return false;
            }
            set
            {
                if (mButton != null)
                {
                    mButton.interactable = value;

                    if (value == true)
                    {
                        Graphic graphic = mButton.GetComponent<Graphic>();
                        // 保证可以点击
                        if (null != graphic)
                        {
                            graphic.raycastTarget = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 触发被点击事件
        /// </summary>
        public void ClickedTrigger()
        {
            OnButtonClicked();
        }

        /// <summary>
        /// 事件传递
        /// </summary>
        private void OnButtonClicked()
        {
            PlayClickAudio();
            mOnClicked?.Invoke(this, this.Extra);
        }
        
        //为了只查找一次
        private bool mIsFindUiPlayAudio = true;
        private void PlayClickAudio()
        {
            //if(Root == null)
            //    return;
            
        }

        public void AddListener(Action<UIButton, object> call)
        {
            this.mOnClicked -= call;
            this.mOnClicked += call;
        }
        
        public void ReplaceListener(Action<UIButton, object> call)
        {
            this.mOnClicked = call;
        }

        public void RemoveListener(Action<UIButton, object> call)
        {
            this.mOnClicked -= call;
        }

        public void RemoveAllListeners()
        {
            this.Extra = null;
            this.mOnClicked = null;
        }

        public void SetText(string text)
        {
            if (null != mButtonText)
            {
                mButtonText.text = text;
            }
        }

        public void SetTextColor(Color color)
        {
            if (null != mButtonText)
            {
                mButtonText.color = color;
            }
        }
        
        /// <summary>
        /// 销毁时执行
        /// </summary>
        protected override void OnDestroy()
        {
            this.RemoveAllListeners();
            mButton.onClick.RemoveListener(OnButtonClicked);
        }
    }
}