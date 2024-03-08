using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KamenGameFramewrok
{
    public interface IInputManager : IModule
    {
        public InputAction PlayerMove { get; set; }
        public PlayerInput PlayerInput { get; set; }
        public Vector2 LastTouch { get; }
        public Vector2 LastInput { get; set; }
    }
}