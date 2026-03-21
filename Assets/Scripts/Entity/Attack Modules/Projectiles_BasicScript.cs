using Game.Entity.Attack;
using Game.PoolSystem;
using Game.PoolSystem.Object;
using System;
using System.Collections;
using UnityEngine;

public class Projectiles_BasicScript : MonoBehaviour
{
    [SerializeField] Vector3 direction;
    [SerializeField] float speed;
    [SerializeField] float range;
    [SerializeField] float timeToDelete;
    [SerializeField] LayerMask colLayers;

    public Entity_AttackRunner owner;

    TrailRenderer trail;

    float timer;

    /*[NonSerialized]
    public PooledObject pooled;*/

    //public event Action OnCollisionEnter;

    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    void OnDisable()
    {
        trail.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += 180f;

        //if (MathF.Abs(dir.x) > 1 && MathF.Abs(dir.y) > 1)
        transform.rotation = Quaternion.Euler(0, 0, angle);

        timer -= Time.deltaTime;

        if (timer <= 0)
            HideObj();
    }

    public void HideObj()
    {
        PoolManager.Instance.Release(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (colLayers == (colLayers | (1 << other.gameObject.layer)))
        {
            HideObj();
            owner.NotifyHit(other);
        }
    }

    public void Init(Vector3 direction, float speed, float range, float timeToDelete, Entity_AttackRunner owner)
    {
        this.direction = direction;
        this.speed = speed;
        this.range = range;
        this.timeToDelete = timeToDelete;
        this.owner = owner;

        timer = timeToDelete;
    }
}
