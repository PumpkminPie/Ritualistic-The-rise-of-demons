using Game.Entity.Player;
using Game.Entity.Player.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Entity.Attack
{
    public enum AttackState
    {
        Idle,
        Started,
        //Delay,
        Execute,
        Finished
    }

    public class Entity_AttackRunner : MonoBehaviour
    {
        public Action<Collider2D> OnHitEvent;
        public Action<Vector3> OnAir;
        public Action OnStart;
        public Action OnExecute;
        public Action OnFinish;

        [SerializeField] Vector3 mouseDirection;
        [SerializeField] Vector3 entityDirection;
        [SerializeField] Vector3 playerDirection;

        [SerializeField] Transform playerTrans;
        [SerializeField] Player_InputHandler playerInput;

        [SerializeField] Transform ownerTransform;

        [SerializeField] Entity_AttackData attackData;
        [SerializeField] AttackState state;
        AttackModule currentAttack;

        [SerializeField] bool canAttack = true;
        [SerializeField] bool inWaitTime;

        Camera mainCam;
        public Transform OwnerTransform => ownerTransform;
        public bool CanAttack => canAttack;
        public bool InWaitTime => inWaitTime;
        public Entity_AttackData AttackData => attackData;
        public AttackModule CurrentAttack => currentAttack;

        void OnEnable()
        {
            if (!attackData) return;

            currentAttack = attackData.attackModules[0];

            currentAttack.inputBind.action.performed += EnterState;
        }
        void OnDisable()
        {
            if (!attackData || !Equals(currentAttack)) return;

            currentAttack.inputBind.action.performed -= EnterState;
        }
        void Start()
        {
            playerTrans = FindAnyObjectByType<Player_Movement>().transform;
            playerInput = playerTrans.GetComponent<Player_InputHandler>();

            mainCam = Camera.main;
        }
        void Update()
        {
            if (!attackData) return;

            playerDirection = (playerTrans.position - transform.position).normalized;

            var mouseWorld = mainCam.ScreenToWorldPoint(playerInput.MousePos);
            mouseDirection = (mouseWorld - transform.position).normalized;
            mouseDirection.z = 0;
        }
        void EnterState(InputAction.CallbackContext context)
        {
            EnterMethod();
        }
        public void EnterMethod()
        {
            if (canAttack && !inWaitTime)
                StartCoroutine(IEnter(currentAttack));
        }
        public IEnumerator IEnter(AttackModule mod)
        {
            if (!canAttack || inWaitTime) yield break;

            yield return new WaitForSeconds(mod.startDelay);

            OnStart?.Invoke();

            switch (mod.dirType)
            {
                case DireType.Mouse:
                    {
                        mod.module.direction = (mouseDirection);
                        break;
                    }
                case DireType.Entity:
                    {
                        mod.module.direction = (entityDirection);
                        break;
                    }
                case DireType.ToPlayer:
                    {
                        mod.module.direction = (playerDirection);
                        break;
                    }
                default:
                    {
                        mod.module.direction = (entityDirection);
                        break;
                    }
            }

            state = AttackState.Execute;
            StartCoroutine(IExecuter(mod));

            mod.module.Enter(this);

            SetCanAttack(false);
        }
        public IEnumerator IExecuter(AttackModule mod)
        {
            var _time = mod.duration;

            while (_time > 0)
            {
                _time -= Time.deltaTime;

                OnExecute?.Invoke();

                switch (mod.dirType)
                {
                    case DireType.Mouse:
                        {
                            mod.module.direction = (mouseDirection);
                            break;
                        }
                    case DireType.Entity:
                        {
                            mod.module.direction = (entityDirection);
                            break;
                        }
                    case DireType.ToPlayer:
                        {
                            mod.module.direction = (playerDirection);
                            break;
                        }
                    default:
                        {
                            mod.module.direction = (entityDirection);
                            break;
                        }
                }

                mod.module.Execute(this);

                yield return null; 
            }

            yield return new WaitForSeconds(mod.endDelay);

            Finish(mod);
            state = AttackState.Finished;
        }
        public void Finish(AttackModule mod)
        {
            OnFinish?.Invoke();

            mod.module.Exit(this);

            SetCanAttack(true);
            state = AttackState.Idle;
        }
        public void NotifyHit(Collider2D col)
        {
            OnHitEvent?.Invoke(col);

            foreach (var mod in attackData.attackModules)
                mod.module.Hit(this, col);
        }

        public void SetAttackData(Entity_AttackData attackData) => this.attackData = attackData;
        public void SetCanAttack(bool value) => canAttack = value;
        public void SetEntityDirection(Vector3 value) => entityDirection = value;
        public void SetInWaitTime(bool value) => inWaitTime = value;
        public void SetCurrentAttack(AttackModule mod) => currentAttack = mod;
        /*public IEnumerator IAwaitTimer(float time)
        {
            if (time is 0)
            {
                UnityEngine.Debug.Log("no time left"); 
                yield break;
            }

            UnityEngine.Debug.Log("wait time");

            inWaitTime = false;

            yield return new WaitForSeconds(time);

            UnityEngine.Debug.Log("end wait time");
            inWaitTime = true;
        }*/
    }
}
