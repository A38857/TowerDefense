using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatusBullet : MonoBehaviour
{
    private Transform Target;
    [SerializeField] private float Speed = 5f;
    [SerializeField] private Rigidbody2D RigidBody;
    private int Damage = 1;
    private float SlowDown = 0.9f;
    private float TimeSlow = 0.9f;

    private void Start()
    {
        transform.localScale = new Vector3(1 * LeverManager.main.Scale * 0.3f, 1 * LeverManager.main.Scale * 0.3f, 1);
    }

    public void SetTarget(Transform NewTarget,int NewDamage, float Slow,float TimeS)
    {
        Target = NewTarget;
        Damage = NewDamage;
        SlowDown = Slow;
        TimeSlow = TimeS;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Target)
        {
            Destroy(gameObject);
            return;
        }
        Vector2 Direction = (Target.position - transform.position).normalized;
        RigidBody.velocity = Direction * Speed;
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg - 45;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enermy enemy = collision.gameObject.GetComponent<Enermy>();
        if (enemy != null && !enemy.GetIsDie())
        {
            enemy.SetDamage(Damage);
            enemy.SetSpeed(SlowDown, TimeSlow);
            Destroy(gameObject);
        }
    }
}
