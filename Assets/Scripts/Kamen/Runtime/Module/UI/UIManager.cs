/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月27日 星期五 19:10:05
* @Modify:
*/

using System;
using System.Collections.Generic;
using Game.UI.Code;

using UnityEngine;
using UnityEngine.UI;

namespace KamenGameFramewrok
{
    public class UIManager : Module , IUIManager
    {
        public event Action<UIView> OnViewClose;
        public event Action<UIView> OnViewCreate;
        /// <summary>
        /// 目前创建过的Layer
        /// </summary>
        private Dictionary<UILayer, RectTransform> mViewLayers =
            new Dictionary<UILayer, RectTransform>();
        /// <summary>
        /// 用于存储Layer节点的根节点
        /// </summary>
        private Dictionary<UILayer, RectTransform> dictLayerRootTransform =
            new Dictionary<UILayer, RectTransform>();

        private List<UIStackManager> mViewManagerList = null;
        
        private UIActionBarViewManager mActionBarViewManager = new UIActionBarViewManager();

        public UIActionBarViewManager ActionBarViewMgr
        {
            get { return mActionBarViewManager; }
        }


        private UIAlertViewManager mAlertViewManager = new UIAlertViewManager();
        public UIAlertViewManager AlertViewManager
        {
            get
            {
                return mAlertViewManager;
            }
        }
        
        public void CloseAllView()
        {
            mActionBarViewManager.CloseAllView();
            mAlertViewManager.CloseAllView();
        }
        
        public void OnViewClosed(UIView view)
        {
            OnViewClose?.Invoke(view);
        }
        

        public KVIEW CreateView<KVIEW>(UILayer layer, bool isFullScreen) where KVIEW : UIView, new()
        {
            // 获取层级
            RectTransform container = GetRectTransformOfLayer(layer);
            // 创建
            KVIEW view = new KVIEW();
            OnViewPreCreated(view);
            view.Init(layer, container.gameObject, isFullScreen);
            OnViewCreated(view);
            return view;
        }

        public KVIEW CreateView<KVIEW>(GameObject parentContainer, bool isFullScreen) where KVIEW : UIView, new()
        {
            KVIEW view = new KVIEW();
            OnViewPreCreated(view);
            view.Init(UILayer.None, parentContainer, isFullScreen);
            OnViewCreated(view);
            return view;
        }

        private void OnViewPreCreated(UIView view)
        {
            // 创建事件
            OnViewCreate?.Invoke(view);
        }

        private void OnViewCreated(UIView view)
        {
            // 创建事件
            OnViewCreate?.Invoke(view);
        }
        
        private int GetShowingPopupViewCount()
        {
            int retCount = 0;
            if (!mAlertViewManager.IsEmpty())
            {
                retCount += mAlertViewManager.GetStackViews().Count;
            }

            return retCount;
        }
        
        public UICustomAlert<CONTENT_ITEM> CreateCustomAlert<CONTENT_ITEM>(UILayer layer, bool isFullScreen) where CONTENT_ITEM : UIItem, new()
        {
            return CreateAccompanyAlert<CONTENT_ITEM>(layer, isFullScreen);
        }
        

        public KVIEW CreateActionBarView<KVIEW>(UILayer layer = UILayer.ActionBarView, bool isFullScreen = false) where KVIEW : UIActionBarBaseView, new()
        {
            // 获取层级
            RectTransform container = GetRectTransformOfLayer(layer);
            KVIEW view = new KVIEW();
            OnViewPreCreated(view);
            view.Init(layer, container.gameObject, isFullScreen);
            PushView(view);
            OnViewCreated(view);
            return view;
        }


        public UICustomAlert<CONTENT_ITEM> CreateAccompanyAlert<CONTENT_ITEM>(UILayer layer = UILayer.Popup, bool isFullScreen = false) where CONTENT_ITEM : UIItem, new()
        {
            var actionBarView = GetTopActionBarView();
            return CreateAccompanyAlert<CONTENT_ITEM>(actionBarView, layer, isFullScreen);
        }
        
