using Game.PoolSystem;
using UnityEngine;
using System.Collections;
using Game.PoolSystem.Object;
using System;

namespace Game.Entity.Attack.FireBall
{
    [CreateAssetMenu(menuName = "Entitys/Combat/AttackModule/Fire Ball")]
    public class Entity_AttackModule_FireBall : Entity_AttackModule
    {
        public Entity_AttackModule_FireBall(Action<Entity_AttackModule> OnEnterLogic = null,
            Action<Entity_AttackModule> OnExecuteLogic = null,
            Action<Entity_AttackModule> OnExitLogic = null,
            Action<Entity_AttackModule> OnCollideLogic = null,
            Vector2 direction = default,
            ExecutionPhase ExecutePhase = ExecutionPhase.Update) : base(OnEnterLogic, OnExecuteLogic, OnExitLogic, OnCollideLogic, direction, ExecutePhase)
        {
        }

        public GameObject prefabFireBall;
        public float speed = 5;
        public float timeToDelete = 5;
        public float range = 5;

        //public Entity_AttackModule_FireBall(Entity_AttackRunner executer) : base(executer){}

        public override void Enter(Entity_AttackRunner runner)
        { 
            base.Enter(runner);

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
                runner.OwnerTransform.position,
                Quaternion.identity
            );

            //PoolManager.Instance.maxActive[obj] = maxActives;

            if (obj.TryGetComponent<Projectiles_BasicScript>(out var script))
            {
                script.Init(direction, speed, range, timeToDelete, runner);
            }

            //Debug.Log(direction);
            
        }

        public override void Execute(Entity_AttackRunner runner)
        {
            base.Execute(runner);
        }

        public override void Hit(Entity_AttackRunner runner, Collider2D target)
        {
            base.Hit(runner, target);
        }
    }
}
