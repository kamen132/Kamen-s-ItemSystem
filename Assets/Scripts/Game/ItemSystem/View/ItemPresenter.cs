/**
 * 通用物品、英雄小卡，材料，经验卡等展示组件
 * Created by anychen on 2019年1月16日
 */


using System;
using Game.ItemSystem;
using Game.ItemSystem.View;
using KamenGameFramewrok;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.Code
{
    public partial class ItemPresenter : UIItem
    {
        private UGUIEventLongPress mEventLongPress; //ui长按\点击
        private Action<ItemPresenter> mOnItemClick; //点击回调
        private Action<ItemPresenter, PointerEventData, bool> mOnLongPressListener; //长按回调

        private bool mIgnoreEmptyClick;
        
        private IPresenterContextView mContextView;
        private ItemPresenterInfo mContext;
        private Vector3 mTargetMovePos;

        private EnumItemPresenterCategory mDisplayCategory = EnumItemPresenterCategory.Item;
        private EnumPresenterStyle mPresenterStyle = EnumPresenterStyle.NormalStyle;

        public EnumPresenterStyle PresenterStyle
        {
            get { return mPresenterStyle; }
        }

        private Vector2 mDisplaySize = Vector2.zero;

        #region SetContext

        public void SetContext(ItemPresenterInfo value)
        {
            if (null != value && IsEmptyCardStyle(mPresenterStyle))
            {
                SetContext(value, EnumPresenterStyle.NormalStyle);
            }
            else
            {
                SetContext(value, mPresenterStyle);
            }
        }

        public void SetContext(ItemPresenterInfo value, CustomPresenterStyle style)
        {
            switch (style.Type)
            {
                //todo
                // case CustomPresenterStyle.EnumStyleType.PresenterStyle:
                //     SetContext(value, style.PresenterStyle);
                //     break;
                // case CustomPresenterStyle.EnumStyleType.HeroSmallCardStyle:
                //     SetContext(value, EnumPresenterStyle.NormalStyle);
                //     GetItemView<HeroCardSmall>()?.SetCardStyle(style.HeroSmallCardStyle);
                //     break;
                // case CustomPresenterStyle.EnumStyleType.ItemCardStyle:
                //     SetContext(value, EnumPresenterStyle.NormalStyle);
                //     GetItemView<ItemCard>()?.SetCardStyle(style.ItemCardStyle);
                //     break;
                // case CustomPresenterStyle.EnumStyleType.EquipCardStyle:
                //     SetContext(value, EnumPresenterStyle.NormalStyle);
                //     GetItemView<PresenterAfEquipCell>()?.SetCardStyle(style.EquipCardStyle);
                //     break;
            }
        }

        public void SetContext(ItemPresenterInfo value, EnumPresenterStyle style)
        {
            if (null == value)
            {
                mContext = null;
                ClearContextView();
                if (IsEmptyCardStyle(style))
                {
                    SetPresenterStyle(style);
                }
                else
                {
                    SetPresenterStyle(EnumPresenterStyle.EmptyStyle);
                }

                return;
            }
            if(!IsAlive) return;
            CheckContext(value);
            mContext = value;
            LoadWithCategory(mContext.Category);
            SetPresenterStyle(style);
            SetGot(false);
        }

        #endregion

        public ItemPresenterInfo GetContext()
        {
            return mContext;
        }

        /// <summary>
        /// 外边框
        /// </summary>
        /// <param name="borderImgName"></param>
        public void SetBorderImage(string borderImgName)
        {
           //todo K
        }

        /// <summary>
        /// 加锁图
        /// </summary>
        /// <param name="lockedImgName"></param>
        public void SetLockedImage(string lockedImgName)
        {
            //todo K
        }

        public void SetLockedBig(bool isLock)
        {
            _BigLock.SetActive(isLock);
        }

        /// <summary>
        /// +号图
        /// </summary>
        /// <param name="lockedImgName"></param>
        public void SetAddImage(string lockedImgName)
        {
            //todo K
        }

        /// <summary>
        /// 置灰色
        /// </summary>
        /// <param name="isGray"></param>
        public void SetGray(bool isGray)
        {
            mContextView?.SetGray(isGray);
        }
        
        public void SetLock(bool isLock)
        {
            _Lock.SetActive(isLock);
        }

        public void SetChange(bool isChange)
        {
            _change.SetActive(isChange);
        }

        public void SetVirtualItemTxt(string txt)
        {
            _VirtualItemTxt.SetActive(true);
            mVirtualItemTxtTxt.text = txt;
        }

        /// <summary>
        /// 红点显示
        /// </summary>
        /// <param name="active"></param>
        public void SetRedDotStateActive(bool active)
        {
            _redDot.SetActive(active);
        }
        
        public void SetSelectAble(bool isSelected)
        {
            _mPresenterSelect.SetActive(isSelected);
            mContextView?.OnSelected(isSelected);
        }
        
        public void SetCommonSelected(bool isSelected)
        {
            _mPresenterSelect.SetActive(isSelected);
            // mContextView?.OnSelected(isSelected);
        }

        public void SetCommonSelectedRect(int Left,int Bottom,int Right,int Top)
        {
            _mPresenterSelect.GetComponent<RectTransform>().offsetMin=new Vector2(Left,Bottom);
            _mPresenterSelect.GetComponent<RectTransform>().offsetMax=new Vector2(Right,Top);
        }


        public void SetPresenterStyle(EnumPresenterStyle style)
        {
            mPresenterStyle = style;
            switch (mPresenterStyle)
            {
                case EnumPresenterStyle.NormalStyle:
                case EnumPresenterStyle.RewardIconOnlyStyle:
                case EnumPresenterStyle.NoBorderIconOnlyStyle:
                case EnumPresenterStyle.BorderIconOnlyStyle:
                case EnumPresenterStyle.BorderIconWithBgStyle:
                    _mPresenterStyle.SetActive(false);
                    _mPresenterCard.SetActive(true);
                    Convert2CardStyle();
                    break;
                case EnumPresenterStyle.EmptyStyle:
                    ToEmptyStyle();
                    break;
                case EnumPresenterStyle.LockedStyle:
                    ToEmptyStyle();
                    _m_sprLocked.SetActive(true);
                    break;
                case EnumPresenterStyle.AddStyle:
                    ToEmptyStyle();
                    _m_sprAdd.SetActive(true);
                    break;
            }
        }

        private void ToEmptyStyle()
        {
            DestroyContextView();

            _mPresenterCard.SetActive(false);
            _mPresenterStyle.SetActive(true);
            _m_sprAdd.SetActive(false);
            _m_sprLocked.SetActive(false);
            if (mDisplaySize != Vector2.zero)
            {
                ScaleToDisplaySize();
            }
        }

        private bool IsEmptyCardStyle(EnumPresenterStyle style)
        {
            return style == EnumPresenterStyle.EmptyStyle
                   || style == EnumPresenterStyle.AddStyle
                   || style == EnumPresenterStyle.LockedStyle;
        }

        protected override void OnInitializeComponent()
        {
            base.OnInitializeComponent();

            mEventLongPress = UGUIEvents.GetLongPress(this.Root, null);
            // ItemPresenter默认添加长按事件
            mEventLongPress.onPress = OnLongPressTriggered;
            // ItemPresenter默认添加点击事件
            // (点击事件这样处理的原因是, 点击和长按会出现冲突。
            // 两个事件必须在一个组件中处理, 同一个操作只允许触发1次)
            mEventLongPress.onPressCancel = OnLongPressCancelTriggered;
        }


        // 长按被触发
        private void OnLongPressTriggered(PointerEventData data, bool isPress)
        {
            if (IsEmptyCardStyle(mPresenterStyle)) return;
            
            // 防止被触发两次
            if (!isPress) return; 
            
            if (null == mOnLongPressListener)
            {
                mContextView?.OnDefaultLongPressedHandler();
            }
            else
            {
                mOnLongPressListener.Invoke(this, data, isPress);
            }

            Debug.Log("item presenter LongPress.");
        }

        // 长按取消(未达到长按阈值), 视为点击被触发
        private void OnLongPressCancelTriggered(PointerEventData data)
        {
            OnBtn_Clicked(null, null);
        }


        private void CheckContext(ItemPresenterInfo newContext)
        {
            if (mDisplayCategory != newContext.Category)
            {
                ClearContextView();
            }
        }

        private void ClearContextView()
        {
            if (mContextView != null)
            {
                ((UIItem) mContextView).Destroy();
            }

            mContextView = null;
        }

        public void DestroyContextView()
        {
            ClearContextView();
            mContext = null;
            _mPresenterStyle.SetActive(false);
        }


        public T GetItemView<T>() where T : class, IPresenterContextView
        {
            return mContextView as T;
        }
        
        /// <summary>
        /// 是否卖完(暂时用于商城)
        /// </summary>
        /// <param name="isGot"></param>
        public void SetGot(bool isGot){
            mContextView?.SetGray(isGot);
            if (_GotImage != null)
            {
                _GotImage.SetActive(isGot);
            }
//            _GotImage?.SetActive(isGot);
        }


        public void SetGotScale(float scale){
            if (_GotImage != null)
            {
                _GotImage.transform.localScale = new Vector3(scale,scale,scale);
            }
        }
        
        // 展示逻辑(分层？，英雄，物品，碎片)
        private void LoadWithCategory(EnumItemPresenterCategory category)
        {
            mDisplayCategory = category;
            switch (category)
            {
                //todo 在这往content节点添加对应的itemcell
                // case EnumItemPresenterCategory.Hero:
                //     InitCategoryItem<HeroCardSmall>();
                //     break;
                // case EnumItemPresenterCategory.Item:
                //     InitCategoryItem<ItemCard>();
                //     break;
            }

            //同比整体缩放---》DisplaySize
            if (mDisplaySize != Vector2.zero)
            {
                ScaleToDisplaySize();
            }

            SetSelectAble(false);
        }

        public void SetContextScaleSize(Vector2 size)
        {
            mDisplaySize = size;
            ScaleToDisplaySize();
            base.SetSize(size); //使用UIObject的SetSize
        }

        public override void SetSize(Vector2 size)
        {
            SetContextScaleSize(size);
        }

        private void ScaleToDisplaySize()
        {
            if (null != mContextView)
            {
                UIItem item = mContextView as UIItem;
                Vector2 itemSize = item.GetSize();
                _mPresenterCard.transform.localScale =
                    new Vector3(mDisplaySize.x / itemSize.x, mDisplaySize.y / itemSize.y, 1f);
            }
        }

        private void InitCategoryItem<T>() where T : UIItem, new()
        {
            if (mContextView == null)
            {
                mContextView = (IPresenterContextView) this.AddSubItemToNode<T>(_mPresenterCard.transform);
            }

            mContextView.SetContext(mContext);

            UIItem contextItem = mContextView as UIItem;
            RectTransform contentRect = contextItem.UITransform;

            // 如果没有设置缩放,设置实例化卡片原始的尺寸给ItemPresenter
            if (mDisplaySize == Vector2.zero)
            {
                UITransform.sizeDelta = contentRect.sizeDelta;
            }

            contentRect.anchorMin = Vector2.one * 0.5f;
            contentRect.anchorMax = Vector2.one * 0.5f;
            contentRect.anchoredPosition = Vector2.zero;

            _mPresenterStyle.SetActive(false);
            _mPresenterCard.SetActive(true);
        }
        
        /// <summary>
        /// TableView绑定数据用
        /// </summary>
        public void Binding(ItemPresenterInfo data, int dataIndex)
        {
            this.SetContext(data);
        }
        
        

        #region Click Event
        
        /// <summary>
        /// 设置点击事件（覆盖所有点击事件）
        /// </summary>
        public void SetClickListener(Action<ItemPresenter> callback)
        {
            mOnItemClick = callback;
        }

        public void SetIgnoreEmptyClickEvent(bool result)
        {
            mIgnoreEmptyClick = result;
        }

        /// <summary>
        /// 长按事件
        /// </summary>
        /// <param name="callback"></param>
        public void SetLongPressListener(Action<ItemPresenter, PointerEventData, bool> callback)
        {
            mOnLongPressListener = callback;
        }

        /// <summary>
        /// 设置点击事件Active
        /// </summary>
        public void SetClickActive(bool active)
        {
            if (null != mEventLongPress)
            {
                mEventLongPress.enabled = active;
            }
        }

        private void OnBtn_Clicked(UIButton arg1, object arg2)
        {
            if (mPresenterStyle == EnumPresenterStyle.EmptyStyle && !mIgnoreEmptyClick) return;
            if (null == mOnItemClick)
            {
                TriggerDefaultClick();
            }
            else
            {
                mOnItemClick.Invoke(this);
            }
        }

        public void Select()
        {
            mOnItemClick?.Invoke(this);
        }

        public void TriggerDefaultClick()
        {
            mContextView?.OnDefaultClickedHandler();
        }

        #endregion

        #region Convert2CardStyle

        private void Convert2CardStyle()
        {
            switch (mDisplayCategory)
            {
                case EnumItemPresenterCategory.Hero:
                    Convert2HeroCardStyle();
                    break;
                case EnumItemPresenterCategory.Item:
                    Convert2ItemCardStyle();
                    break;
            }
        }
        

        private void Convert2ItemCardStyle()
        {
            // switch (mPresenterStyle)
            // {
            //     case EnumPresenterStyle.NormalStyle:
            //         GetItemView<ItemCard>()?.SetCardStyle(EnumItemCardStyle.NormalStyle);
            //         break;
            //     case EnumPresenterStyle.NoBorderIconOnlyStyle: //
            //         GetItemView<ItemCard>()?.SetCardStyle(EnumItemCardStyle.NoBorderIconOnlyStyle);
            //         break;
            //     case EnumPresenterStyle.RewardIconOnlyStyle:
            //     case EnumPresenterStyle.BorderIconOnlyStyle:
            //         GetItemView<ItemCard>()?.SetCardStyle(EnumItemCardStyle.BorderIconOnlyStyle);
            //         break;
            //     case EnumPresenterStyle.BorderIconWithBgStyle:
            //         GetItemView<ItemCard>()?.SetCardStyle(EnumItemCardStyle.BorderIconWithBgStyle);
            //         break;
            //         
            // }
        }

        private void Convert2HeroCardStyle()
        {
            // switch (mPresenterStyle)
            // {
            //     case EnumPresenterStyle.NormalStyle:
            //         GetItemView<HeroCardSmall>()?.SetCardStyle(EnumHeroSmallCardStyle.NormalStyle);
            //         break;
            //     case EnumPresenterStyle.NoBorderIconOnlyStyle:
            //         GetItemView<HeroCardSmall>()?.SetCardStyle(EnumHeroSmallCardStyle.NoBorderIconOnlyStyle);
            //         break;
            //     case EnumPresenterStyle.RewardIconOnlyStyle:
            //     case EnumPresenterStyle.BorderIconOnlyStyle:
            //     case EnumPresenterStyle.BorderIconWithBgStyle:
            //         GetItemView<HeroCardSmall>()?.SetCardStyle(EnumHeroSmallCardStyle.BorderIconOnlyStyle);
            //         break;
            // }
        }
        #endregion
    }
}