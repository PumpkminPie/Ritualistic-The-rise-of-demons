using Game.Entity.Enemy.StateMachine;
using System;
using UnityEngine;

namespace Game.Entity.Attack
{
    public abstract class Entity_AttackModule : ScriptableObject
    {
        /*protected Entity_AttackRunner executor;

        protected Entity_AttackModule(Entity_AttackRunner executor)
        {
            this.executor = executor;
        }*/
        protected readonly Action<Entity_AttackModule> OnEnterLogic;
        protected readonly Action<Entity_AttackModule> OnExecuteLogic;
        protected readonly Action<Entity_AttackModule> OnExitLogic;
        protected readonly Action<Entity_AttackModule> OnCollideLogic;

        public Vector2 direction;

        public readonly ExecutionPhase ExecutePhase;

        public Entity_AttackModule(Action<Entity_AttackModule> OnEnterLogic = null,
            Action<Entity_AttackModule> OnExecuteLogic = null,
            Action<Entity_AttackModule> OnExitLogic = null,
            Action<Entity_AttackModule> OnCollideLogic = null,
            Vector2 direction = default,
            ExecutionPhase ExecutePhase = ExecutionPhase.Update
            )
        {
            this.OnEnterLogic = OnEnterLogic;
            this.OnExecuteLogic = OnExecuteLogic;
            this.OnExitLogic = OnExitLogic;
            this.OnCollideLogic = OnCollideLogic;
            this.direction = direction;
            this.ExecutePhase = ExecutePhase;
        }

        public virtual void Enter(Entity_AttackRunner runner) 
        { 
            OnEnterLogic?.Invoke(this);
        }
        public virtual void Execute(Entity_AttackRunner runner) 
        { 
            OnExecuteLogic?.Invoke(this);
        }
        public virtual void Exit(Entity_AttackRunner runner)
        {
            OnExitLogic?.Invoke(this);
        }
        public virtual void Hit(Entity_AttackRunner runner, Collider2D target) 
        { 
            OnCollideLogic?.Invoke(this); 
        }

        public void CheckCollision(Entity_AttackRunner runner, Collider2D col)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(
            col.bounds.center,
            col.bounds.extents,
            0f
            );

            if (hits.Length > 0)
                runner.NotifyHit(col);
        }
        /*void OnHitEvent(Collider2D target, Entity_AttackRunner executor)
        {
            OnCollide(executor, target);
        }

        private void OnEnable()
        {
            executor.OnHitEvent += OnHitEvent;
        }
        private void OnDisable()
        {
            executor.OnHitEvent -= OnHitEvent;
        }*/
    }
}
