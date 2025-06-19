using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BrocolliBullet : MonoBehaviour
{
    private Transform Target;
    [SerializeField] private float Speed = 5f;
    [SerializeField] private Rigidbody2D RigidBody;
    [SerializeField] private int Damage = 1;
    public SpriteRenderer bulletSprite;
    public GameObject Tower;

    private void Start()
    {
        transform.localScale = new Vector3(1 * LeverManager.main.Scale * 0.3f, 1 * LeverManager.main.Scale * 0.3f, 1);
        if(Tower.GetComponent<Tower>().GetLevel() == 3) bulletSprite.sprite = Resources.Load<Sprite>("Sprites/Tower/brocolli/bullet_3");
    }

    public void SetTarget(Transform NewTarget, int NewDamage)
    {
        Target = NewTarget;
        Damage = NewDamage;
    }

    public void SetDamage(int NewDamage)
    {
        Damage = NewDamage;
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
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg-90;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        if(transform.position == Target.position && Target.GetComponent<Enermy>().GetIsDie())
        {
            Destroy(gameObject);
        }    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enermy enemy = collision.gameObject.GetComponent<Enermy>();
        if (enemy != null && !enemy.GetIsDie())
        {
            enemy.SetDamage(Damage);
            Destroy(gameObject);
        }
    }
}
