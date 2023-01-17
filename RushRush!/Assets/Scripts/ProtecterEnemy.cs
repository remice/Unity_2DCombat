using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtecterEnemy : Enemy
{
    [SerializeField]
    private GameObject lines;
    private LineRenderer line;
    [SerializeField]
    private GameObject o_anim;
    private Animator anim;
    [SerializeField]
    private GameObject[] lastLines;
    private LineRenderer lastLine;
    [SerializeField]
    private GameObject lastAttackPot;

    private bool isAttack1 = false;
    private bool isAttack2 = false;
    private bool isAttack3 = false;
    private bool isLastAttack = false;
    private bool onLastAttack = false;
    private bool onRasor = false;

    private int spawnPointIndex = 0;
    private float x;
    private float y;
    private float angle;
    private float rotSpeed;

    private void Awake()
    {
        line = lines.GetComponent<LineRenderer>();
        anim = o_anim.GetComponent<Animator>();
    }

    protected override void Attack()
    {
        curAttackTimer += Time.deltaTime;
        if (CurHP / MaxHP <= 0.3f && !isLastAttack)
        {
            ex_ReadyLastAttack();
        }
        if (!isLastAttack)
        {
            if (curAttackTimer > 1.5f && !isAttack1)
            {
                isAttack1 = true;
                ex_MineAttack();
            }
            if (curAttackTimer > 3.0f && !isAttack2)
            {
                isAttack2 = true;
                ex_MineAttack();
                Invoke("OnRasorEffect", 1);
                Invoke("OnRasor", 1.5f);
                Invoke("OffRasor", 3.5f);
            }
            if (curAttackTimer > 3.0f && curAttackTimer < 4.0f)
            {
                ex_ReadyRasorAttack();
            }
            if (curAttackTimer > 4.5f && !isAttack3)
            {
                isAttack3 = true;
                ex_MineAttack();
            }
            if (curAttackTimer > maxAttackTimer)
            {
                ex_MineAttack();
                curAttackTimer = 0;
                isAttack1 = false;
                isAttack2 = false;
                isAttack3 = false;
            }
        }
        else
        {
            if (curAttackTimer > maxAttackTimer)
            {
                ex_MineAttack();
                ex_MineAttack();
                curAttackTimer = 0;
            }
        }
    }

    private void LateUpdate()
    {
        if (onRasor)
        {
            ex_ActiveRasorAttack();
        }
        if (onLastAttack)
        {
            ex_LastAttack();
        }
    }

    private void ex_MineAttack()
    {
        GameObject clone = Instantiate(attackPrefab[0]);
        clone.transform.position = transform.position;
        EnemyAttack enemy = clone.GetComponent<EnemyAttack>();
        attackSpawnPoints[spawnPointIndex].position = new Vector3(Random.Range(-9.0f, 9.0f), Random.Range(-4.5f, 4.5f), -5);
        enemy.spawnPoint = attackSpawnPoints[spawnPointIndex];
        if (spawnPointIndex == 0) spawnPointIndex = 1;
        else spawnPointIndex = 0;
    }

    private void OnRasorEffect()
    {
        anim.SetTrigger("RasorOn");
    }


    private void OnRasor()
    {
        onRasor = true;
        line.startColor = new Color(1, 100f / 255f, 30f / 255f, 1);
        line.endColor = new Color(1, 80f / 255f, 25f / 255f, 1);
        x = target.transform.position.x - lines.transform.position.x;
        y = target.transform.position.y - lines.transform.position.y;
        float dAngle = angle - Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        if ((dAngle >= 0 && dAngle <= 180) || (dAngle >= -360 && dAngle <= -180))
        {
            rotSpeed = -60;
        } else rotSpeed = 60;
    }

    private void OffRasor()
    {
        anim.SetTrigger("RasorOff");
        line.SetPosition(1, Vector3.zero);
        onRasor = false;
    }

    private void ex_ReadyRasorAttack()
    {
        line.startColor = new Color(1, 50f / 255f, 15f / 255f, 0.7f);
        line.endColor = new Color(1, 45f / 255f, 14f / 255f, 0.7f);
        line.SetPosition(1, new Vector3(13, 0, 0));
        x = target.transform.position.x - lines.transform.position.x;
        y = target.transform.position.y - lines.transform.position.y;
        
        angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        line.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void ex_ActiveRasorAttack()
    {
        int layerMask = (1 << LayerMask.NameToLayer("Shield")) + (1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D hit = Physics2D.Raycast(lines.transform.position, RadianToVector2(angle * Mathf.Deg2Rad).normalized, 13, layerMask);
        if (hit)
        {
            float dis = Vector2.Distance(hit.transform.position, lines.transform.position);
            if(Vector2.Distance(target.transform.position, lines.transform.position) - dis < 0.3f)
            {
                line.SetPosition(1, new Vector3(dis - 0.6f, 0, 0));
                MainCharacter mainCharacter = target.GetComponent<MainCharacter>();
                mainCharacter.GetDamaged(1.5f);
            } else
            {
                line.SetPosition(1, new Vector3(dis - 0.1f, 0, 0));
            }
        } else
        {
            line.SetPosition(1, new Vector3(13, 0, 0));
        }
        angle += rotSpeed * Time.deltaTime;
        line.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void ex_ReadyLastAttack()
    {
        isLastAttack = true;
        curAttackTimer = 0;
        maxAttackTimer = 1.5f;
        rotSpeed = 40;
        angle = 0;
        CancelInvoke("OnRasorEffect");
        CancelInvoke("OnRasor");
        CancelInvoke("OffRasor");
        OffRasor();
        OnRasorEffect();
        for (int i = 0; i < lastLines.Length; i++)
        {
            lastLine = lastLines[i].GetComponent<LineRenderer>();
            lastLine.SetPosition(0, new Vector3(1.75f, 0, 1));
            lastLine.SetPosition(1, new Vector3(2, 0, 0));
            lastLine.SetPosition(2, new Vector3(13, 0, 0));
            lastLine.startColor = new Color(1, 28f / 255f, 30f / 255f, 0.7f);
            lastLine.endColor = new Color(250f / 255f, 198f / 255f, 119f / 255f, 0.7f);
        }
        Invoke("ex_StartLastAttack", 1.5f);
    }

    private void ex_StartLastAttack()
    {
        onLastAttack = true;
        for (int i = 0; i < lastLines.Length; i++)
        {
            lastLine = lastLines[i].GetComponent<LineRenderer>();
            lastLine.startColor = new Color(1, 28f / 255f, 30f / 255f, 1);
            lastLine.endColor = new Color(250f / 255f, 198f / 255f, 119f / 255f, 1);
        }
    }

    private void ex_LastAttack()
    {
        angle += rotSpeed * Time.deltaTime;
        lastAttackPot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        int layerMask = (1 << LayerMask.NameToLayer("Shield")) + (1 << LayerMask.NameToLayer("Player"));
        for (int i = 0; i < 4; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(lines.transform.position, RadianToVector2((angle + i * 90) * Mathf.Deg2Rad).normalized, 13, layerMask);
            if (hit)
            {
                float dis = Vector2.Distance(hit.transform.position, lines.transform.position);
                if (Vector2.Distance(target.transform.position, lines.transform.position) - dis < 0.3f)
                {
                    lastLine = lastLines[i].GetComponent<LineRenderer>();
                    lastLine.SetPosition(2, new Vector3(dis - 0.6f, 0, 0));
                    MainCharacter mainCharacter = target.GetComponent<MainCharacter>();
                    mainCharacter.GetDamaged(1f);
                }
                else
                {
                    lastLine = lastLines[i].GetComponent<LineRenderer>();
                    lastLine.SetPosition(2, new Vector3(dis - 0.1f, 0, 0));
                }
            }
            else
            {
                lastLine = lastLines[i].GetComponent<LineRenderer>();
                lastLine.SetPosition(2, new Vector3(13, 0, 0));
            }
        }
    }

    private Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
}
