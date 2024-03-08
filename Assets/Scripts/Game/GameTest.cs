using System;
using Game.UI.Code;
using KamenGameFramewrok;
using UnityEngine;

namespace Game
{
    public class GameTest : MonoBehaviour
    {
        private void Awake()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
              
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                KamenGame.Instance.UIManager.CreateActionBarView<UITestActionBarView>();
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                KamenGame.Instance.UIManager.CreateActionBarView<UITestActionBarViewA>();
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                KamenGame.Instance.UIManager.CreateActionBarView<UITestActionBarViewB>();
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                KamenGame.Instance.UIManager.CreateCustomAlert<UITestItem>(UILayer.Popup, false);
            }
        }
    }
}