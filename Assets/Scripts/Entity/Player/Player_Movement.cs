using System.Collections;
using UnityEngine;

namespace Game.Character.Player.Movement
{
    public class Player_Movement : MonoBehaviour
    {
        [Header("Basic stats")]
        [SerializeField] float maxSpeed = 1;

        [Header("combatRoll")]
        [SerializeField] float rollForce = 1;
        //[SerializeField] float rollRechargeMaxTime = 1;
        [SerializeField] float rollTime = 1;
        [SerializeField] float rollCooldown = 1;
        [SerializeField] bool isRolling;
        [SerializeField] bool canRoll;
        //[SerializeField] bool isColliding;

        [Header("RigidBody (2D)")]
        [SerializeField] Vector2 direction, velocity, rollDirection;

        Rigidbody2D rb;
        PlayerInputHandler inputHandler;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            inputHandler = GetComponent<PlayerInputHandler>();

            inputHandler.OnDoubleTapMove?.AddListener((value) => TryRoll(value));
        }

        void Update()
        {
            direction = (inputHandler.Move).normalized;

            //if (rb.attachedColliderCount == 0)
            velocity = (direction * maxSpeed);


            /*if (isRolling)
                TryRoll();*/
            /*else
                rollTime = Mathf.MoveTowards(rollTime, rollRechargeMaxTime, 0.1f);*/

        }
        void FixedUpdate()
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        public void TryRoll(Vector2 dir)
        {
            if (isRolling || !canRoll) return;

            //if (direction == Vector2.zero)
            rollDirection = dir;

            StartCoroutine(IRoll(rollDirection));
        }

        IEnumerator IRoll(Vector2 dir)
        {
            isRolling = true;
            canRoll = false;

            float timer = 0f;

            while (timer < rollTime)
            {
                rb.AddForce(dir * rollForce);
                timer += Time.deltaTime;
                yield return null;
            }

            //rb.AddForce = Vector2.zero;
            isRolling = false;

            yield return new WaitForSeconds(rollCooldown);

            canRoll = true;
        }

        /*void OnCollisionEnter2D(Collision2D collision)
        {
            isColliding = true;
        }
        void OnCollisionExit2D(Collision2D collision)
        {
            isColliding = false;
        }*/
    }
}
