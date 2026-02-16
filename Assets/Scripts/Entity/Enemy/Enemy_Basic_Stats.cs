using UnityEngine;

namespace Game.Character.Enemy.StateMachine.States
{
    public abstract class Enemy_Basic_Stats
    {
        protected Enemy_Basic_StateMachine enemy;
        protected Enemy_Basic_Stats(Enemy_Basic_StateMachine enemy) { }

        public abstract void Enter(Enemy_Basic_StateMachine enemy);
        public abstract void Execute(Enemy_Basic_StateMachine enemy);
        public abstract void Exit(Enemy_Basic_StateMachine enemy);
    }
}