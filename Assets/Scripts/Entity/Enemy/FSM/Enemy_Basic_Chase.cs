using Game.Entity.Enemy.StateMachine;
using Game.Entity.Enemy.StateMachine.States;
using System;
using UnityEngine;

namespace Game.Entity.Enemy.StateMachine.States
{
    public class Enemy_Basic_Chase : Enemy_Basic_States
    {
        public Enemy_Basic_Chase(Enemy_Basic_StateMachine main_enemy,
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

            GoTo(main_enemy, main_enemy.Player.transform, main_enemy.InfoAsset.maxVel);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public void GoTo(Enemy_Basic_StateMachine enemy, Transform target, float vel)
        {
            Vector2 velocity = Vector2.one, currentVel = Vector2.one;

            enemy.transform.position = Vector2.Lerp(enemy.transform.position, target.position, vel * Time.deltaTime);
        }

    }
}
