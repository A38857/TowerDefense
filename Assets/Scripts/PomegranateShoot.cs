using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PomegranateShoot : MonoBehaviour, IAttackBehavior
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float StartCooldown = 1f;
    [SerializeField] private int StartDamage = 16;
    [SerializeField] private TextMeshProUGUI TextLevel;
    [SerializeField] private TextMeshProUGUI TextDamage;
    [SerializeField] private TextMeshProUGUI TextCoolDown;
    public Transform BodyTransform;
    private Coroutine scaleAnimCoroutine;
    public Transform EyesTransform; 
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
            DoAttackScaleAnim();

            // Sound
            SoundManager.Instance.PlayPomeShoot();
        }
    }

    public void Upgrade(int Level)
    {
        if (attackCooldown > 0.15f) attackCooldown = StartCooldown - Level * 0.05f;
        Damage = StartDamage + Level * 2;

        // Gui
        TextLevel.SetText("Level: " + ((Level < 3) ? Level : "Max Level"));
        TextDamage.SetText("Damage: " + Damage);
        TextCoolDown.SetText("CoolDown: " + attackCooldown);
    }

    private void Shoot(Transform target)
    {
        GameObject bulletPre = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        PomegranateBullet bullet = bulletPre.GetComponent<PomegranateBullet>();
        bullet.Tower = gameObject;
        bullet.SetTarget(target, Damage);
    }

    public void DoAttackScaleAnim()
    {
        if (scaleAnimCoroutine != null)
            StopCoroutine(scaleAnimCoroutine);

        scaleAnimCoroutine = StartCoroutine(ScaleSquashStretch());
    }

    IEnumerator ScaleSquashStretch()
    {
        Vector3 originalScaleBody = BodyTransform.localScale;
        Vector3 originalScaleEyes = EyesTransform.localScale;

        Vector3 stretchedScaleBody = new Vector3(originalScaleBody.x * 1.2f, originalScaleBody.y * 0.8f, originalScaleBody.z);
        Vector3 stretchedScaleEyes = new Vector3(originalScaleEyes.x * 1.2f, originalScaleEyes.y * 0.8f, originalScaleEyes.z);

        float duration = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            BodyTransform.localScale = Vector3.Lerp(originalScaleBody, stretchedScaleBody, t);
            EyesTransform.localScale = Vector3.Lerp(originalScaleEyes, stretchedScaleEyes, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            BodyTransform.localScale = Vector3.Lerp(stretchedScaleBody, originalScaleBody, t);
            EyesTransform.localScale = Vector3.Lerp(stretchedScaleEyes, originalScaleEyes, t);
            yield return null;
        }

        BodyTransform.localScale = originalScaleBody;
        EyesTransform.localScale = originalScaleEyes;
        scaleAnimCoroutine = null;
    }
}