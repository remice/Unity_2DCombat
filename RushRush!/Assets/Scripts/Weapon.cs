using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float damage;

    private float damageRatio = 1.0f;
    public float DamageRatio { set { damageRatio = value; } }

    private bool isAttacked = false;

    private void OnEnable()
    {
        isAttacked = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if (isAttacked) return;
            isAttacked = true;
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.GetDamaged(damage * damageRatio);
        }
    }
}
