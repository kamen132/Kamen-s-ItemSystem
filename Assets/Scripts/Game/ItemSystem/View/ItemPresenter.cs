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
        private UGUIEventLongPress mEventLongPress; 
        private Action<ItemPresenter> mOnItemClick; 
        private Action<ItemPresenter, PointerEventData, bool> mOnLongPressListener; 

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
                case CustomPresenterStyle.EnumStyleType.PresenterStyle:
                    SetContext(value, style.PresenterStyle);
                    break;
                case CustomPresenterStyle.EnumStyleType.HeroSmallCardStyle:
                    SetContext(value, EnumPresenterStyle.NormalStyle);
                    GetItemView<HeroCardCell>()?.SetCardStyle(style.HeroSmallCardStyle);
                    break;
                case CustomPresenterStyle.EnumStyleType.ItemCellStyle:
                    SetContext(value, EnumPresenterStyle.NormalStyle);
                    GetItemView<ItemCell>()?.SetCardStyle(style.ItemCellStyle);
                    break;
                case CustomPresenterStyle.EnumStyleType.HeroEquipStyle:
                    SetContext(value, EnumPresenterStyle.NormalStyle);
                    GetItemView<HeroEquipCell>()?.SetCardStyle(style.HeroEquipCardStyle);
                    break;
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

            if (!IsAlive) return;
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
        
        public void SetBorderImage(string borderImgName)
        {
            //todo K
        }
        
        public void SetLockedImage(string lockedImgName)
        {
            //todo K
        }

        public void SetLockedBig(bool isLock)
        {
            _BigLock.SetActive(isLock);
        }
        
        public void SetAddImage(string lockedImgName)
        {
            //todo K
        }
        
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
        
        public void SetRedDotStateActive(bool active)
        {
            _redDot.SetActive(active);
        }

        public void SetSelectAble(bool isSelected)
        {
            _mPresenterSelect.SetActive(isSelected);
            mContextView?.OnSelected(isSelected);
        }
        

        public void SetCommonSelectedRect(int Left, int Bottom, int Right, int Top)
        {
            _mPresenterSelect.GetComponent<RectTransform>().offsetMin = new Vector2(Left, Bottom);
            _mPresenterSelect.GetComponent<RectTransform>().offsetMax = new Vector2(Right, Top);
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
            mEventLongPress.onPress = OnLongPressTriggered;
            mEventLongPress.onPressCancel = OnLongPressCancelTriggered;
        }

        
        private void OnLongPressTriggered(PointerEventData data, bool isPress)
        {
            if (IsEmptyCardStyle(mPresenterStyle)) return;
            
            if (!isPress) return;

            if (null == mOnLongPressListener)
            {
                mContextView?.OnDefaultLongPressedHandler();
            }
            else
            {
                mOnLongPressListener.Invoke(this, data, isPress);
            }
        }
        
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
        
        public void SetGot(bool isGot)
        {
            mContextView?.SetGray(isGot);
            if (_GotImage != null)
            {
                _GotImage.SetActive(isGot);
            }
        }


        public void SetGotScale(float scale)
        {
            if (_GotImage != null)
            {
                _GotImage.transform.localScale = new Vector3(scale, scale, scale);
            }
        }
        
        private void LoadWithCategory(EnumItemPresenterCategory category)
        {
            mDisplayCategory = category;
            switch (category)
            {
                //todo 在这往content节点添加对应的itemcell
                case EnumItemPresenterCategory.Hero:
                    InitCategoryItem<HeroCardCell>();
                    break;
                case EnumItemPresenterCategory.Item:
                    InitCategoryItem<ItemCell>();
                    break;
                case EnumItemPresenterCategory.HeroEquip:
                    InitCategoryItem<HeroEquipCell>();
                    break;
            }
            
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
            base.SetSize(size); 
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
        


        #region Click Event
        
        public void SetClickListener(Action<ItemPresenter> callback)
        {
            mOnItemClick = callback;
        }

        public void SetIgnoreEmptyClickEvent(bool result)
        {
            mIgnoreEmptyClick = result;
        }
        
        public void SetLongPressListener(Action<ItemPresenter, PointerEventData, bool> callback)
        {
            mOnLongPressListener = callback;
        }
        
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
                    Convert2ItemCellStyle();
                    break;
                case EnumItemPresenterCategory.HeroEquip:
                    Convert2HeroEquipInfoItemStyle();
                    break;
            }
        }


        private void Convert2ItemCellStyle()
        {
            switch (mPresenterStyle)
            {
                case EnumPresenterStyle.NormalStyle:
                    GetItemView<ItemCell>()?.SetCardStyle(EnumItemCellStyle.NormalStyle);
                    break;
                case EnumPresenterStyle.NoBorderIconOnlyStyle: 
                    GetItemView<ItemCell>()?.SetCardStyle(EnumItemCellStyle.NoBorderIconOnlyStyle);
                    break;
                case EnumPresenterStyle.RewardIconOnlyStyle:
                case EnumPresenterStyle.BorderIconOnlyStyle:
                    GetItemView<ItemCell>()?.SetCardStyle(EnumItemCellStyle.BorderIconOnlyStyle);
                    break;
                case EnumPresenterStyle.BorderIconWithBgStyle:
                    GetItemView<ItemCell>()?.SetCardStyle(EnumItemCellStyle.BorderIconWithBgStyle);
                    break;
                    
            }
        }

        private void Convert2HeroCardStyle()
        {
           
        }

        private void Convert2HeroEquipInfoItemStyle()
        {
            
        }
        #endregion
    }
}