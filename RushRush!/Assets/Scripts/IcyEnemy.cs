using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyEnemy : Enemy
{
    private bool onIcicle = false;

    private bool isAttack1 = false; // use LastAttack ↓↓
    private bool isAttack2 = false;
    private bool isAttack3 = false;
    private bool isAttack4 = false;
    private bool isAttack5 = false; // use LastAttack ↑↑

    private float attackTimer = 0;

    int i = 0;


    protected override void Attack()
    {
        curAttackTimer += Time.deltaTime;
        if(curAttackTimer > 3 && !onIcicle)
        {
            onIcicle = true;
            IcicleAttack();
        }
        if(curAttackTimer > maxAttackTimer)
        {
            onIcicle = false;
            curAttackTimer = 0;
        }
        if(CurHP <= MaxHP * 0.3)
        {
            LastAttack();
        }
    }

    private void IcicleAttack()
    {
        i = 0;
        InvokeRepeating("ex_IcicleAttack", 0, 0.2f);
    }

    private void ex_IcicleAttack()
    {
        GameObject clone = Instantiate(attackPrefab[0]);
        clone.transform.position = transform.position + Vector3.forward;
        EnemyAttack attack = clone.GetComponent<EnemyAttack>();
        attack.target = this.target;
        attack.spawnPoint = attackSpawnPoints[i];

        if (++i >= 6) CancelInvoke("ex_IcicleAttack");
    }
    
    private void LastAttack()
    {
        attackTimer += Time.deltaTime;
        if (!isAttack1)
        {
            isAttack1 = true;
            ex_RightLeftAttack();
        } else if (!isAttack2 && attackTimer > 1.5f)
        {
            isAttack2 = true;
            ex_UpDownAttack();
        } else if (!isAttack3 && attackTimer > 3.0f)
        {
            isAttack3 = true;
            ex_RightLeftAttack();
            ex_UpDownAttack();
        } else if (!isAttack4 && attackTimer > 4.5f)
        {
            isAttack4 = true;
            ex_SlashAttack();
        } else if(!isAttack5 && attackTimer > 6.0f)
        {
            isAttack5 = true;
            ex_RightLeftAttack();
            ex_UpDownAttack();
            ex_SlashAttack();
        }
    }

    private void ex_RightLeftAttack()
    {
        SpawnIcicle(attackPrefab[1], target.transform.position + transform.right * 3.5f);
        SpawnIcicle(attackPrefab[1], target.transform.position + transform.right * -3.5f);
    }

    private void ex_UpDownAttack()
    {
        SpawnIcicle(attackPrefab[1], target.transform.position + transform.up * 3.5f);
        SpawnIcicle(attackPrefab[1], target.transform.position + transform.up * -3.5f);
    }

    private void ex_SlashAttack()
    {
        SpawnIcicle(attackPrefab[1], target.transform.position + transform.right * 2.5f + transform.up * 2.5f);
        SpawnIcicle(attackPrefab[1], target.transform.position + transform.right * -2.5f + transform.up * 2.5f);
        SpawnIcicle(attackPrefab[1], target.transform.position + transform.right * 2.5f + transform.up * -2.5f);
        SpawnIcicle(attackPrefab[1], target.transform.position + transform.right * -2.5f + transform.up * -2.5f);
    }

    private void SpawnIcicle(GameObject prefab, Vector3 pos)
    {
        GameObject clone = Instantiate(prefab);
        clone.transform.position = pos;
        EnemyAttack attack = clone.GetComponent<EnemyAttack>();
        attack.target = this.target;
    }
}
