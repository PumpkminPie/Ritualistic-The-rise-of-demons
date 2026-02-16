using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.Character.Player.Movement
{
    public class Player_Movement : MonoBehaviour
    {
        [Header("Basic stats")]
        [SerializeField] float maxSpeed = 1;
        [SerializeField] bool isRightFacing = true;
        [SerializeField] bool canMove = true;

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

        [Header("Events")]
        public UnityEvent OnPlayerWalk;
        public UnityEvent OnPlayeStoprWalk;

        Rigidbody2D rb;
        PlayerInputHandler inputHandler;
        PlayerInput playerInput;
        [SerializeField] Animator animator;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            inputHandler = GetComponent<PlayerInputHandler>();
            //animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();

            // event pra quando o input pegar o "double click" direcional
            inputHandler.OnRoll?.AddListener((value) => TryRoll(value));

            // Eventos de quando andar e quando parar de andar (mais versatil e menos dependente de referencia)
            OnPlayerWalk?.AddListener(() => animator.SetBool("IsWalking", true));
            OnPlayeStoprWalk?.AddListener(() => animator.SetBool("IsWalking", false));
        }

        void Update()
        {
            // pegar input do player (vec2)
            if (canMove)
                direction = (inputHandler.Move).normalized;
            else
                direction = Vector2.zero;

            //if (rb.attachedColliderCount == 0)
            velocity = (direction * maxSpeed);

            if (velocity.magnitude > 0)
                OnPlayerWalk?.Invoke();
            else
                OnPlayeStoprWalk?.Invoke();
            /*if (isRolling)
                TryRoll();*/
            /*else
                rollTime = Mathf.MoveTowards(rollTime, rollRechargeMaxTime, 0.1f);*/

            canMove = isRolling ? false : true;

            if (inputHandler.Move.x < 0 && !isRightFacing)
                FlipPlayer(true);
            else if (inputHandler.Move.x > 0 && isRightFacing)
                FlipPlayer(false);
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

        public void FlipPlayer(bool _side)
        {
            isRightFacing = _side;

            if (_side)
                transform.rotation = Quaternion.Euler(0, 180f, 0);

            else
                transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        public Vector2 GetVelocity()
        {
            return velocity;
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
