/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月27日 星期五 16:10:21
* @Modify:
*/
using System;
using UnityEngine;

namespace KamenGameFramewrok
{
    /// <summary>
    /// UI组件基类
    /// </summary>
    public class UIObject : IUIObject
    {
        #region Attributes
        public virtual string Name { get; protected set;}
        public virtual string AssetPath { get; protected set;}
        public bool IsAlive { get; private set; }
        public bool IsVisible { get; private set; }
        
        private Transform mTransform;
        /// <summary>
        /// UI组件 RectTrans
        /// </summary>
        public RectTransform UITransform { get; private set; }
        
        private GameObject mRoot;
        /// <summary>
        /// UI组件节点
        /// </summary>
        public GameObject Root
        {
            get
            {
                return mRoot;
            }

            protected set
            {
                mRoot = value;
                if (mRoot != null)
                {
                    UITransform = mRoot.transform as RectTransform;
                }
            }
        }

        #endregion

        #region Override Method

        protected virtual void OnInitialize()
        {
            OnInitializeComponent();
            SetActive(Root.activeSelf);
        }
        protected virtual void OnPreInitializeAssets()
        {

        }

        protected virtual void OnInitializeAsset()
        {

        }

        protected virtual void OnInitializeComponent()
        {

        }

        protected virtual void OnShow()
        {

        }

        protected virtual void OnHide()
        {

        }


        protected virtual void OnDestroy()
        {
            OnViewHideEvent = null;
            OnViewShowEvent = null;
            OnViewDestroyEvent = null;
        }

        #endregion
        
        #region interface

        public void Initialize()
        {
            OnPreInitializeAssets();
            OnInitializeAsset();
        }

        protected virtual void FindComponent()
        {
            
        }
        
        private void InstantiateAssetCallBack(GameObject root)
        {
            this.Root = root;
            if (Root != null)
            {
                IsAlive = true;
            }

            if (IsAlive)
            {
                FindComponent();
                OnInitialize();
            }
        }

        private bool mIsDestroying;
        public void Destroy()
        {
            if (mIsDestroying)
            {
                return;
            }
            
            mIsDestroying = true;
            OnViewDestroyEvent?.Invoke(this);
            OnViewDestroyEvent = null;
            OnViewHideEvent = null;
            OnViewShowEvent = null;
            KamenGame.Instance.AssetModule.Destroy(Root);
            OnDestroy();
        }

        public void Show()
        {
            if (IsAlive == false)
                return;

            if (!IsVisible)
            {
                OnShow();
                if (null != Root) Root.SetActive(true);
                OnViewShowEvent?.Invoke(this);
                IsVisible = true;
            }
        }

        public void Hide()
        {
            if (IsAlive == false)
                return;

            if (IsVisible)
            {
                OnHide();
                if (null != Root) Root.SetActive(false);
                OnViewHideEvent?.Invoke(this);
                IsVisible = false;
            }
        }

        #endregion

        #region Custom Method
        
