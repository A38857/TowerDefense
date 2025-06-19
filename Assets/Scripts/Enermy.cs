using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

//const int ENERMY_GROUND = 1;
//const int ENERMY_FLY = 2;

public class Enermy : MonoBehaviour
{
    // Data
    [SerializeField] private Rigidbody2D RigidBody;
    [SerializeField] private int Type;
    [SerializeField] private int MaxHitPoint;
    [SerializeField] private int CoinWorth;
    [SerializeField] private float Speed;
    [SerializeField] private int DirectX;
    [SerializeField] FloatingHealthBar healthBar;
    private int HitPoint;
    private int PathIndex = 0;
    private float SpeedNormal;
    private float TimeSlow;
    private bool IsDie = false;
    private Vector2 Direction;
    private Color originalColor;
    private Animator Anim;
    private Transform Target;
    private SpriteRenderer Sprite;
    private List<Transform> path;
    private int pathIndex = 0;

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        Sprite = GetComponent<SpriteRenderer>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    void Start()
    {
        SpeedNormal = Speed;
        TimeSlow = 0;
        originalColor = Sprite.color;
        HitPoint = MaxHitPoint;
        healthBar.UpdateHealthBar(HitPoint, MaxHitPoint);
    }

    void Update()
    {
        // Check
        if (IsDie) return;

        if (path == null || Target == null) return;

        if (Vector2.Distance(transform.position, Target.position) <= 0.1f)
        {
            pathIndex++;
            if (pathIndex >= path.Count)
            {
                EnermySpawner.OnEnermyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            Target = path[pathIndex];
        }
    }

    //// Target
    //if (Vector2.Distance(Target.position, transform.position) <= 0.1f)
    //{
    //    PathIndex++;
    //    if (PathIndex > LeverManager.main.PathEnermy.Count)
    //    {
    //        EnermySpawner.OnEnermyDestroy.Invoke();
    //        Destroy(gameObject);
    //        return;
    //    }
    //    else if(PathIndex < LeverManager.main.PathEnermy.Count)
    //    {
    //        Target = LeverManager.main.PathEnermy[PathIndex];
    //    }
    //}
//}

    public void SetPath(List<Transform> path)
    {
        this.path = path;
        pathIndex = 0;
        transform.position = path[0].position;

        if (path.Count > 1) Target = path[1];
    }

    private void FixedUpdate()
    {
        // Check
        if (IsDie)
        {
            RigidBody.velocity = Vector2.zero;
            return;
        }

        // Move
        if(TimeSlow > 0)
        {
            TimeSlow -= Time.deltaTime;
            if (TimeSlow <= 0)
            {
                TimeSlow = 0;
                Speed = SpeedNormal;
                Sprite.color = originalColor;
            }
        }
        Direction = (Target.position - transform.position).normalized;
        RigidBody.velocity = Direction * Speed;

        // Animation
        if (Mathf.Abs(Direction.x) > Mathf.Abs(Direction.y))
        {
            // Animation
            Anim.Play("move_0");

            // Flip X
            Vector3 scale = transform.localScale;
            if (Direction.x < 0) scale.x = -DirectX*Mathf.Abs(scale.x);
            else scale.x = DirectX*Mathf.Abs(scale.x);
            transform.localScale = scale;

            if (healthBar != null)
            {
                Vector3 hbScale = healthBar.transform.localScale;
                hbScale.x = Mathf.Abs(transform.localScale.x) / transform.localScale.x * Mathf.Abs(hbScale.x);
                healthBar.transform.localScale = hbScale;
            }
        }
        else
        {
            if (Direction.y > 0) Anim.Play("move_270");
            else Anim.Play("move_90");
        }
    }

    public void SetDamage(int Damage)
    {
        // Data
        HitPoint -= Damage;
        healthBar.UpdateHealthBar(HitPoint, MaxHitPoint);

        // Dame
        if (HitPoint <= 0 && !IsDie)
        {
            // Data
            IsDie = true;

            // Animation
            if (Mathf.Abs(Direction.x) > Mathf.Abs(Direction.y))
            {
                // Animation
                Anim.Play("die_0");

                // Flip X
                Vector3 scale = transform.localScale;
                if (Direction.x < 0) scale.x = -DirectX * Mathf.Abs(scale.x);
                else scale.x = DirectX * Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
            else
            {
                if (Direction.y > 0) Anim.Play("die_270");
                else Anim.Play("die_90");
            }

            //Effect
            EffectManager.Instance.PlayDieEffect(this.gameObject, 1.5f);
            Sprite.sortingOrder = -1;
            EffectManager.Instance.PlayCoinJumpEffect(transform.position, transform.localScale.x, 2);
            LeverManager.main.IncreseCoin(CoinWorth);
            SoundManager.Instance.PlayCoinDrop();

            // Score
            LeverManager.main.AddScore();

            // healthbar
            if (healthBar != null) Destroy(healthBar.gameObject);
        }
    }

    public int GetType()
    {
        return Type;
    }

    public void SetSpeed(float SpeedRate, float Time)
    {
        Speed = SpeedNormal * SpeedRate;
        TimeSlow = Time;
        Sprite.color = new Color(0.6f, 0.85f, 1f);
    }

    public bool GetIsDie()
    {
        return IsDie;
    }  
}
