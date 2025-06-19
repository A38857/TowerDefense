using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawberryTopCollide : MonoBehaviour
{
    public int Damage;

    public void SetDamage(int NewDamage)
    {
        Damage = NewDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enermy enemy = collision.gameObject.GetComponent<Enermy>();
            if (enemy != null && !enemy.GetIsDie() && enemy.GetType() == 1)
            {
                enemy.SetDamage(Damage);
            }
        }
    }
}
