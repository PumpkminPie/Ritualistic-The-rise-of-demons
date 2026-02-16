using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.Character.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 Move { get; private set; }
        public Vector2 DoubleClickMove { get; private set; }
        public UnityEvent<Vector2> OnDoubleTapMove;
        public UnityEvent<Vector2> OnRoll;

        //public float doubleClickTime = 0.25f;
        public float lastTapTime;
        public Vector2 lastTapDir;
        public bool isDoubleClick;

        public float doubleTapThreshold = 0.25f;

        public PlayerInput playerInput;

        InputAction moveAct;
        InputAction rollAct;

        //public Vector2 inputNumber;
        void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        void OnEnable()
        {
            //playerInput.actions.Enable();

            if (GameController.Instance.currentBindPresset == 1)
                playerInput.SwitchCurrentActionMap("Player_Presset1");
            
            else if (GameController.Instance.currentBindPresset == 2)
                playerInput.SwitchCurrentActionMap("Player_Presset2");

            moveAct = playerInput.actions["Move"];
            rollAct = playerInput.actions["Roll"];

            moveAct.performed += OnMove;
            moveAct.canceled += OnMove;
            rollAct.performed += ctx => OnRoll?.Invoke(Vector2.left);

            //Debug.Log(playerInput.currentActionMap.name);
        }
        void OnDisable()
        {
            moveAct.performed -= OnMove;
            moveAct.canceled -= OnMove;
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
            //Debug.Log(context.control.path);
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
                OnRoll?.Invoke(dir);

                //Debug.Log("double tap");

                lastTapTime = 0f;
                lastTapDir = Vector2.zero;
                return;
            }

            lastTapTime = time;
            lastTapDir = dir;
        }
    }
}