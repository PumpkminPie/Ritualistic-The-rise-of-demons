using Game.Entity.Enemy.StateMachine.States;
using System;
using UnityEngine;

namespace Game.Entity.Enemy.StateMachine.States
{
    public class Enemy_Basic_Idle : Enemy_Basic_States
    {
        public Enemy_Basic_Idle(Enemy_Basic_StateMachine main_enemy,
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
        }

        public override void Execute()
        {
            base.Execute();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
