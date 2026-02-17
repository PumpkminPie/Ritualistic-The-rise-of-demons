using Unity.IntegerTime;
using UnityEngine;

namespace Game.Character.Enemy.StateMachine.States
{
    public abstract class Enemy_Basic_States
    {
        protected Enemy_Basic_StateMachine enemy;
        protected Enemy_Basic_States(Enemy_Basic_StateMachine enemy) { }

        public abstract void Enter(Enemy_Basic_StateMachine enemy);
        public abstract void Execute(Enemy_Basic_StateMachine enemy);
        public abstract void Exit(Enemy_Basic_StateMachine enemy);

        public void GoTo(Enemy_Basic_StateMachine enemy, Transform target, float vel) 
        {
            Vector2 velocity = Vector2.one, currentVel = Vector2.one;

            enemy.transform.position = Vector2.Lerp(enemy.transform.position, target.position, vel * Time.deltaTime);
        }
    }
}