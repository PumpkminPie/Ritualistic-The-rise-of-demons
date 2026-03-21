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
        Enemy_Basic_States currentState;

        [SerializeField] Enemy_InfoAsset infoAsset;

        Enemy_Basic_States idleState;
        Enemy_Basic_States chaseState;
        Enemy_Basic_States attackState;

        [SerializeField] Player_Movement playerMove;
        [SerializeField] Entity_AttackRunner attackRunner;
        [SerializeField] Enemy_Basic_Sensor sensor;

        [SerializeField] bool canChase = true;
        public Enemy_InfoAsset InfoAsset => infoAsset;
        public Player_Movement Player => playerMove;
        public Entity_AttackRunner AttackRunner => attackRunner;
        public Enemy_Basic_Sensor Sensor => sensor;
        public bool CanChase => canChase;

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

            if ((attackRunner.CanAttack && !attackRunner.InWaitTime))
                sensor.OnPlayerStayInAttackArea += (() => ChangeState(attackState));

            sensor.OnLostPlayer += (() => ChangeState(idleState));
        }

        private void OnDisable()
        {
            sensor.OnDetectPlayer -= (() => ChangeState(chaseState));
            sensor.OnLostPlayer -= (() => ChangeState(idleState));

            sensor.OnPlayerStayInAttackArea -= (() => ChangeState(attackState));

            if (attackRunner.InWaitTime && currentState != idleState)
                ChangeState(idleState);
        }

        void Update()
        {
            currentState.Execute();
        }

        public void ChangeState(Enemy_Basic_States state)
        {
            if (currentState == state) return;
            
            currentState.Exit();
            currentState = state;
            currentState.Enter();
        }


        private void OnDrawGizmos()
        {
            if (currentState != null)
                CustomTextGizmo.DrawText(currentState.GetType().Name, transform.position + Vector3.up * 2.5f, Color.red);
        }
    }
}
