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

        public Entity_AttackData currentAttackData;
        public AttackState state;

        public bool canAttack = true;
        //[SerializeField] bool isPlayer;
        public ModuleAttacks[] modulesData;

        Camera mainCam;


        void OnEnable()
        {
            foreach (var mod in currentAttackData.attackModules)
            {
                mod.inputBind.action.performed += EnterMethod;
            }
        }
        void OnDisable()
        {
            foreach (var mod in currentAttackData.attackModules)
            {
                mod.inputBind.action.performed -= EnterMethod;
            }
        }

        void EnterMethod(InputAction.CallbackContext context)
        {
            if (canAttack)
                StartCoroutine(Enter(currentAttackData));
        }

        private void Start()
        {
            playerTrans = FindAnyObjectByType<Player_Movement>().transform;
            playerInput = playerTrans.GetComponent<PlayerInputHandler>();

            mainCam = Camera.main;
        }


        void Update()
        {
            if (!currentAttackData) return;

            playerDirection = (playerTrans.position - transform.position).normalized;

            var mouseWorld = Camera.main.ScreenToWorldPoint(playerInput.MousePos);
            mouseDirection = (mouseWorld - transform.position);
            //{ 
            /*switch (state)
            {*/
            /*case AttackState.Started:
                {
                    StartCoroutine(Enter(currentAttackData));
                    break;
                }*/
            /*case AttackState.Delay:
                {
                    break;
                }*/
            /*case AttackState.Execute:
                {*/
            //if (canAttack && state == AttackState.Execute)
            //Debug.Log("execute");
            //break;
                //}
            /*case AttackState.Finished:
                {
                    Finish(currentAttackData);
                    break;
                }*/
            /*case AttackState.Idle:
                {
                    break;
                }*/
            //} 
            //}
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

                currentAttackData = attackData;
                //Debug.Log("started");
                state = AttackState.Execute;
                StartCoroutine(Executer(currentAttackData));

                if (mod.dirType == DireType.Mouse)
                    OnAir?.Invoke(mouseDirection);
                else if (mod.dirType == DireType.Entity)
                    OnAir?.Invoke(entityDirection);
                else
                    OnAir?.Invoke(playerDirection);
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

                    if (mod.changeDireOnAir)
                    {
                        if (mod.dirType == DireType.Mouse)
                            OnAir?.Invoke(mouseDirection);
                        else if (mod.dirType == DireType.Entity)
                            OnAir?.Invoke(entityDirection);
                        else
                            OnAir?.Invoke(playerDirection);
                        //Debug.Log("phase 2.5");

                        mod.module.OnExecute(this);
                    }

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

            foreach (var mod in currentAttackData.attackModules)
                mod.module.OnHit(this, col);
        }

        public GameObject CreateObjData(GameObject obj, Vector3 pos, Quaternion rot)
        {
            var _ob = Instantiate(obj, pos, rot);

            foreach (var data in modulesData)
            if (!data.savedData.Contains(obj))
                data.savedData.Add(_ob);

            return _ob;
        }

        /*void OnCollisionEnter2D(Collision2D collision)
        {
            NotifyHit(collision.collider);
        }*/
        void OnTriggerEnter2D(Collider2D collision)
        {
            NotifyHit(collision);
        }
    }
}
