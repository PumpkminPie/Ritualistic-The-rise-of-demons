using Game.Entity.Health;
using System;
using UnityEngine;

public enum ExecutionPhase
{
    Update,
    FixedUpdate,
    LateUpdate
}
namespace Game.Entity.Enemy.StateMachine.States
{
    public abstract class Enemy_Basic_States
    {
        protected readonly Enemy_Basic_StateMachine main_enemy;

        protected readonly Action<Enemy_Basic_States> OnEnterLogic;
        protected readonly Action<Enemy_Basic_States> OnExecuteLogic;
        protected readonly Action<Enemy_Basic_States> OnExitLogic;

        protected readonly Func<Enemy_Basic_States, bool> CanExit;

        public readonly ExecutionPhase ExecutePhase;

        public Enemy_Basic_States(Enemy_Basic_StateMachine main_enemy,
            Action<Enemy_Basic_States> OnEnterLogic = null,
            Action<Enemy_Basic_States> OnExecuteLogic = null,
            Action<Enemy_Basic_States> OnExitLogic = null,
            Func<Enemy_Basic_States, bool> CanExit = null,
            ExecutionPhase ExecutePhase = ExecutionPhase.Update
            )
        {
            this.main_enemy = main_enemy;
            this.OnEnterLogic = OnEnterLogic;
            this.OnExecuteLogic = OnExecuteLogic;
            this.OnExitLogic = OnExitLogic;
            this.CanExit = CanExit ?? (_ => true);
            this.ExecutePhase = ExecutePhase;
        }

        public virtual void Enter()
        {
            OnEnterLogic?.Invoke(this);
        }
        public virtual void Execute()
        {
            OnExecuteLogic?.Invoke(this);
        }
        public virtual void Exit()
        {
            if (CanExit(this))
                OnExitLogic?.Invoke(this);
        }
    }
}