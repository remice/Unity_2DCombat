using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    [SerializeField]
    private float delayDestroyTime;
    [SerializeField]
    protected float maxAttackTimer;

    private float curHP;
    protected float curAttackTimer = 0;

    public GameObject target;
    public GameObject[] attackPrefab;
    public Transform[] attackSpawnPoints;
    public float MaxHP { get { return maxHP; } }
    public float CurHP { get { return curHP; } }

    private void Start()
    {
        curHP = maxHP;
    }

    private void Update()
    {
        Attack();
    }

    public void GetDamaged(float damage)
    {
        curHP -= damage;
        Debug.Log("데미지량 : " + damage + "   현재 HP : " + curHP);
        if (curHP <= 0)
        {
            Invoke("DestroySelf", delayDestroyTime);
            gameObject.SetActive(false);
        }
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    protected virtual void Attack()
    {
    }
}
