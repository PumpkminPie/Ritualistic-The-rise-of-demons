using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; set; }

    PlayerInput input;

    void Awake()
    {
        input = new PlayerInput();
    }


    void OnEnable()
    {
        input.actions.Enable();

        input.actions.FindAction("Movement");
    }
}
