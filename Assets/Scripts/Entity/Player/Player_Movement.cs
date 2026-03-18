using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace Game.Entity.Player.Movement
{
    public enum VFX_ConfigType 
    { 
        Walking,
        Running,
        Rolling
    }

    [Serializable]
    public struct VFX_Configs
    {
        public GameObject objVisual;
        public VisualEffect visualEffect;

        public VFX_ConfigType type;
    }

    public class Player_Movement : MonoBehaviour
    {
        [Header("Basic stats")]
        [SerializeField] SpriteRenderer playerSprite;
        [SerializeField] float maxWalkSpeed = 1;
        [SerializeField] float maxSprintSpeed = 2;
        [SerializeField] bool isRightFacing = true;
        [SerializeField] bool canMove = true;
        [SerializeField] float currentMaxSpeed;

        [Header("combatRoll")]
        [SerializeField] float rollForce = 1;
        //[SerializeField] float rollRechargeMaxTime = 1;
        [SerializeField] float rollTime = 1;
        [SerializeField] float rollCooldown = 1;
        [SerializeField] bool isRolling;
        [SerializeField] bool canRoll;
        //[SerializeField] bool isColliding;
        //bool wasMoving = false;

        [Header("RigidBody (2D)")]
        [SerializeField] Vector2 direction;
        [SerializeField] Vector2 velocity;
        [SerializeField] Vector2 rollDirection;

        [Header("VFX")]
        [SerializeField] VFX_Configs[] vfx_groups; 

        //[Header("Events")]
        //public UnityEvent OnPlayerStartWalk;
        public event Action OnPlayerWalk;
        public event Action OnPlayerStopWalk;
        public event Action OnPlayerRun;
        public event Action OnPlayeStopRun;
        public event Action<Vector2> OnPlayerRoll;
        public event Action OnPlayerStopRoll;

        Rigidbody2D rb;
        PlayerInputHandler inputHandler;
        PlayerInput playerInput;
        //Transform ogPlayerTransform { get { return transform; } set { ogPlayerTransform = value; } }
        [SerializeField] Animator animator;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            inputHandler = GetComponent<PlayerInputHandler>();
            //animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();

            //ogPlayerTransform = transform;

            // event pra quando o input pegar o "double click" direcional OU quando apertar a tecla de rolar (ambos suportados)
            inputHandler.OnRoll += (value) => TryRoll(value);
            inputHandler.OnPressMovement += () => animator.SetTrigger("StartWalking");

            // Eventos de quando andar e quando parar de andar (mais versatil e menos dependente de referencia)
            //OnPlayerWalk?.AddListener(() => );
            //OnPlayerStopWalk?.AddListener(() => );

            /*foreach (var vfx in vfx_groups)
            {
                OnPlayerStopRoll?.AddListener(() => vfx.visualEffect.Stop());

                OnPlayerRoll?.AddListener((value) => vfx.visualEffect.Play());
            }*/
            foreach (var vfx in vfx_groups)
            {                
                vfx.visualEffect.Stop();
            }
        }

        void Update()
        {
            // pegar input do playerTrans (vec2)
            if (canMove)
            {
                direction = (inputHandler.Move).normalized;

                //if (rb.attachedColliderCount == 0)
                velocity = (direction * currentMaxSpeed);

                if (!inputHandler.Sprint)
                {
                    if (velocity.magnitude > 0)
                    {
                        OnPlayerWalk?.Invoke();
                        animator.SetBool("IsWalking", true);
                    }
                    else
                    {
                        OnPlayerStopWalk?.Invoke();
                        animator.SetBool("IsWalking", false);
                    }
                }
                else
                {
                    if (velocity.magnitude > 0)
                        OnPlayerRun?.Invoke();
                    else
                        OnPlayeStopRun?.Invoke();
                }

                
                foreach (var vfx in vfx_groups)
                {
                    var dir_roll = !isRolling ? (inputHandler.Move).normalized : rollDirection;

                    if (vfx.type == VFX_ConfigType.Rolling)
                    {
                        var _rot = vfx.objVisual.transform.localRotation;
                        float angle = Mathf.Atan2(dir_roll.y, dir_roll.x) * Mathf.Rad2Deg;
                        angle += 180f;

                        if (dir_roll == Vector2.zero)
                            break;

                        //if (MathF.Abs(dir.x) > 1 && MathF.Abs(dir.y) > 1)
                        vfx.objVisual.transform.localRotation = Quaternion.Euler(0, 0, angle);
                        /*else
                            vfx.objVisual.transform.localRotation = Quaternion.Euler(0, 0, -angle);*/

                        if (isRolling)
                        {
                            vfx.visualEffect.Play();
                        }
                    }
                }
                Sprint();

                /*if (isRolling)
                    TryRoll();
                else
                    rollTime = Mathf.MoveTowards(rollTime, rollRechargeMaxTime, 0.1f);*/

                if (inputHandler.Move.x < 0 && !isRightFacing)
                    FlipPlayer(true);
                if (inputHandler.Move.x > 0 && isRightFacing)
                    FlipPlayer(false);
            }
            else
                direction = Vector2.zero;

            canMove = isRolling ? false : true;
            /*bool isMoving = inputHandler.Move != Vector2.zero;

            if (isMoving && !wasMoving)
            {
                //Debug.Log("Player começou a andar");

                OnPlayerStartWalk?.Invoke();
            }

            wasMoving = isMoving;*/
        }
        void FixedUpdate()
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        public void TryRoll(Vector2 dir)
        {
            if (isRolling || !canRoll) return;

            //if (mouseDirection == Vector2.zero)
            rollDirection = dir;

            StartCoroutine(IRoll(rollDirection));
        }

        public void Sprint()
        {
            currentMaxSpeed = inputHandler.Sprint ? maxSprintSpeed : maxWalkSpeed;
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
                animator.SetBool("IsDashing", true);

                OnPlayerRoll?.Invoke(dir);
                yield return null;
            }

            //rb.AddForce = Vector2.zero;
            isRolling = false;
            animator.SetBool("IsDashing", false);
            OnPlayerStopRoll?.Invoke();

            yield return new WaitForSeconds(rollCooldown);

            canRoll = true;
        }

        public void FlipPlayer(bool _side)
        {
            isRightFacing = _side;

            playerSprite.flipX = isRightFacing;
        }

        public Vector2 GetVelocity()
        {
            return velocity;
        }

        public Vector2 GetFaceDirection()
        {
            var _dir = Vector2.zero;

            _dir.y += direction.y;

            _dir.y = Mathf.Clamp(_dir.y, -1, 1);

            if (isRightFacing)
                _dir.x = -1;
            else
                _dir.x = 1;

            return _dir;
        }
    }
}
