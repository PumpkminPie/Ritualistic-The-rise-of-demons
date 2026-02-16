using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] float maxSpeed = 1;
    //[SerializeField] float acceleration = 2.5f;

    [SerializeField] Vector2 direction, velocity;

    Rigidbody2D rb;
    PlayerInputHandler inputHandler;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        direction = inputHandler.Move;
        velocity += (direction * maxSpeed) * Time.deltaTime;
    }
    private void FixedUpdate()
    {
        rb.MovePosition(velocity);
    }
}
