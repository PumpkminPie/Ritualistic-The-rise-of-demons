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

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += 180f;

        //if (MathF.Abs(dir.x) > 1 && MathF.Abs(dir.y) > 1)
        transform.rotation = Quaternion.Euler(0, 0, angle);
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
