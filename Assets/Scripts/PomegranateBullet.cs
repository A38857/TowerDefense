using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PomegranateBullet : MonoBehaviour
{
    private Transform Target;
    private Vector3 StartPosition;
    private Vector3 TargetPosition;

    [SerializeField] private float TravelTime = 1.0f;
    [SerializeField] private float ArcHeight = 2f;
    [SerializeField] private GameObject ExplosionEffect;
    public SpriteRenderer bulletSprite1;
    public SpriteRenderer bulletSprite2;
    public GameObject Tower;

    private int Damage = 1;
    private float Timer;
    private bool IsFlying = false;
    private bool HasHit = false;

    private void Start()
    {
        transform.localScale = new Vector3( 1 * LeverManager.main.Scale * 0.65f,1 * LeverManager.main.Scale * 0.65f,1);
        int Level = Tower.GetComponent<Tower>().GetLevel();
        int a = (Level <= 2) ? 1 : 2;
        if (Level >= 2) bulletSprite1.sprite = Resources.Load<Sprite>("Sprites/Tower/pomegranate/bullet_"+a+"/bullet_1");
        if (Level >= 2) bulletSprite2.sprite = Resources.Load<Sprite>("Sprites/Tower/pomegranate/bullet_"+a+"/bullet_2");
    }

    public void SetTarget(Transform newTarget, int NewDamage)
    {
        if (newTarget == null) return;

        Target = newTarget;
        Damage = NewDamage;
        StartPosition = transform.position;
        TargetPosition = newTarget.position;
        Timer = 0;
        IsFlying = true;
    }

    private void FixedUpdate()
    {
        if (!IsFlying || HasHit || Target == null)
        {
            if (Target == null) Destroy(gameObject);
            return;
        }

        Timer += Time.fixedDeltaTime;
        float t = Mathf.Clamp01(Timer / TravelTime);

        TargetPosition = Target.position;
        Vector3 currentPos = Vector3.Lerp(StartPosition, TargetPosition, t);
        float heightOffset = Mathf.Sin(Mathf.PI * t) * ArcHeight;
        currentPos.y += heightOffset;
        transform.position = currentPos;

        //// Rotate
        //if (t < 1f)
        //{
        //    Vector3 nextPos = Vector3.Lerp(StartPosition, TargetPosition, t + 0.01f);
        //    nextPos.y += Mathf.Sin(Mathf.PI * (t + 0.01f)) * ArcHeight;
        //    Vector2 direction = (nextPos - transform.position).normalized;
        //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //    transform.rotation = Quaternion.Euler(0, 0, angle);
        //}

        if (t >= 1f && !HasHit)
        {
            if (ExplosionEffect != null)
            {
               GameObject ParticleEx = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
               ParticleEx.transform.localScale = new Vector3(LeverManager.main.Scale*0.2f, LeverManager.main.Scale*0.2f, 1);
            }

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.2f);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    Enermy enemy = hit.GetComponent<Enermy>();
                    if (enemy != null && !enemy.GetIsDie())
                    {
                        enemy.SetDamage(Damage);
                    }
                    //break;
                }
            }

            HasHit = true;
            Destroy(gameObject);
        }
    }
}