        public UICustomAlert<CONTENT_ITEM> CreateAccompanyAlert<CONTENT_ITEM>(UIActionBarBaseView view,
            UILayer layer = UILayer.Popup,
            bool isFullScreen = false) where CONTENT_ITEM : UIItem, new()
        {
            
            RectTransform container = GetRectTransformOfLayer(layer);
            UICustomAlert<CONTENT_ITEM> alert = new UICustomAlert<CONTENT_ITEM>(view);
            OnViewPreCreated(alert);
            alert.Init(layer, container.gameObject, isFullScreen);
            PushView(alert);
            OnViewCreated(alert);
            return alert;
        }

        public void ReorderCanvas(int index, UILayer maxLayer)
        {
            if (index < 0)
            {
                foreach (var oneRect in mViewLayers.Values) 
                {
                    oneRect.GetComponent<Canvas>().enabled = true;
                }
                return;
            }
    
            //存在全屏界面，将比本层级低的层面全部隐藏
            foreach (UILayer oneLayer in Enum.GetValues(typeof(UILayer)))
            {
                if (mViewLayers.TryGetValue(oneLayer, out var layerObj))
                {
                    layerObj.GetComponent<Canvas>().enabled = oneLayer >= maxLayer;
                }
            }
        }

        private void PushView(UIView ui)
        {
            ui.RegisterDestroyEvent(PopView);
            if (ui is UIActionBarBaseView)
            {
                mActionBarViewManager.PushView(ui);
                return;
            }
            mActionBarViewManager.OnPauseTopView();
            if(ui is UIAlertView)
            {
                mAlertViewManager.PushView(ui);
            }
        }
        
        public UIActionBarBaseView GetTopActionBarView()
        {
            if (null != mActionBarViewManager)
            {
                return (UIActionBarBaseView) mActionBarViewManager.GetTopView();
            }
            return null;
        }
        
        /// <summary>
        /// 将栈中指定UI弹出
        /// </summary>
        private void PopView(UIObject ui)
        {
            UIView uiView = ui as UIView;
            if (ui is UIActionBarBaseView)
            {
                mActionBarViewManager.RemoveView(uiView);
                return;
            }
            if (ui is UIAlertView)
            {
                mAlertViewManager.RemoveView(uiView);
            }
        }

        public bool AutoResume { get; } = true;


        #region 创建层级

         /// <summary>
        /// 获取View层级
        /// </summary>
        public RectTransform GetRectTransformOfLayer(UILayer layer)
        {
            RectTransform rt;

            if (!mViewLayers.TryGetValue(layer, out rt))
            {
                rt = CreateNewLayer(layer);

                if (null != rt)
                {
                    mViewLayers[layer] = rt;
                }

                ReorderLayers();
            }
            return rt;
        }
        /// <summary>
        /// 新建层级
        /// </summary>
        private RectTransform CreateNewLayer(UILayer layer)
        {
            // 新建空节点
            GameObject layerObj = new GameObject(layer.ToString());
            int layerID = LayerMask.NameToLayer(GameLayer.UI.ToString());
            layerObj.layer = layerID;
            RectTransform rt = layerObj.AddComponent<RectTransform>();

            // 放置到根节点
            rt.SetParent(KamenApp.Instance.GetAppRoot(AppRootType.UI).transform);
            rt.SetAsFirstSibling();
            // 归零
            rt.localScale = Vector3.one;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = Vector2.one * 0.5f;
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;

            // 设置canvas
            Canvas canvas = layerObj.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = (int) layer;

            layerObj.AddComponent<GraphicRaycaster>();
            dictLayerRootTransform[layer] = rt;
            return rt;
        }

        /// <summary>
        /// 重新排序层级
        /// </summary>
        private void ReorderLayers()
        {
#if UNITY_EDITOR
            foreach (UILayer layer in Enum.GetValues(typeof(UILayer)))
            {
                RectTransform layerObj;

                if (dictLayerRootTransform.TryGetValue(layer, out layerObj))
                {
                    layerObj.SetAsLastSibling();
                }
            }
            RectTransform notice;
            if (dictLayerRootTransform.TryGetValue(UILayer.Notice, out notice))
            {
                notice.SetAsFirstSibling();
            }
            RectTransform tips;
            if (dictLayerRootTransform.TryGetValue(UILayer.Tips, out tips))
            {
                tips.SetAsFirstSibling();
            }
#endif
        }
        #endregion
    }
}