        /// <summary>
        /// 设置显隐
        /// </summary>
        /// <param name="active"></param>
        public void SetActive(bool active)
        {
            if (active)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="assetName">完成资源路径</param>
        public void InstantiateAsset(string assetName)
        {
#if UNITY_EDITOR
            if (KamenGame.Instance.isUseLocalUIAssets)
            {
                KamenGame.Instance.AssetModule.Instantiate(assetName,InstantiateAssetCallBack);
            }
            else
            {
                KamenGame.Instance.AssetModule.InstantiateOnePerFrame(assetName, InstantiateAssetCallBack);
            }
            return;
#endif
            KamenGame.Instance.AssetModule.InstantiateOnePerFrame(assetName, InstantiateAssetCallBack);
        }

        /// <summary>
        /// 设置父节点
        /// </summary>
        /// <param name="obj"></param>
        public void SetParent(GameObject obj)
        {
            SetParent(obj.transform);
        }

        /// <summary>
        /// 设置父节点
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(Transform parent)
        {
            mTransform = parent;
            if (Root != null)
            {
                if (mTransform != Root.transform.parent)
                {
                    Root.transform.SetParent(mTransform, false);
                }

                if (parent != null)
                {
                    Root.layer = mTransform.gameObject.layer;
                }
            }
        }

        /// <summary>
        /// 重置UI
        /// </summary>
        public void ResetUIObject()
        {
            if (UITransform != null)
            {
                UITransform.anchoredPosition = Vector3.zero;
                UITransform.localScale = Vector3.one;
                UITransform.localRotation = Quaternion.identity;
            }
        }

        /// <summary>
        /// 设置UI位置
        /// </summary>
        /// <param name="pos"></param>
        public void SetAnchorPosition(Vector2 pos)
        {
            if (UITransform != null)
            {
                UITransform.anchoredPosition = pos;
            }
        }

        /// <summary>
        /// 设置UI大小
        /// </summary>
        /// <param name="size"></param>
        public virtual void SetSize(Vector2 size)
        {
            UITransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            UITransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        }

        /// <summary>
        /// 设置UI宽度
        /// </summary>
        /// <param name="width"></param>
        public void SetWidth(float width)
        {
            UITransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }

        /// <summary>
        /// 设置UI高度
        /// </summary>
        /// <param name="height"></param>
        public void SetHeight(float height)
        {
            UITransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        /// <summary>
        /// 获取UI大小
        /// </summary>
        /// <returns></returns>
        public Vector2 GetSize()
        {
            return UITransform.rect.size;
        }

        /// <summary>
        /// 原有基础上偏移
        /// </summary>
        /// <param name="offset"></param>
        public void Offset(Vector2 offset)
        {
            UITransform.anchoredPosition = UITransform.anchoredPosition + offset;
        }

        /// <summary>
        /// 偏移 - X轴
        /// </summary>
        /// <param name="offsetX"></param>
        public void OffsetX(float offsetX)
        {
            var pos = UITransform.anchoredPosition;
            pos.x += offsetX;
            UITransform.anchoredPosition = pos;
        }

        /// <summary>
        /// 偏移 - Y轴
        /// </summary>
        /// <param name="offsetY"></param>
        public void OffsetY(float offsetY)
        {
            var pos = UITransform.anchoredPosition;
            pos.y += offsetY;
            UITransform.anchoredPosition = pos;
        }

        /// <summary>
        /// 是否被激活 --外部禁止调用 UI框架调用
        /// </summary>
        /// <param name="alive"></param>
        public void SetIsAlive(bool alive)
        {
            IsAlive = alive;
        }

        /// <summary>
        /// 设置父节点为空
        /// </summary>
        public void SetParentNull()
        {
            if (!IsAlive)
            {
                return;
            }
            Root.transform.SetParent(null,false);
        }


        private event Action<UIObject> OnViewDestroyEvent;
        private event Action<UIObject> OnViewHideEvent;
        private event Action<UIObject> OnViewShowEvent;
        
        /// <summary>
        /// 销毁事件 - 注册
        /// </summary>
        /// <param name="callBack"></param>
        public void RegisterDestroyEvent(Action<UIObject> callBack)
        {
            OnViewDestroyEvent -= callBack;
            OnViewDestroyEvent += callBack;
        }

        /// <summary>
        /// 销毁事件 - 注销
        /// </summary>
        /// <param name="callBack"></param>
        public void UnRegisterDestroyEvent(Action<UIObject> callBack)
        {
            OnViewDestroyEvent -= callBack;
        }
        
        /// <summary>
        /// 关闭事件 - 注册
        /// </summary>
        /// <param name="callBack"></param>
        public void RegisterCloseEvent(Action<UIObject> callBack)
        {
            OnViewHideEvent -= callBack;
            OnViewHideEvent += callBack;
        }
        
        /// <summary>
        /// 关闭事件 - 注销
        /// </summary>
        /// <param name="callBack"></param>
        public void UnRegisterCloseEvent(Action<UIObject> callBack)
        {
            OnViewHideEvent -= callBack;
        }

        /// <summary>
        /// 展示事件 - 注册
        /// </summary>
        /// <param name="callBack"></param>
        public void RegisterShowEvent(Action<UIObject> callBack)
        {
            OnViewShowEvent -= callBack;
            OnViewShowEvent += callBack;
        }
        
        /// <summary>
        /// 展示事件 - 注销
        /// </summary>
        /// <param name="callBack"></param>
        public void UnRegisterShowEvent(Action<UIObject> callBack)
        {
            OnViewShowEvent -= callBack;
        }
        
        public void UnRegisterNotifyViewClose()
        {
            OnViewHideEvent = null;
        }
        #endregion
    }
}