using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.Character.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public InputSystem_Actions input;

        public Vector2 Move { get; private set; }
        public Vector2 DoubleClickMove { get; private set; }
        public UnityEvent<Vector2> OnDoubleTapMove;

        //public float doubleClickTime = 0.25f;
        public float lastTapTime;
        public Vector2 lastTapDir;
        public bool isDoubleClick;

        public float doubleTapThreshold = 0.25f;

        //public Vector2 inputNumber;
        void Awake()
        {
            input = new InputSystem_Actions();
        }

        void OnEnable()
        {
            input.Enable();

            input.Player.Move.performed += OnMove;
            input.Player.Move.canceled += OnMove;
        }

        /*public void MovePerformed(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }
        public void MoveCanceled(InputAction.CallbackContext context)
        {
            Move = Vector2.zero;
        }*/
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();

            Move = value;

            if (context.performed)
            {
                CheckDoubleTap(value);
            }
        }

        void CheckDoubleTap(Vector2 dir)
        {
            float time = Time.time;

            if (dir == Vector2.zero)
                return;

            // mesma direção?
            if (Vector2.Dot(dir, lastTapDir) > 0.8f &&
                time - lastTapTime <= doubleTapThreshold)
            {
                // DOUBLE TAP DETECTADO
                OnDoubleTapMove?.Invoke(dir);

                Debug.Log("double tap");

                lastTapTime = 0f;
                lastTapDir = Vector2.zero;
                return;
            }

            lastTapTime = time;
            lastTapDir = dir;
        }
    }
}