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

    TrailRenderer trail;

    float timer;

    /*[NonSerialized]
    public PooledObject pooled;*/

    //public event Action OnCollisionEnter;

    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
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
            PoolManager.Instance.Release(gameObject);
    }

    /*public void SetDirection(Vector3 direction) => this.direction = direction;
    
    public void SetSpeed(float speed) => this.speed = speed;
    
    public void SetRange(float range) => this.range = range;/*
    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (colLayers == (colLayers | (1 << collision.gameObject.layer)))
        {
            DeleteObj();
            //OnCollisionEnter?.Invoke();
        }
        //Debug.Log(collision.gameObject.layer);
    }


    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (colLayers == (colLayers | (1 << collision.gameObject.layer)))
            DeleteObj();

        //Debug.Log(collision.gameObject.layer);
    }*/

    public void DeleteObj()
    {
        PoolManager.Instance.Release(gameObject);
    }
    /*public IEnumerator ILifeTimer()
    {
        yield return new WaitForSeconds(timeToDelete);
        /*var obj = PoolManager.Instance.Get(
                gameObject,
                executor.ownerTransform.position,
                Quaternion.identity
            );
    }*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((colLayers.value & (1 << other.gameObject.layer)) == 0)
            DeleteObj();
    }

    public void Init(Vector3 direction, float speed, float range, float timeToDelete)
    {
        this.direction = direction;
        this.speed = speed;
        this.range = range;
        this.timeToDelete = timeToDelete;

        timer = timeToDelete;
    }
}
