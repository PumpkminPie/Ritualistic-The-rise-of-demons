using Game.Entity.Player;
using Game.Entity.Player.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public event Action<Collider2D> OnHitEvent;
        public event Action<Vector3> OnAir;
        public event Action OnStart;
        public event Action OnExecute;

        public Vector3 mouseDirection;
        public Vector3 entityDirection;
        public Vector3 playerDirection;

        public Transform playerTrans;
        public PlayerInputHandler playerInput;

        public Transform ownerTransform;

        public Entity_AttackData currentAttack;
        public AttackState state;

        public bool canAttack = true;
        [SerializeField] bool isPlayer;

        Camera mainCam;

        public List<GameObject> savedData = new List<GameObject>();

        void OnEnable()
        {
            foreach (var mod in currentAttack.attackModules)
            {
                mod.inputBind.action.performed += ctx => StartCoroutine(Enter(currentAttack));
            }
        }

        private void Start()
        {
            playerTrans = FindAnyObjectByType<Player_Movement>().transform;
            playerInput = playerTrans.GetComponent<PlayerInputHandler>();

            mainCam = Camera.main;
        }


        void Update()
        {
            if (!currentAttack) return;

            playerDirection = (playerTrans.position - transform.position).normalized;

            if (isPlayer)
                mouseDirection = mainCam.ScreenToWorldPoint(playerInput.MousePos) - transform.position;

            switch (state)
            {
                case AttackState.Started:
                    {
                        StartCoroutine(Enter(currentAttack));
                        break;
                    }
                /*case AttackState.Delay:
                    {
                        break;
                    }*/
                case AttackState.Execute:
                    {
                        StartCoroutine(Executer(currentAttack));
                        //Debug.Log("execute");
                        break;
                    }
                case AttackState.Finished:
                    {
                        Finish(currentAttack);
                        break;
                    }
                case AttackState.Idle:
                    {
                        break;
                    }
            }
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

                //Executer(currentAttack);

                OnStart?.Invoke();

                currentAttack = attackData;
                //Debug.Log("started");
                state = AttackState.Execute;

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
                
                while (_time > 0)
                {
                    _time -= Time.deltaTime;

                    OnExecute?.Invoke();

                    if (mod.changeDireOnAir)
                    {
                        if (mod.dirType == DireType.Mouse)
                            OnAir?.Invoke(mouseDirection);
                        else if (mod.dirType == DireType.Entity)
                            OnAir?.Invoke(entityDirection);
                        else
                            OnAir?.Invoke(playerDirection);
                    }

                    mod.module.OnExecute(this);

                    yield return null; 
                }

                //Debug.Log("executer");

                yield return new WaitForSeconds(mod.endDelay);

                state = AttackState.Finished;
            }
        }
        public void Finish(Entity_AttackData attackData)
        {
            /*foreach (var mod in attackData.attackModules)
                mod.module.O*/

            state = AttackState.Idle;
            canAttack = true;
        }


        public void NotifyHit(Collider2D col)
        {
            OnHitEvent?.Invoke(col);

            foreach (var mod in currentAttack.attackModules)
                mod.module.OnHit(this, col);
        }

        public GameObject CreateObjData(GameObject obj, Vector3 pos, Quaternion rot)
        {
            var _ob = Instantiate(obj, pos, rot);

            if (!savedData.Contains(obj))
                savedData.Add(_ob);

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
