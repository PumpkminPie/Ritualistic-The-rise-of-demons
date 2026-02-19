using UnityEngine;

namespace Game.Entity.Attack
{
    [CreateAssetMenu(menuName = "Entitys/Combat/AttackModule/Fire Ball")]
    public class Entity_AttackModule_FireBall : Entity_AttackModule
    {
        public GameObject prefabFireBall;

        public override void OnStart(Entity_AttackRunner executor)
        {
            GameObject fireBall = null;

            if (!fireBall)
                fireBall = executor.CreateObjData(prefabFireBall.GetInstanceID(), prefabFireBall, executor.ownerTransform.position, Quaternion.identity);
        }
        
        public override void OnExecute(Entity_AttackRunner executor)
        {
            GameObject obj = null;
            executor.savedData.TryGetValue(prefabFireBall.GetInstanceID(), out obj);

            executor.OnAir += (dir) => obj.transform.position += dir * Time.deltaTime;
        }

        public override void OnHit(Entity_AttackRunner executor, Collider2D target)
        {
            GameObject obj = null;
            executor.savedData.TryGetValue(prefabFireBall.GetInstanceID(), out obj);

            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }
}
