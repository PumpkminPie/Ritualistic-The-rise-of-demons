using UnityEngine;
using Game.Entity.Enemy.StateMachine.States;
using Game.Entity.Player.Movement;
using Game.Entity.Enemy.StateMachine.Sensor;

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

        public Enemy_Basic_Sensor sensor;

        void Awake()
        {
            chaseState = new Enemy_Basic_Chase(this);
            idleState = new Enemy_Basic_Idle(this);

            currentState = idleState;
        }
        void Start()
        {
            playerMove = FindAnyObjectByType<Player_Movement>();

            sensor = GetComponent<Enemy_Basic_Sensor>();

            sensor.OnDetectPlayer?.AddListener(() => ChangeState(chaseState));
            sensor.OnLostPlayer?.AddListener(() => ChangeState(idleState));
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
    }
}
