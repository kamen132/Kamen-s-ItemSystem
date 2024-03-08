using UnityEngine;
using UnityEngine.InputSystem;

namespace KamenGameFramewrok
{
    internal class InputManager : Module, IInputManager
    {
        private int mLastTouchX = 0;
        private int mLastTouchY = 0;
        public InputAction PlayerMove { get; set; }
        public PlayerInput PlayerInput { get; set; }
        public Vector2 LastTouch => new Vector2(mLastTouchX, mLastTouchY);
        public Vector2 LastInput { get; set; }

        public override void BeforeInit()
        {
            base.BeforeInit();
            PlayerInput = GameApp.Instance.PlayerInput;
            PlayerMove = PlayerInput.actions["Move"];
            PlayerMove.performed += RecordInput;
        }
        
        private void RecordInput(InputAction.CallbackContext context)
        {
            if (!(context.ReadValue<Vector2>() == Vector2.zero))
            {
                LastInput = context.ReadValue<Vector2>();
            }
        }
    }
}