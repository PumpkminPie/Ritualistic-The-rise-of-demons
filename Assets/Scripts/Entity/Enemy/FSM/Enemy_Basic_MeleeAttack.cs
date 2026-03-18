using Game.Entity.Enemy.StateMachine;
using Game.Entity.Enemy.StateMachine.States;
using UnityEngine;

namespace Game.Entity.Enemy.StateMachine.States
{

    public class Enemy_Basic_MeleeAttack : Enemy_Basic_States
    {
        public Enemy_Basic_MeleeAttack(Enemy_Basic_StateMachine enemy) : base(enemy) {}

        public override void Enter(Enemy_Basic_StateMachine enemy)
        {
            if (!enemy.attackRunner.canAttack) 
            {
                Execute(enemy); 
                return; 
            }

            enemy.attackRunner.EnterMethod();
        }

        public override void Execute(Enemy_Basic_StateMachine enemy)
        {
            enemy.attackRunner.Executer(enemy.attackRunner.attackData);
        }

        public override void Exit(Enemy_Basic_StateMachine enemy)
        {
            enemy.attackRunner.Finish(enemy.attackRunner.attackData);
        }
    }
}
