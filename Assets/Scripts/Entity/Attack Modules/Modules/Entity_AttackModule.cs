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

        protected event Action OnCollide;
        public Vector2 direction;

        public virtual void OnStart(Entity_AttackRunner executor) { }
        public virtual void OnExecute(Entity_AttackRunner executor) { }
        public virtual void OnFinish(Entity_AttackRunner executor) { }
        public virtual void OnHit(Entity_AttackRunner executor, Collider2D target) { OnCollide?.Invoke(); }

        public void CheckCollision(Entity_AttackRunner executor, Collider2D col)
        {
            
            Collider2D[] hits = Physics2D.OverlapBoxAll(
            col.bounds.center,
            col.bounds.extents,
            0f
            );

            if (hits.Length > 0)
                executor.NotifyHit(col);
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
