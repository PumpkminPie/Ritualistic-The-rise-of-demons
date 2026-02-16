using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    //public InputSystem_Actions input;

    public Vector2 Move { get; private set; }

    /* void Awake()
    {
        input = new InputSystem_Actions();
    }

    void OnEnable()
    {
        input.Enable();

        //input.Player.Move.performed += MovePerformed;
    }*/

    public void MovePerformed(InputAction.CallbackContext context)
    {
        Move = context.ReadValue<Vector2>();
    }
}
