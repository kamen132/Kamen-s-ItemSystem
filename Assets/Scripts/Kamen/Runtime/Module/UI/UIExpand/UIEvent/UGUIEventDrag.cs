/*
* @Author: Kamen
* @Description:
* @Date: 2024年03月08日 星期五 18:03:12
* @Modify:
*/
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KamenGameFramewrok
{
    public class UGUIEventDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
    {
        private ScrollRect mScrollRect;
        public Action<PointerEventData, UGUIEventDrag> onPointerDown;
        public Action<PointerEventData, UGUIEventDrag> onPointerUp;
        public Action<PointerEventData, UGUIEventDrag> onBeginDrag;
        public Action<PointerEventData, UGUIEventDrag> onDrag;
        public Action<PointerEventData, UGUIEventDrag> onEndDrag;
        public Action<PointerEventData, UGUIEventDrag> onDrop;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDrag?.Invoke(eventData, this);
            if (mScrollRect != null)
            {
                mScrollRect.OnBeginDrag(eventData);
            }
        }
    
        public void OnDrag(PointerEventData eventData)
        {
            onDrag?.Invoke(eventData, this);
            if (mScrollRect != null)
            {
                mScrollRect.OnDrag(eventData);
            }
        }
    
        public void OnDrop(PointerEventData eventData)
        {
            onDrop?.Invoke(eventData, this);
        }
    
        public void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag?.Invoke(eventData, this);
            if (mScrollRect != null)
            {
                mScrollRect.OnEndDrag(eventData);
            }
        }
    
        public void OnPointerDown(PointerEventData eventData)
        {
            onPointerDown?.Invoke(eventData, this);
        }
    
        public void OnPointerUp(PointerEventData eventData)
        {
            onPointerUp?.Invoke(eventData, this);
        }
    }
}

