using Game.Entity.Attack;
using Game.Entity.Enemy.StateMachine.Sensor;
using Game.Entity.Enemy.StateMachine.States;
using Game.Entity.Player.Movement;
using Game.Debug;
using UnityEditor;
using UnityEngine;

namespace Game.Entity.Enemy.StateMachine
{
    public class Enemy_Basic_StateMachine : MonoBehaviour
    {
        public Enemy_Basic_States currentState;

        public Enemy_InfoAsset infoAsset;

        public Enemy_Basic_States idleState;
        public Enemy_Basic_States chaseState;
        public Enemy_Basic_States attackState;

        public Player_Movement playerMove;
        public Entity_AttackRunner attackRunner;
        public Enemy_Basic_Sensor sensor;

        void Awake()
        {
            chaseState  = new Enemy_Basic_Chase(this);
            idleState   = new Enemy_Basic_Idle(this);
            attackState = new Enemy_Basic_MeleeAttack(this);

            currentState = idleState;
        }
        void Start()
        {
            playerMove = FindAnyObjectByType<Player_Movement>();

            sensor = GetComponent<Enemy_Basic_Sensor>();
            attackRunner = GetComponent<Entity_AttackRunner>();

            attackRunner.SetAttackData(infoAsset.attackData);

            sensor.OnDetectPlayer?.AddListener(() => ChangeState(chaseState));
            sensor.OnLostPlayer?.AddListener(() => ChangeState(idleState));
        }

        void Update()
        {
            currentState.Execute(this);

            if (attackRunner.canAttack)
            {
                foreach (var mod in infoAsset.attackData.attackModules)
                {
                    var _dist = transform.position - playerMove.transform.position;
                    var _dire = (transform.position - playerMove.transform.position).normalized;

                    if (_dist.magnitude <= mod.enemyPart.attackDistance)
                    {
                        if (attackRunner.canAttack)
                            ChangeState(attackState);

                        break;
                    } 
                }
            }
        }

        public void ChangeState(Enemy_Basic_States state)
        {
            if (currentState == state) return;
            
            currentState.Exit(this);
            currentState = state;
            currentState.Enter(this);
        }

        private void OnDrawGizmos()
        {
            if (currentState != null)
                CustomTextGizmo.DrawText(currentState.GetType().Name, transform.position + Vector3.up * 2.5f, Color.red);
        }
    }
}
