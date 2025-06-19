using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

public class StrawberryAttack : MonoBehaviour, IAttackBehavior
{
    public Transform centerPoint;
    public GameObject topBody;

    public float radius = 0.6f;
    [SerializeField]  private float StartrotateSpeed = 180f;
    public float selfRotateSpeed = 360f;
    public float returnSpeed = 5f;
    public float slideOutSpeed = 2f;
    [SerializeField] private int StartDamage = 16;
    [SerializeField] private TextMeshProUGUI TextLevel;
    [SerializeField] private TextMeshProUGUI TextDamage;
    [SerializeField] private TextMeshProUGUI TextRotateSpeed;

    private float attackCooldown;
    private int Damage;
    public float rotateSpeed;

    private float angle;
    private Vector3 originalWorldPos;
    private Quaternion originalRotation;

    public bool hasTarget = false;
    private bool isSlidingOut = false;
    private float orbitProgress = 0f;
    private Vector3 orbitDirection;

    private void Start()
    {
        originalWorldPos = topBody.transform.position;
        originalRotation = topBody.transform.rotation;
        rotateSpeed = StartrotateSpeed;
        Damage = StartDamage;
        topBody.GetComponent<StrawberryTopCollide>().SetDamage(Damage);

        // Gui
        TextLevel.SetText("Level: " + 1);
        TextDamage.SetText("Damage: " + Damage);
        TextRotateSpeed.SetText("Speed: " + (float)Math.Round( rotateSpeed*Time.deltaTime,2));
    }

    public void Attack(Transform target)
    {
        if (target == null)
        {
            hasTarget = false;
            isSlidingOut = false;
            orbitProgress = 0f;
            return;
        }

        if (!hasTarget)
        {
            orbitDirection = (topBody.transform.position - centerPoint.position).normalized;
            angle = Mathf.Atan2(orbitDirection.y, orbitDirection.x) * Mathf.Rad2Deg;

            orbitProgress = 0f;
            isSlidingOut = true;
        }

        hasTarget = true;

        if (isSlidingOut)
        {
            orbitProgress += Time.deltaTime * slideOutSpeed;
            orbitProgress = Mathf.Clamp01(orbitProgress);

            Vector3 offset = orbitDirection * (radius * orbitProgress);
            topBody.transform.position = centerPoint.position + offset;

            if (orbitProgress >= 1f)
            {
                isSlidingOut = false;

                // sound
                SoundManager.Instance.PlayStawAttack();
            }
        }
        else
        {
            angle += rotateSpeed * Time.deltaTime;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;

            topBody.transform.position = centerPoint.position + offset;
            topBody.transform.Rotate(Vector3.forward, selfRotateSpeed * Time.deltaTime);
        }
    }

    public void Upgrade(int Level)
    {
        rotateSpeed = StartrotateSpeed + Level * 10f;
        Damage = StartDamage + Level * 5;
        topBody.GetComponent<StrawberryTopCollide>().SetDamage(Damage);

        // Gui
        TextLevel.SetText("Level: " + ((Level < 3) ? Level : "Max Level"));
        TextDamage.SetText("Damage: " + Damage);
        TextRotateSpeed.SetText("Speed: " + (float)Math.Round(rotateSpeed * Time.deltaTime, 2));
    }

    private void Update()
    {
        if (!hasTarget && (topBody.transform.position != originalWorldPos || topBody.transform.rotation != originalRotation))
        {
            topBody.transform.position = Vector3.Lerp(topBody.transform.position, originalWorldPos, Time.deltaTime * returnSpeed);
            topBody.transform.rotation = Quaternion.Lerp(topBody.transform.rotation, originalRotation, Time.deltaTime * returnSpeed);
        }
    }
}
