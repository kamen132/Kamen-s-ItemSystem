/*
* @Author: Kamen
* @Description:
* @Date: 2023年11月06日 星期一 11:11:32
* @Modify:
*/
using System.Collections.Generic;

namespace KamenGameFramewrok
{
    /// <summary>
    /// 界面堆栈管理器
    /// </summary>
    public abstract class UIStackManager
    {
        protected List<UIView> mStackViews = new List<UIView>();
        
        public int mIsFullScreenViewCount = 0;
        public int IsFullScreenViewCount
        {
            get
            {
                if (mReorderCanvasAble) return mIsFullScreenViewCount;
                return 0;
            }
        }
        
        private readonly int mKeepBottomViews = 0;
        private readonly bool mReorderCanvasAble = false;

        public UIStackManager(int keepBottomViews, bool reorderCanvasAble)
        {
            this.mKeepBottomViews = keepBottomViews;
            mReorderCanvasAble = reorderCanvasAble;
        }
        
        /// <summary>
        /// 界面入栈
        /// </summary>
        /// <param name="ui"></param>
        public virtual void PushView(UIView ui)
        {
            OnPauseTopView();
            mStackViews.Add(ui);
            OnResumeTopView();
            if (ui.IsFullScreen)
            {
                mIsFullScreenViewCount++;
                ReorderCanvas();
            }
        }
        
        public virtual bool PopView()
        {
            if (mStackViews.Count > mKeepBottomViews)
            {
                int top = mStackViews.Count - 1;
                UIView ui = mStackViews[top];
                RemoveView(ui);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 界面出栈
        /// </summary>
        /// <returns></returns>
        public virtual UIView PeakTop()
        {
            if (mStackViews.Count > mKeepBottomViews)
            {
                int top = mStackViews.Count - 1;
                UIView ui = mStackViews[top];
                return ui;
            }

            return null;
        }

        public bool IsEmpty()
        {
            return mStackViews.Count == 0;
        }

        public List<UIView> GetStackViews()
        {
            return mStackViews;
        }

        public void RemoveView(UIView ui)
        {
            mStackViews.Remove(ui);
            if (ui.IsFullScreen)
            {
                mIsFullScreenViewCount--;
                ReorderCanvas();
            }

            KamenGame.Instance.UIManager.OnViewClosed(ui);

            ResumeTopView();
        }

        public void OnPauseTopView()
        {
            if (mStackViews.Count > 0)
            {
                int top = mStackViews.Count - 1;
                UIView ui = mStackViews[top];
                ui.OnPause();
            }
        }

        public void ResumeTopView()
        {
            if (KamenGame.Instance.UIManager.AutoResume)
            {
                OnResumeTopView();
            }
        }

        protected virtual void OnResumeTopView()
        {
            if (mStackViews.Count > 0)
            {
                int top = mStackViews.Count - 1;
                UIView ui = mStackViews[top];

                if (NeedMaskViewToShow(ui)) return;

                ui.Show();
                ui.OnResume();
            }
        }

        private bool NeedMaskViewToShow(UIView view)
        {
            if (view == null)
            {
                return false;
            }

            bool mask = false;

            return mask;
        }

        private bool NeedMaskViewToHide(UIView view)
        {
            if (view == null)
            {
                return false;
            }

            bool mask = false;

            return mask;
        }

        public virtual UIView GetTopView()
        {
            if (mStackViews.Count > 0)
            {
                int top = mStackViews.Count - 1;
                return mStackViews[top];
            }

            return null;
        }

        /// <summary>
        ///  关闭所有打开的View
        /// </summary>
        public virtual void CloseAllView()
        {
            while (mStackViews.Count > 0) 
            {
                UIView view = mStackViews[mStackViews.Count - 1];
                view.UnRegisterNotifyViewClose();
                mStackViews.Remove(view);
                KamenGame.Instance.UIManager.OnViewClosed(view);
                // 不调用Close，因为Close 中 有 RemoveView -> OnResumeTopView 处理
                view.Destroy();
            }

            mIsFullScreenViewCount = 0;
            mStackViews.Clear();
            ReorderCanvas();
        }

        public virtual void CloseToBottom(bool isAutoResume = true)
        {
            while (mStackViews.Count > mKeepBottomViews)
            {
                int index = mStackViews.Count - 1;
                UIView view = mStackViews[index];

                if (NeedMaskViewToHide(view)) break;

                mStackViews.Remove(view);
                if (view.IsFullScreen)
                {
                    mIsFullScreenViewCount--;
                }

                KamenGame.Instance.UIManager.OnViewClosed(view);
                view.Close();
            }

            if (isAutoResume) OnResumeTopView();
            ReorderCanvas();
        }

        /// <summary>
        /// 关闭在层里的UIView
        /// </summary>
        public void CloseViewInLayers(params UILayer[] layers)
        {
            if (null == layers || layers.Length <= 0)
            {
                return;
            }

            List<UIView> temp = new List<UIView>();

            // 查找
            for (int i = 0; i < mStackViews.Count; i++)
            {
                UIView baseView = mStackViews[i];

                for (int l = 0; l < layers.Length; l++)
                {
                    if (layers[l] == baseView.Layer)
                    {
                        KamenGame.Instance.UIManager.OnViewClosed(baseView);
                        baseView.UnLoad();
                        baseView.DestroyAllItem();
                        temp.Add(baseView);
                        break;
                    }
                }
            }

            // 移除已经关闭的View的引用
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].IsFullScreen)
                {
                    mIsFullScreenViewCount--;
                }
                mStackViews.Remove(temp[i]);
            }
            OnResumeTopView();
        }

        private void ReorderCanvas()
        {
            if (!mReorderCanvasAble) return;
            int index = -1;
            UILayer maxLayer = UILayer.Bottom;
            for (int i = 0; i < mStackViews.Count; i++)
            {
                if (!mStackViews[i].IsFullScreen)
                {
                    continue;
                }

                if (mStackViews[i].Layer > maxLayer)
                {
                    maxLayer = mStackViews[i].Layer;
                    index = i;
                }
            }

            KamenGame.Instance.UIManager.ReorderCanvas(index, maxLayer);
        }
    }
}