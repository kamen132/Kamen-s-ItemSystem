/*
* @Author: Kamen
* @Description:
* @Date: 2024年03月08日 星期五 18:03:14
* @Modify:
*/
using System;
using UnityEngine;
using UnityEngine.UI;

namespace KamenGameFramewrok
{
    public static class UGUIEvents
    {
        private static void CheckEventRaycastGraphic(GameObject go)
        {
            Collider collider = go.GetComponent<Collider>();

            if (null != collider)
            {
                return;
            }

            Graphic graphic = go.GetComponent<Graphic>();

            if (null == graphic)
            {
                graphic = go.AddComponent<NoDrawcallRect>();
                graphic.raycastTarget = true;
            }
        }

        public static UGUIEventPointer GetPointer(GameObject go, Action<UGUIEventPointer> afterCreate, object extra = null)
        {
            CheckEventRaycastGraphic(go);

            UGUIEventPointer evet = go.GetComponent<UGUIEventPointer>();

            if (null == evet)
            {
                evet = go.AddComponent<UGUIEventPointer>();
            }

            evet.Extra = extra;

            afterCreate?.Invoke(evet);

            return evet;
        }

        public static UGUIEventDrag GetDrag(GameObject go, object extra = null, bool checkScrollRect = false)
        {
            CheckEventRaycastGraphic(go);

            UGUIEventDrag evet = go.GetComponent<UGUIEventDrag>();

            if (null == evet)
            {
                evet = go.AddComponent<UGUIEventDrag>();
            }
            
            return evet;
        }
        
        
        public static UGUIEventLongPress GetLongPress(GameObject go, Action<UGUIEventLongPress> afterCreate, float pressThreshold = 0f)
        {
            CheckEventRaycastGraphic(go);

            UGUIEventLongPress evet = go.GetComponent<UGUIEventLongPress>();
            UIButtonEffect buttonEffect = go.AddComponent<UIButtonEffect>();
            if (null == evet)
            {
                evet = go.AddComponent<UGUIEventLongPress>();
            }

            if (buttonEffect == null)
            {
                go.AddComponent<UIButtonEffect>();
            }

            if (pressThreshold > 0f)
            {
                evet.pressThreshold = pressThreshold;
            }

            afterCreate?.Invoke(evet);

            return evet;
        }
    }
}