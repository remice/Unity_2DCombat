using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int bossIndex;
    [SerializeField]
    private int maxBossIndex;
    [SerializeField]
    private float maxBossSpawnDelay;

    public GameObject mainCharacter;
    public GameObject[] bossPrefab;
    public Slider healthSlider;
    public Image playerHealthBar;
    public GameObject cardSet;

    private MainCharacter s_mainCharacter;
    private CardChanger s_cardChanger;
    private GameObject healthBarTarget;
    private float curBossSpawnDelay = 0;
    private bool isSelect = false;
    public bool IsSelect { set { isSelect = value; } }
    private bool onCard = false;

    private int[] cardArr = { 0, 1, 2, 3 };

    private void Awake()
    {
        s_mainCharacter = mainCharacter.GetComponent<MainCharacter>();
        s_cardChanger = cardSet.GetComponent<CardChanger>();
        playerHealthBar.fillAmount = 1;
        SpawnBoss();
    }

    private void Update()
    {
        if (healthBarTarget == null && curBossSpawnDelay > maxBossSpawnDelay)
        {
            healthSlider.value = 0;
            if (bossIndex < maxBossIndex)
            {
                bossIndex++;
                curBossSpawnDelay = 0;
                isSelect = false;
                onCard = false;
                SpawnBoss();
            }
            return;
        }
        if (isSelect) curBossSpawnDelay += Time.deltaTime;

        if(healthBarTarget != null)
        {
            Enemy enemy = healthBarTarget.GetComponent<Enemy>();
            healthSlider.value = enemy.CurHP;
        }
        else
        {
            if (onCard) return;
            EnableCardSystem();
        }
    }

    private void SpawnBoss()
    {
        GameObject clone = Instantiate(bossPrefab[bossIndex]);
        clone.transform.position = new Vector3(0, 0, -3);
        Enemy enemy = clone.gameObject.GetComponent<Enemy>();
        enemy.target = mainCharacter;
        healthBarTarget = clone;
        healthSlider.maxValue = enemy.MaxHP;
        s_mainCharacter.enemy = clone;
    }

    public void SetPlayerHealthBar(float amount)
    {
        if (amount < 0) amount = 0;
        playerHealthBar.fillAmount = amount;
    }

    public void SetPlayerHealthPoint(float percent)
    {
        s_mainCharacter.SetHealthPoint(percent);
    }

    public void PlayerAttackDamageUp(float percent)
    {
        s_mainCharacter.AdjustAttackDamageRatio(percent);
    }

    public void PlayerAttackSpeedUp(float percent)
    {
        s_mainCharacter.AdjustAttackSpeedRatio(percent);
    }

    public void PlayerShieldSizeUp(float percent)
    {
        s_mainCharacter.AdjustShieldSizeRatio(percent);
    }

    private void EnableCardSystem()
    {
        onCard = true;
        s_cardChanger.gameObject.SetActive(true);
        for (int i = 0; i < cardArr.Length * 3; i++)
        {
            int random1 = Random.Range(0, cardArr.Length);
            int random2 = Random.Range(0, cardArr.Length);
            int temp = cardArr[random1];
            cardArr[random1] = cardArr[random2];
            cardArr[random2] = temp;
        }
        for(int i = 0; i < 3; i++)
        {
            s_cardChanger.ChangeCard(i, cardArr[i]);
        }
    }
}
