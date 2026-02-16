using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; set; }
    
    InputSystem_Actions input;

    public Vector2 Move {  get; private set; }

    private void Awake()
    {
        input = new InputSystem_Actions();

        if (!Instance)
        {
            Instance = this;
            //Don
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        input.Enable();

        input.Player.Move.performed += ctx => Move = ctx.ReadValue<Vector2>();
    }
}
