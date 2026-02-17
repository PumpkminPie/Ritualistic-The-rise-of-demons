using UnityEngine;
using Game.Character.Enemy.StateMachine.States;
using Game.Character.Player.Movement;

namespace Game.Character.Enemy.StateMachine
{
    public class Enemy_Basic_StateMachine : MonoBehaviour
    {
        public Enemy_Basic_States currentState;

        public Enemy_Basic_States idleState;
        public Enemy_Basic_States chaseState;
        public Enemy_Basic_States attackState;

        public Player_Movement playerMove;

        private void Awake()
        {
            chaseState = new Enemy_Basic_Chase(this);
            idleState = new Enemy_Basic_Idle(this);

            currentState = idleState;
        }
        private void Start()
        {
            playerMove = FindAnyObjectByType<Player_Movement>();
        }

        private void Update()
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
