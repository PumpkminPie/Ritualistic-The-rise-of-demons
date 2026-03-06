using Game.PoolSystem.Object;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.PoolSystem
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance;

        Dictionary<GameObject, IObjectPool<GameObject>> pools = new(); 

        /*Dictionary<GameObject, int> activeCount = new();
        public Dictionary<GameObject, int> maxActive = new();*/

        void Awake()
        {
            Instance = this;
        }

        public GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            if (!pools.TryGetValue(prefab, out var pool))
            {
                pool = CreatePool(prefab);
                pools.Add(prefab, pool);
            }

            var obj = pool.Get();
            obj.transform.SetPositionAndRotation(pos, rot);

            //activeCount[prefab]++;

            return obj;
        }

        IObjectPool<GameObject> CreatePool(GameObject prefab)
        {
            return new ObjectPool<GameObject>(
                () =>
                {
                    var obj = Instantiate(prefab);
                    var pooled = obj.AddComponent<PooledObject>();
                    pooled.SetPool(pools[prefab]);
                    return obj;
                },
                obj => obj.SetActive(true),
                obj => obj.SetActive(false),
                obj => Destroy(obj),
                false,
                10,
                200
            );
        }

        public void Release(GameObject obj)
        {
            var pooled = obj.GetComponent<PooledObject>();
            pooled.Release();

            //activeCount[obj]--;
        }
    }
}