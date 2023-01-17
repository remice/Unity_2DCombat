using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxHP;

    public Animator animator;
    public LineRenderer lineRenderer;
    public Transform linePos;
    public GameObject enemy;
    public GameObject gameManager;
    public GameObject sword;
    public GameObject shield;

    public float rasorDamage;

    private float x;
    private float y;
    private bool isDesh = false;
    private bool onAttack = false;
    private bool onRasorAttack = false;
    private bool isCharge = false;
    private float stockSpeed;
    private float curHP;
    private float curChargeTime = 0;
    private float maxChargeTime = 0.5f;

    private float attackDamageRatio = 1.0f;
    private float attackSpeedRatio = 1.0f;
    private Vector3 shieldSizeRatio = new Vector3(1.0f, 1.0f, 1);

    private Spectrum spectrum;
    private GameManager s_gameManager;
    private Weapon s_sword;

    private void Awake()
    {
        stockSpeed = speed;
        spectrum = GetComponent<Spectrum>();
        curHP = maxHP;
        ResetRasor();
        s_gameManager = gameManager.GetComponent<GameManager>();
        s_sword = sword.GetComponent<Weapon>();
    }

    private void Update()
    {
        Move();
        Attack();
    }

    private void LateUpdate()
    {
        if (onRasorAttack) DrawRasorRay();
    }

    private void Move()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        DrawingRay();
        Desh();

        transform.position = new Vector3(transform.position.x + x * speed * Time.deltaTime, transform.position.y + y * speed * Time.deltaTime, 0);
    }

    private void Desh()
    {
        if (speed <= stockSpeed + 0.5f)
        {
            speed = stockSpeed;
            isDesh = false;
        }
        else
        {
            speed = Mathf.Lerp(speed, stockSpeed, 0.05f);
        }
        if (isDesh) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            speed *= 3;
            spectrum.OnSpectrum();
            isDesh = true;
        }
    }

    private void DrawingRay()
    {
        int layerMask = (1 << LayerMask.NameToLayer("Border")) + (1 << LayerMask.NameToLayer("Enemy"));

        for (int i = -1; i < 2; i++) // right
        {
            float distance = i * 0.55f;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, distance, 0), Vector2.right, 0.7f, layerMask);
            if (hit && x > 0)
            {
                x = 0;
            }
        }
        for (int i = -1; i < 2; i++) // left
        {
            float distance = i * 0.55f;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, distance, 0), Vector2.left, 0.7f, layerMask);
            if (hit && x < 0)
            {
                x = 0;
            }
        }
        for (int i = -1; i < 2; i++) // up
        {
            float distance = i * 0.55f;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(distance, 0, 0), Vector2.up, 0.7f, layerMask);
            if (hit && y > 0)
            {
                y = 0;
            }
        }
        for (int i = -1; i < 2; i++) // down
        {
            float distance = i * 0.55f;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(distance, 0, 0), Vector2.down, 0.7f, layerMask);
            if (hit && y < 0)
            {
                y = 0;
            }
        }

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, Vector2.right, 0.1f, layerMask); // bugfix
        if (hit1)
        {
            transform.position = new Vector3(transform.position.x + -10 * Time.deltaTime, transform.position.y + 10 * Time.deltaTime, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack" || collision.gameObject.tag == "EnemyRangeAttack")
        {
            EnemyAttack enemyAttack = collision.gameObject.GetComponent<EnemyAttack>();
            if (enemyAttack.isAttack) return;
            GetDamaged(enemyAttack.Damage);
            enemyAttack.isAttack = true;
        }
    }

    private void Attack()
    {
        LeftButton();
        RightButton();
        if (isCharge) curChargeTime += (Time.deltaTime * attackSpeedRatio);
    }

    private void LeftButton()
    {
        if (Input.GetMouseButton(0))
        {
            animator.SetTrigger("Attack");
            onAttack = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            animator.ResetTrigger("Attack");
            onAttack = false;
        }
    }

    private void RightButton()
    {
        if (Input.GetMouseButtonDown(1) && !onAttack && !isCharge)
        {
            animator.ResetTrigger("RightAttackUp");
            animator.SetTrigger("RightAttackDown");
            isCharge = true;
        }
        if (Input.GetMouseButtonUp(1) && !onAttack)
        {
            if(curChargeTime > maxChargeTime) onRasorAttack = true; // on DrawRasorRay() Funtion
            onAttack = true;
            curChargeTime = -0.1f;
            Invoke("ResetRasor", 0.6f);
            animator.ResetTrigger("RightAttackDown");
            animator.SetTrigger("RightAttackUp");
        }
    }

    private void DrawRasorRay()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
        float xPos = (lookPos.x - linePos.position.x) * (lookPos.x - linePos.position.x);
        float yPos = (lookPos.y - linePos.position.y) * (lookPos.y - linePos.position.y);

        lineRenderer.SetPosition(1, new Vector3(0.5f, 0, 0));
        lineRenderer.SetPosition(2, new Vector3(Mathf.Sqrt(xPos + yPos), 0, 0));

        int layerMask = 1 << LayerMask.NameToLayer("Enemy");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (lookPos - transform.position).normalized, Mathf.Sqrt(xPos + yPos) + 2, layerMask);

        if (hit)
        {
            Enemy s_enemy = enemy.GetComponent<Enemy>();
            s_enemy.GetDamaged(rasorDamage * attackDamageRatio);
        }
    }

    private void ResetRasor()
    {
        onAttack = false;
        onRasorAttack = false;
        isCharge = false;
        curChargeTime = 0;
        lineRenderer.SetPosition(1, Vector3.zero);
        lineRenderer.SetPosition(2, Vector3.zero);
    }

    public void GetDamaged(float damage)
    {
        curHP -= damage;
        if (curHP > maxHP) curHP = maxHP;
        s_gameManager.SetPlayerHealthBar(curHP / maxHP);
        if(curHP <= 0)
        {
            Debug.Log("사망");
        }
    }

    public void SetHealthPoint(float percent)
    {
        curHP = maxHP * percent / 100;
        if (curHP > maxHP) curHP = maxHP;
        s_gameManager.SetPlayerHealthBar(curHP / maxHP);
        if (curHP <= 0)
        {
            Debug.Log("사망");
        }
    }

    public void GetPercentDamage(float percent)
    {
        curHP -= maxHP * percent / 100;
        if (curHP > maxHP) curHP = maxHP;
        s_gameManager.SetPlayerHealthBar(curHP / maxHP);
        if (curHP <= 0)
        {
            Debug.Log("사망");
        }
    }

    public void AdjustAttackDamageRatio(float percent)
    {
        attackDamageRatio += (percent / 100);
        s_sword.DamageRatio = attackDamageRatio;
    }

    public void AdjustAttackSpeedRatio(float percent)
    {
        attackSpeedRatio += (percent / 100);
        animator.SetFloat("SpeedRatio", attackSpeedRatio);
    }

    public void AdjustShieldSizeRatio(float percent)
    {
        shieldSizeRatio.x += (shieldSizeRatio.x * percent / 100);
        shieldSizeRatio.y += (shieldSizeRatio.y * percent / 100);

        shield.transform.localScale = shieldSizeRatio;
    }
}
