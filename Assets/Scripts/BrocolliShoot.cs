using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BrocolliShoot : MonoBehaviour, IAttackBehavior
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float StartCooldown = 1f;
    [SerializeField] private int StartDamage = 16;
    [SerializeField] private TextMeshProUGUI TextLevel;
    [SerializeField] private TextMeshProUGUI TextDamage;
    [SerializeField] private TextMeshProUGUI TextCoolDown;
    public Transform PivotTransform;
    private Coroutine bodyShakeCoroutine;
    public Transform LeftHandTransform;
    private Coroutine attackAnimCoroutine;
    private float attackCooldown;
    private int Damage;

    private float attackTimer;

    void Start()
    {
        // Data
        Damage = StartDamage;
        attackCooldown = StartCooldown;

        // Gui
        TextLevel.SetText("Level: " + 1);
        TextDamage.SetText("Damage: " + Damage);
        TextCoolDown.SetText("CoolDown: " + attackCooldown);
    }

    public void Attack(Transform target)
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            Shoot(target);
            attackTimer = 0f;
            PlayAttackAnim();

            // Sound
            SoundManager.Instance.PlayBrocolliShoot();
        }
    }

    public void Upgrade(int Level)
    {
        if (attackCooldown > 0.15f) attackCooldown = StartCooldown - Level*0.1f;
        Damage = StartDamage + Level*5;

        // Gui
        TextLevel.SetText("Level: " + ((Level<3)?Level:"Max Level"));
        TextDamage.SetText("Damage: " + Damage);
        TextCoolDown.SetText("CoolDown: " + attackCooldown);
    }

    private void Shoot(Transform target)
    {
        GameObject bulletPre = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        BrocolliBullet bullet = bulletPre.GetComponent<BrocolliBullet>();
        bullet.Tower = gameObject;
        bullet.SetTarget(target, Damage);
    }

    public void PlayAttackAnim()
    {
        DoAttackAnim_LeftHand();
        //DoAttackShakeBody();
    }

    public void DoAttackAnim_LeftHand()
    {
        if (attackAnimCoroutine != null)
            StopCoroutine(attackAnimCoroutine);

        attackAnimCoroutine = StartCoroutine(LeftHandAttackRoutine());
    }

    IEnumerator LeftHandAttackRoutine()
    {
        float duration = 0.1f;
        float angle = 20f; 
        float elapsed = 0f;

        Quaternion originalRotation = LeftHandTransform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            LeftHandTransform.localRotation = Quaternion.Lerp(originalRotation, targetRotation, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            LeftHandTransform.localRotation = Quaternion.Lerp(targetRotation, originalRotation, t);
            yield return null;
        }

        LeftHandTransform.localRotation = originalRotation;
        attackAnimCoroutine = null;
    }

    public void DoAttackShakeBody()
    {
        if (bodyShakeCoroutine != null)
            StopCoroutine(bodyShakeCoroutine);

        bodyShakeCoroutine = StartCoroutine(ShakeBodyOnce());
    }

    IEnumerator ShakeBodyOnce()
    {
        Vector3 originalPos = PivotTransform.localPosition;
        Vector3 offset = new Vector3(0.05f, 0, 0); 

        PivotTransform.localPosition = originalPos + offset;
        yield return new WaitForSeconds(0.05f);

        PivotTransform.localPosition = originalPos;
        bodyShakeCoroutine = null;
    }
}