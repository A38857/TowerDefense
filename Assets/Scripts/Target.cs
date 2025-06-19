using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D Rb;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnermySpawner.OnEnermyDestroy.Invoke();
            Destroy(other.gameObject);
            LeverManager.main.ChangeHealth();
        }
    }
}
