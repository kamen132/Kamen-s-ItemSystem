/*
* @Author: Kamen
* @Description:
* @Date: 2023年11月06日 星期一 11:11:04
* @Modify:
*/
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KamenGameFramewrok
{
    public class UIButtonEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private readonly Color mPressedColor = new Color32(200, 200, 200, 255);

        public void OnPointerDown(PointerEventData eventData)
        {
            ChangeChildColor(mPressedColor);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ChangeChildColor(Color.white);
        }

        private Graphic[] mGraphics;

        private void ChangeChildColor(Color color)
        {
            if (mGraphics == null)
            {
                mGraphics = GetComponentsInChildren<Graphic>();
            }

            foreach (var targetGraphic in mGraphics)
            {
                //ui源码
                if (targetGraphic != null)
                {
                    targetGraphic.CrossFadeColor(color, 0.1f, true, true);
                }
            }
        }

        private void OnDestroy()
        {
            mGraphics = null;
        }
    }
}