using Game.Entity.Enemy.StateMachine;
using Game.Entity.Enemy.StateMachine.States;
using System;
using UnityEngine;

namespace Game.Entity.Enemy.StateMachine.States
{

    public class Enemy_Basic_MeleeAttack : Enemy_Basic_States
    {
        public Enemy_Basic_MeleeAttack(Enemy_Basic_StateMachine main_enemy,
            Action<Enemy_Basic_States> OnEnterLogic = null,
            Action<Enemy_Basic_States> OnExecuteLogic = null,
            Action<Enemy_Basic_States> OnExitLogic = null,
            Func<Enemy_Basic_States, bool> CanExit = null,
            ExecutionPhase ExecutePhase = ExecutionPhase.Update) : base(main_enemy, OnEnterLogic, OnExecuteLogic, OnExitLogic, CanExit, ExecutePhase)
        {
        }

        public override void Enter()
        {
            base.Enter();

            if (main_enemy.AttackRunner.CanAttack is false) 
            {
                return; 
            }

            main_enemy.AttackRunner.EnterMethod();
            main_enemy.AttackRunner.SetCanAttack(false);
        }

        public override void Execute()
        {
            base.Execute();
        }

        public override void Exit()
        {
            base.Exit();

            /*foreach (var mod in main_enemy.AttackRunner.AttackData.attackModules)
            {
                main_enemy.AttackRunner.SetInWaitTime(true);
            }*/
        }
    }
}
