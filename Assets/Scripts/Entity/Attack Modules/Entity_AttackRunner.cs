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

    [Serializable]
    public struct ModuleAttacks
    {
        public Entity_AttackModule attackModule;
        public List<GameObject> savedData;
    }

    public class Entity_AttackRunner : MonoBehaviour
    {
        public event Action<Collider2D> OnHitEvent;
        public event Action<Vector3> OnAir;
        public event Action OnStart;
        public event Action OnExecute;
        public event Action OnFinish;

        public Vector3 mouseDirection;
        public Vector3 entityDirection;
        public Vector3 playerDirection;

        public Transform playerTrans;
        public PlayerInputHandler playerInput;

        public Transform ownerTransform;

        public Entity_AttackData attackData;
        public AttackState state;

        public bool canAttack = true;
        //[SerializeField] bool isPlayer;
        //public ModuleAttacks[] modulesData;

        Camera mainCam;


        void OnEnable()
        {
            if (!attackData) return;

            foreach (var mod in attackData.attackModules)
            {
                if (mod.inputBind)
                    mod.inputBind.action.performed += EnterState;
            }
        }
        void OnDisable()
        {
            if (!attackData) return;

            foreach (var mod in attackData.attackModules)
            {
                if (mod.inputBind)
                    mod.inputBind.action.performed -= EnterState;
            }
        }
        void EnterState(InputAction.CallbackContext context)
        {
            EnterMethod();
        }

        public void EnterMethod()
        {
            if (canAttack)
                StartCoroutine(Enter(attackData));
        }

        private void Start()
        {
            playerTrans = FindAnyObjectByType<Player_Movement>().transform;
            playerInput = playerTrans.GetComponent<PlayerInputHandler>();

            mainCam = Camera.main;
        }


        void Update()
        {
            if (!attackData) return;

            playerDirection = (playerTrans.position - transform.position).normalized;

            var mouseWorld = mainCam.ScreenToWorldPoint(playerInput.MousePos);
            mouseDirection = (mouseWorld);
            mouseDirection.z = 0;
        }

        public IEnumerator Enter(Entity_AttackData attackData)
        {
            if (!canAttack) yield break;

            //state = AttackState.Started;

            foreach (var mod in attackData.attackModules)
            {
                //state = AttackState.Delay;
                
                yield return new WaitForSeconds(mod.startDelay);

                mod.module.OnStart(this);

                //Executer(currentAttackData);

                OnStart?.Invoke();

                this.attackData = attackData;
                //Debug.Log("started");
                state = AttackState.Execute;
                StartCoroutine(Executer(this.attackData));

                switch (mod.dirType)
                {
                    case DireType.Mouse:
                        {
                            mod.module.direction = (mouseDirection).normalized;
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
                            mod.module.direction = (Vector2.one);
                            break;
                        }
                }
            }

            canAttack = false;
        }

        public IEnumerator Executer(Entity_AttackData attackData)
        {
            foreach (var mod in attackData.attackModules)
            {
                var _time = mod.duration;

                //Debug.Log(_time);
                //Debug.Log("phase 1");

                while (_time > 0)
                {
                    _time -= Time.deltaTime;

                    OnExecute?.Invoke();

                    //Debug.Log("phase 2");

                    /*if (mod.changeDireOnAir)
                    {*/
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
                                mod.module.direction = (Vector2.one);
                                break;
                            }
                    }
                    //Debug.Log("phase 2.5");

                    mod.module.OnExecute(this);
                    //}

                    yield return null; 
                }

                //Debug.Log("executor");

                //Debug.Log("phase 3");

                yield return new WaitForSeconds(mod.endDelay);

                //Debug.Log("phase 4");
                Finish(attackData);
                state = AttackState.Finished;
            }
        }
        public void Finish(Entity_AttackData attackData)
        {
            /*foreach (var mod in attackData.attackModules)
                mod.module.O*/

            OnFinish?.Invoke();

            foreach (var mod in attackData.attackModules)
            {
                mod.module.OnFinish(this);
            }
            canAttack = true;
            state = AttackState.Idle;
        }


        public void NotifyHit(Collider2D col)
        {
            OnHitEvent?.Invoke(col);

            foreach (var mod in attackData.attackModules)
                mod.module.OnHit(this, col);

            //Debug.Log("colidiu 1");
        }

        public void SetAttackData(Entity_AttackData attackData) => this.attackData = attackData;
    }
}
