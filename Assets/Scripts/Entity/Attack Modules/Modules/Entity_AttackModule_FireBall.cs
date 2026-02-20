using UnityEngine;

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
            for (int i = 0; i < executor.savedData.Count; i++) 
            {
                var _obj = executor.savedData[i];

                if (_obj == null)
                {
                    executor.savedData.Remove(_obj);
                    continue;
                }

                if (_obj.TryGetComponent<Projectiles_BasicScript>(out var script))
                {
                    script.SetDirection(direction);
                    script.SetSpeed(speed);
                    script.SetRange(range);
                }
                //Debug.Log("achou");

            }
            //else
                //Debug.Log("n achou");

            //Debug.Log("execute");

        }

        public override void OnHit(Entity_AttackRunner executor, Collider2D target)
        {
            for (int i = 0; i < executor.savedData.Count; i++)
            {
                var obj = executor.savedData[i];

                if(obj == target.gameObject)
                    Destroy(obj);
            }
        }

        /*void OnAirHandler(Vector3 dir)
        {
            direction = dir.normalized;
        }*/
    }
}
