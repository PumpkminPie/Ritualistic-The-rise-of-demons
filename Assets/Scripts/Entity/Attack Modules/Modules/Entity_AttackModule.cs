using UnityEngine;

namespace Game.Entity.Attack
{
    public abstract class Entity_AttackModule : ScriptableObject
    {
        public virtual void OnStart(Entity_AttackRunner executor) { }
        public virtual void OnExecute(Entity_AttackRunner executor)
        {
            executor.OnHitEvent -= (target) => OnHit(executor, target);
            executor.OnHitEvent += (target) => OnHit(executor, target);
        }
        public abstract void OnHit(Entity_AttackRunner executor, Collider2D target);
    }
}
