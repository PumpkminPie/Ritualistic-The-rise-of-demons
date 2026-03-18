using Game.Entity.Attack;
using Game.Entity.Enemy.StateMachine.Sensor;
using Game.Entity.Enemy.StateMachine.States;
using Game.Entity.Player.Movement;
using Game.Debug;
using UnityEditor;
using UnityEngine;
using System.Collections;

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

        public bool canChase = true;


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

            if (canChase)
                sensor.OnDetectPlayer += (() => ChangeState(chaseState));

            if (attackRunner.canAttack)
                sensor.OnPlayerStayInAttackArea += (() => ChangeState(attackState));

            sensor.OnLostPlayer += (() => ChangeState(idleState));
        }

        private void OnDisable()
        {
            sensor.OnDetectPlayer -= (() => ChangeState(chaseState));
            sensor.OnLostPlayer -= (() => ChangeState(idleState));

            sensor.OnPlayerStayInAttackArea -= (() => ChangeState(attackState));
        }

        void Update()
        {
            currentState.Execute(this);
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
