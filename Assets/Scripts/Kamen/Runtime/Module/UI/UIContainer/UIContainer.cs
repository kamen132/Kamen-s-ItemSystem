/*
* @Author: Kamen
* @Description:
* @Date: 2023年10月28日 星期六 11:10:00
* @Modify:
*/

using System;
using System.Collections.Generic;
using UnityEngine;
namespace KamenGameFramewrok
{
    /// <summary>
    /// UI容器
    /// </summary>
    public class UIContainer : UIObject
    {
        /// <summary>
        /// 容器里的信息
        /// </summary>
        private Dictionary<GameObject, HashSet<UIItem>> mContainerMap;
        
        private HashSet<UIControl> mControls;

        #region Public Method

        /// <summary>
        /// UI添加子节点
        /// </summary>
        public T AddSubItem<T>(T subItem, GameObject parent) where T : UIItem
        {
            if (mContainerMap == null)
            {
                mContainerMap = new Dictionary<GameObject, HashSet<UIItem>>();
            }

            if (!mContainerMap.TryGetValue(parent, out var targetHasSet))
            {
                targetHasSet = new HashSet<UIItem>();
                mContainerMap.Add(parent, targetHasSet);
            }

            if (!targetHasSet.Contains(subItem))
            {
                targetHasSet.Add(subItem);
                return subItem;
            }

            KLogger.LogError("Item 重复添加");

            return subItem;
        }

        /// <summary>
        /// 添加子Item,被自动管理释放的(自动添加到View节点下)
        /// </summary>
        public T AddSubItemToNode<T>() where T : UIItem, new()
        {
            return AddSubItemToNode<T>(UITransform);
        }

        /// <summary>
        /// 添加子Item,并且设置到渲染节点上,被自动管理释放的
        /// </summary>
        private T AddSubItemToNode<T>(Transform parentNode, T subitem) where T : UIItem
        {
            subitem = AddSubItem(subitem, parentNode.gameObject); // 返回null表示添加控制

            if (null != subitem)
            {
                subitem.SetParent(parentNode);
                subitem.ResetUIObject();
                subitem.AttachToContainer(this);
            }

            return subitem;
        }

        public T AddSubItemToNode<T>(GameObject parentNode) where T : UIItem, new()
        {
            return AddSubItemToNode<T>(parentNode.transform);
        }

        /// <summary>
        /// 添加子Item,被自动管理释放的
        /// </summary>
        public T AddSubItemToNode<T>(Transform parentNode) where T : UIItem, new()
        {
            T item = new T();
            item.SetParent(parentNode);
            item.Initialize();
            item.Show();
            if (!this.IsAlive || this.Root == null)
            {
                item.SetIsAlive(false);
                if (item.mContainerMap != null)
                {
                    foreach (var hashset in item.mContainerMap)
                    {
                        foreach (UIItem sItem in hashset.Value)
                        {
                            sItem?.SetIsAlive(false);
                        }
                    }
                }
                return item;
            }

            Transform pNode = null;
            if (parentNode != null)
            {
                pNode = parentNode;
            }
            else
            {
                pNode = this.UITransform;
            }

            if (pNode != null)
            {
                return AddSubItemToNode(pNode, item);
            }
            return item;
        }


        public T AddSubItem<T>() where T : UIItem, new()
        {
            T subItem = new T();
            subItem.Initialize();
            subItem.Show();
            return AddSubItem(subItem);
        }

        public T AddSubItem<T>(T subItem) where T : UIItem
        {
            return AddSubItem<T>(subItem, Root);
        }
        
        /// <summary>
        /// 添加被容器自动管理的Button
        /// </summary>
        public UIButton AddButton(GameObject go, Action callback, object extra = null)
        {
            UIButton button = new UIButton(go, (a,b)=>{callback?.Invoke();});

            if (null != extra)
            {
                button.Extra = extra;
            }

            this.AddControl(button);

            return button;
        }
        
        /// <summary>
        /// 添加被容器自动管理的Button
        /// </summary>
        public UIButton AddButton(GameObject go, Action<UIButton, object> callback, object extra = null)
        {
            UIButton button = new UIButton(go, callback);

            if (null != extra)
            {
                button.Extra = extra;
            }

            this.AddControl(button);

            return button;
        }

        /// <summary>
        /// 添加被容器自动管理的Button
        /// </summary>
        public UIButton AddButton(UnityEngine.UI.Button button, Action<UIButton, object> callback, object extra = null)
        {
            UIButton uiBtn = new UIButton(button, callback);

            if (null != extra)
            {
                uiBtn.Extra = extra;
            }

            this.AddControl(uiBtn);

            return uiBtn;
        }
        
        /// <summary>
        /// 添加控件
        /// </summary>
        public void AddControl(UIControl control)
        {
            if (null == mControls)
            {
                mControls = new HashSet<UIControl>();
            }

            if (!mControls.Contains(control))
            {
                mControls.Add(control);
            }
        }

        /// <summary>
        /// 销毁所有子物体
        /// </summary>
        public void DestroyAllItem()
        {
            if (!IsAlive || null == mContainerMap)
            {
                return;
            }
            foreach (var hashset in mContainerMap)
            {
                foreach (var item in hashset.Value)
                {
                    item.Destroy();
                }
            }
            mContainerMap.Clear();
        }
        
        
        /// <summary>
        /// 销毁所有控件
        /// </summary>
        public void DestroyAllControls()
        {
            if (!this.IsAlive || null == mControls)
            {
                return;
            }

            foreach (UIControl control in mControls)
            {
                // 销毁未销毁的控件
                control.Destroy();
            }
            mControls.Clear();
        }

        /// <summary>
        /// 移除UI组件
        /// </summary>
        /// <param name="uiItem"></param>
        public void RemoveItem(UIItem uiItem)
        {
            if (uiItem==null || mContainerMap.Count==0)
            {
                return;
            }
            if (mContainerMap.TryGetValue(uiItem.Root.transform.parent.gameObject, out var hashSet))
            {
                if (hashSet.Remove(uiItem))
                {
                    uiItem.DetachFromContainer();
                    uiItem.SetParentNull();
                }
            }
        }

        #endregion
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            DestroyAllItem();
            DestroyAllControls();
        }
    }
}
