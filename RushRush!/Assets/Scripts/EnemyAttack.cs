using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private string attackName;
    [SerializeField]
    private float destroyTime;
    [SerializeField]
    private float maxDelay;
    [SerializeField]
    private float speed;

    private float curDelay;
    private bool onTrigger = false;
    private float tempTime;

    public float Damage { get { return damage; } }

    public bool isAttack = false;
    public GameObject target;
    public Transform spawnPoint;

    Collider2D m_collider;

    private void Awake()
    {
        m_collider = gameObject.GetComponent<Collider2D>();
        Invoke("DestroyThis", destroyTime);
        switch (attackName)
        {
            case "MineAttack":
                tempTime = Time.time + 1.0f;
                transform.Rotate(Vector3.forward * Random.Range(0, 360));
                ChangeColor(1, 1, 1, 0.7f);
                break;
        }
    }

    private void Update()
    {
        curDelay += Time.deltaTime;
        switch (attackName)
        {
            case "BiteAttack":
                BiteAttack();
                break;
            case "FireballAttack":
                FireballAttack();
                break;
            case "IcicleAttack":
                IcicleAttack();
                break;
            case "LastIcicleAttack":
                LastIcicleAttack();
                break;
            case "MineAttack":
                MineAttack();
                break;
        }
    }

    private void DestroyThis()
    {
        Destroy(this.gameObject);
    }

    private float AngleToTarget()
    {
        Vector2 direction = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
        return (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }

    private void ChangeColor(float r, float g, float b, float a)
    {
        Color new_Color = new Color(r, g, b, a);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new_Color;
    }

    private void BiteAttack()
    {
        if(curDelay < maxDelay) transform.position += new Vector3(-0.6f, 0, 0) * Time.deltaTime;
    }
    
    private void FireballAttack()
    {
        if (curDelay > maxDelay)
        {
            m_collider.enabled = true;
            float angle = AngleToTarget();
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.position = transform.position + transform.right * speed * Time.deltaTime;
        } else
        {
            m_collider.enabled = false;
        }
        if (isAttack) DestroyThis();
    }

    private void IcicleAttack()
    {
        float angle;
        if(!onTrigger)
        {
            m_collider.enabled = false;
            transform.Rotate(Vector3.forward * 1080 * Time.deltaTime);
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, spawnPoint.position.x, 0.03f), Mathf.Lerp(transform.position.y, spawnPoint.position.y, 0.1f), 0);
            if (Vector2.Distance(transform.position, spawnPoint.position) <= 0.2f) onTrigger = true;
        } else if (curDelay < maxDelay)
        {
            angle = AngleToTarget();
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        } else
        {
            m_collider.enabled = true;
            transform.position = transform.position + transform.right * speed * Time.deltaTime;
        }
        if (isAttack) DestroyThis();
    }

    private void LastIcicleAttack()
    {
        float angle;
        if (!onTrigger)
        {
            onTrigger = true;
            angle = AngleToTarget();
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else if (curDelay > maxDelay)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        if (isAttack) DestroyThis();
    }

    private void MineAttack()
    {
        if (!onTrigger)
        {
            m_collider.enabled = false;
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, spawnPoint.position.x, 0.04f), Mathf.Lerp(transform.position.y, spawnPoint.position.y, 0.04f), 0);
            if (Vector2.Distance(transform.position, spawnPoint.position) <= 0.3f)
            {
                tempTime = Time.time;
                onTrigger = true;
            }
        } else if (Time.time - tempTime > maxDelay)
        {
            m_collider.enabled = true;
            ChangeColor(1, 1, 1, 1);
        }
        if (isAttack) DestroyThis();
    }
}
