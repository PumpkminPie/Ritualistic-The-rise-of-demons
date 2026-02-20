using UnityEngine;

public class Projectiles_BasicScript : MonoBehaviour
{
    [SerializeField] Vector3 direction;
    [SerializeField] float speed;
    [SerializeField] float range;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, direction * range, speed * Time.deltaTime);
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    public void SetRange(float range)
    {
        this.range = range;
    }
}
