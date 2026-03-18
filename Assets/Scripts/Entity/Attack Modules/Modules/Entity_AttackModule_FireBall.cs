using Game.PoolSystem;
using UnityEngine;
using System.Collections;
using Game.PoolSystem.Object;

namespace Game.Entity.Attack.FireBall
{
    [CreateAssetMenu(menuName = "Entitys/Combat/AttackModule/Fire Ball")]
    public class Entity_AttackModule_FireBall : Entity_AttackModule
    {
        public GameObject prefabFireBall;
        public float speed = 5;
        public float timeToDelete = 5;
        public float range = 5;

        //public Entity_AttackModule_FireBall(Entity_AttackRunner executer) : base(executer){}

        public override void OnStart(Entity_AttackRunner executor)
        { 
            //GameObject fireBall = null;

            /*direction = executor.mouseDirection.normalized;

            var _obj = executor.CreateObjData(prefabFireBall, this, executor.ownerTransform.position, Quaternion.identity);

            Destroy(_obj, timeToDelete);

            if (_obj.TryGetComponent<Projectiles_BasicScript>(out var script))
            {
                script.SetDirection(direction);
                script.SetSpeed(speed);
                script.SetRange(range);
            }*/

            var obj = PoolManager.Instance.Get(
                prefabFireBall,
                executor.ownerTransform.position,
                Quaternion.identity
            );

            //PoolManager.Instance.maxActive[obj] = maxActives;

            if (obj.TryGetComponent<Projectiles_BasicScript>(out var script))
            {
                script.Init(direction, speed, range, timeToDelete);
            }

            //Debug.Log(direction);
            
        }

        public override void OnExecute(Entity_AttackRunner executor)
        {
            /*var obj = PoolManager.Instance.Get(
                prefabFireBall,
                executor.ownerTransform.position,
                Quaternion.identity
            );

            //PoolManager.Instance.maxActive[obj] = maxActives;

            if (obj.TryGetComponent<Projectiles_BasicScript>(out var script))
            {
                script.Init(direction, speed, range, timeToDelete);
            }*/

            /*var obj = PoolManager.Instance.Get(
                prefabFireBall,
                executor.ownerTransform.position,
                Quaternion.identity
            );*/

            /*if (obj.TryGetComponent<Projectiles_BasicScript>(out var script))
            {
                script.Init(direction, speed, range, timeToDelete);
            }*/
        }

        public override void OnHit(Entity_AttackRunner executor, Collider2D target)
        {
            
        }

        void OnEnable()
        {
            direction = Vector3.zero;
        }
    }
}
