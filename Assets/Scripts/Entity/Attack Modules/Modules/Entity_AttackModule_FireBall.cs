using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Game.Entity.Attack
{
    [CreateAssetMenu(menuName = "Entitys/Combat/AttackModule/Fire Ball")]
    public class Entity_AttackModule_FireBall : Entity_AttackModule
    {
        public GameObject prefabFireBall;
        public float speed = 5;
        public float timeToDelete = 5;
        public float range = 5;
        public Vector3 direction;

        //public Entity_AttackModule_FireBall(Entity_AttackRunner executer) : base(executer){}

        public override void OnStart(Entity_AttackRunner executor)
        { 
            //GameObject fireBall = null;

            direction = executor.mouseDirection;

            var _obj = executor.CreateObjData(prefabFireBall, executor.ownerTransform.position, Quaternion.identity);

            Destroy(_obj, timeToDelete);

            if (_obj.TryGetComponent<Projectiles_BasicScript>(out var script))
            {
                script.SetDirection(direction);
                script.SetSpeed(speed);
                script.SetRange(range);
            }
            //Debug.Log(direction);
        }

        public override void OnExecute(Entity_AttackRunner executor)
        {
            direction = executor.mouseDirection.normalized;

            for (int i = executor.modulesData.Length - 1; i >= 0; i--)
            {
                foreach (var data in executor.modulesData[i].savedData)
                {
                    var obj = data.gameObject;

                    if (obj == null)
                    {
                        executor.modulesData[i].savedData.RemoveAt(i);
                        continue;
                    }

                    if (obj.TryGetComponent<Projectiles_BasicScript>(out var script))
                    {
                        script.SetDirection(direction);
                        script.SetSpeed(speed);
                        script.SetRange(range);
                    }
                }
            }
        }

        public override void OnFinish(Entity_AttackRunner executor)
        {
            for (int i = executor.modulesData.Length - 1; i >= 0; i--)
            {
                executor.modulesData[i].savedData.Clear();
            }
        }

        public override void OnHit(Entity_AttackRunner executor, Collider2D target)
        {
            for (int i = executor.modulesData.Length - 1; i >= 0; i--)
            {
                foreach (var data in executor.modulesData[i].savedData)
                {
                    var obj = data.gameObject;

                    if (obj == null)
                    {
                        executor.modulesData[i].savedData.RemoveAt(i);
                        continue;
                    }

                    if (obj == target.gameObject)
                    {
                        Object.Destroy(obj);
                        executor.modulesData[i].savedData.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        /*void OnAirHandler(Vector3 dir)
        {
            direction = dir.normalized;
        }*/

        void OnDisable()
        {
            direction = Vector3.zero;
        }
    }
}
