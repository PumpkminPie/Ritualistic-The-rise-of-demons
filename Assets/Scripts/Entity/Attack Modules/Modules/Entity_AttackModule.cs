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

        public virtual void OnStart(Entity_AttackRunner executor) { }
        public virtual void OnExecute(Entity_AttackRunner executor) { }
        public virtual void OnFinish(Entity_AttackRunner executor) { }
        public abstract void OnHit(Entity_AttackRunner executor, Collider2D target);

        /*void OnHitEvent(Collider2D target, Entity_AttackRunner executor)
        {
            OnHit(executor, target);
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
