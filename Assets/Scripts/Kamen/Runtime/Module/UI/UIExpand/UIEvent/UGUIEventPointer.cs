/*
* @Author: Kamen
* @Description:
* @Date: 2024年03月08日 星期五 18:03:29
* @Modify:
*/
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KamenGameFramewrok
{
    public class UGUIEventPointer : MonoBehaviour,
        IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public object Extra;
        public Action<PointerEventData, UGUIEventPointer> onClick;
        public Action<PointerEventData, UGUIEventPointer, bool> onPress;


        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(eventData, this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            onPress?.Invoke(eventData, this, true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onPress?.Invoke(eventData, this, false);
        }
    }

}
