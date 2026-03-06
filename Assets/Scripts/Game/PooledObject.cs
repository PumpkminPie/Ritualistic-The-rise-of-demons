using UnityEngine;
using UnityEngine.Pool;

namespace Game.PoolSystem.Object
{
    public class PooledObject : MonoBehaviour
    {
        IObjectPool<GameObject> pool;

        public void SetPool(IObjectPool<GameObject> pool)
        {
            this.pool = pool;
        }

        public void Release()
        {
            pool.Release(gameObject);
        }
    }
}