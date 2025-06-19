 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Burst.CompilerServices;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform TurretRotatePoint;
    [SerializeField] private float TargetRange = 2.5f;
    [SerializeField] private LayerMask EnermyMask;
    [SerializeField] private float RotateSpeed = 200f;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform ShootPoint;
    [SerializeField] float BulletPerSecond = 1f;
    int  Cost = 100;
    private Transform target;
    private float TimeToShoot;

    // Update is called once per frame
    void Update()
    {
        if(!target)
        {
            FindTarget();
            return;
        }
        else
        {
            TimeToShoot += Time.deltaTime;
            if (TimeToShoot >= 1 / BulletPerSecond)
            {
                Shoot();
                TimeToShoot = 0f;
            }
        }
        RotateTowardTarget();
        if(!CheckTargetInRange() || target.GetComponent<Enermy>().GetIsDie()) target = null;
    }

    private void Shoot()
    {
        GameObject BulletShoot = Instantiate(BulletPrefab, ShootPoint.position, Quaternion.identity);
        BrocolliBullet BulletScript = BulletShoot.GetComponent<BrocolliBullet>();
        BulletScript.SetTarget(target, 1);
    }

    private void FindTarget()
    {
        // Find
        RaycastHit2D[] hitList = Physics2D.CircleCastAll(transform.position, TargetRange, Vector2.zero, 0f, EnermyMask);

        float minDistance = Mathf.Infinity;
        Transform nearestTarget = null;

        for (int i = 0; i < hitList.Length; i++)
        {
            Enermy enermy = hitList[i].transform.GetComponent<Enermy>();
            if (enermy != null && enermy.GetIsDie() == false)
            {
                float distance = Vector2.Distance(transform.position, hitList[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestTarget = hitList[i].transform;
                }
            }
        }

        // Set target
        target = nearestTarget;
    }

    private bool CheckTargetInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= TargetRange;
    }

    private void RotateTowardTarget()
    {
        float Angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x)*Mathf.Rad2Deg -90;
        Quaternion TargetRotation = Quaternion.Euler(new Vector3(0f, 0f, Angle));
        TurretRotatePoint.rotation = Quaternion.RotateTowards(TurretRotatePoint.rotation, TargetRotation, RotateSpeed*Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, TargetRange);
#endif
    }

    public float GetTargetRange()
    {
        return TargetRange;
    }

    public float GetCost()
    {
        return Cost;
    }
}
