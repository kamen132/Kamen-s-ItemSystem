/*
* @Author: Kamen
* @Description:
* @Date: 2024年03月08日 星期五 18:03:07
* @Modify:
*/
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KamenGameFramewrok
{
    public class UGUIEventLongPress : UIBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        // 防止多个手指触控
        private static GameObject sLockPressGameObject;

        public enum EnumPressState
        {
            None,
            Pressing,
            Pass,
        }

        private EnumPressState mState;
        private float mTimer;
        private PointerEventData mPointerEventData;

        /// <summary>
        /// 长按触发阈值
        /// </summary>
        public float pressThreshold = 0.85f;

        public Action<PointerEventData, bool> onPress;
        public Action<PointerEventData> onPressCancel;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.dragging)
            {
                return;
            }

            if (null == onPress)
            {
                return;
            }

            if (null != sLockPressGameObject
                && false == sLockPressGameObject.activeInHierarchy)
            {
                return;
            }

            mPointerEventData = eventData;

            StartTimer();
            sLockPressGameObject = gameObject;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (mState == EnumPressState.None)
            {
                return;
            }

            if (null != mPointerEventData)
            {
                sLockPressGameObject = null;

                if (!eventData.dragging)
                {
                    if (mState == EnumPressState.Pressing)
                    {
                        onPressCancel?.Invoke(eventData);
                    }
                    else
                    {
                        onPress?.Invoke(eventData, false);
                    }
                }

                StopTimer();
            }
        }

        private void StartTimer()
        {
            this.mTimer = 0f;
            this.mState = EnumPressState.Pressing;
        }

        private void StopTimer()
        {
            this.mTimer = 0f;
            this.mState = EnumPressState.None;
            mPointerEventData = null;
        }

        private void Update()
        {
            if (mState == EnumPressState.Pressing)
            {
                mTimer += Time.deltaTime;
                if (mTimer >= pressThreshold)
                {
                    sLockPressGameObject = null;

                    this.mState = EnumPressState.Pass;
                    this.onPress?.Invoke(mPointerEventData, true);
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            this.onPress = null;
            this.onPressCancel = null;
            this.mPointerEventData = null;
        }
    }
}