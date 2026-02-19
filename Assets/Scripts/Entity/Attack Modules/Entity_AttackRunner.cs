using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity.Attack
{
    public enum AttackState
    {
        Started,
        Delay,
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

        public Transform ownerTransform;

        public Entity_AttackData currentAttack;
        public AttackState state;

        public Dictionary<int, GameObject> savedData = new Dictionary<int, GameObject>();

        void OnEnable()
        {
            foreach (var mod in currentAttack.attackModules)
            {
                mod.inputBind.action.performed += ctx => StartCoroutine(Enter(currentAttack));
            }
        }

        void Update()
        {
            if (!currentAttack) return;

            switch (state)
            {
                case AttackState.Started:
                    {
                        Enter(currentAttack);
                        break;
                    }
                /*case AttackState.Delay:
                    {
                        break;
                    }*/
                case AttackState.Execute:
                    {
                        Executer(currentAttack);
                        break;
                    }
                case AttackState.Finished:
                    {
                        Finish(currentAttack);
                        break;
                    }
            }
        }

        public IEnumerator Enter(Entity_AttackData attackData)
        {
            foreach (var mod in attackData.attackModules)
            {
                state = AttackState.Delay;
                
                yield return new WaitForSeconds(mod.startDelay);

                OnStart?.Invoke();

                state = AttackState.Started;

                currentAttack = attackData;
                OnAir?.Invoke(mod.dirType == DirType.Mouse ? mouseDirection : entityDirection);
            }
        }

        public IEnumerator Executer(Entity_AttackData attackData)
        {
            foreach (var mod in attackData.attackModules)
            {
                var _time = mod.duration;

                _time -= Time.deltaTime;

                while (_time > 0)
                {
                    OnExecute?.Invoke();

                    state = AttackState.Execute;

                    if (mod.changeDireOnAir)
                        OnAir?.Invoke(mod.dirType == DirType.Mouse ? mouseDirection : entityDirection);
                }

                yield return new WaitForSeconds(mod.endDelay);
            }
        }
        public void Finish(Entity_AttackData attackData)
        {
            state = AttackState.Finished;
        }


        public void NotifyHit(Collider2D col)
        {
            OnHitEvent?.Invoke(col);
        }

        public GameObject CreateObjData(int data, GameObject obj, Vector3 pos, Quaternion rot)
        {
            var _ob = Instantiate(obj, pos, rot);

            if (!savedData.ContainsKey(data))
                savedData.Add(data, _ob);

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
