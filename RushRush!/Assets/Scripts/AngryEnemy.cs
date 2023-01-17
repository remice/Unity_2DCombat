using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryEnemy : Enemy
{
    private bool onBite = false;
    private bool onFire = false;

    int i = 0;

    protected override void Attack()
    {
        curAttackTimer += Time.deltaTime;
        if (curAttackTimer > 3 && !onBite)
        {
            onBite = true;
            BitePattern();
        }
        if (curAttackTimer > 6 && !onFire)
        {
            onFire = true;
            FireballPattern();
        }
        if (curAttackTimer > maxAttackTimer)
        {
            onBite = false;
            onFire = false;
            curAttackTimer = 0;
        }
    }

    private void BitePattern()
    {
        GameObject clone = Instantiate(attackPrefab[0]);
        clone.transform.position = target.transform.position + new Vector3(0.8f, 0, -5);
    }

    private void FireballPattern()
    {
        i = 0;
        InvokeRepeating("ex_FireballPattern", 0, 0.3f);
    }

    private void ex_FireballPattern()
    {
        GameObject clone = Instantiate(attackPrefab[1]);
        clone.transform.position = attackSpawnPoints[i].position;
        EnemyAttack attack = clone.GetComponent<EnemyAttack>();
        attack.target = this.target;
        if (++i >= 3) CancelInvoke("ex_FireballPattern");
    }
}