using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardChanger : MonoBehaviour
{
    [System.Serializable]
    private struct Pictures
    {
        public string name;
        public Sprite picture;
    }
    

    [SerializeField]
    private Image[] cards;
    [SerializeField]
    private Pictures[] pictures;

    public GameObject gameManager;

    private GameManager s_gameManager;
    private string[] picturesName;

    private void Awake()
    {
        s_gameManager = gameManager.GetComponent<GameManager>();
        picturesName = new string[3];
    }
    public void ChangeCard(int cardIndex, int pictureIndex)
    {
        cards[cardIndex].sprite = pictures[pictureIndex].picture;
        picturesName[cardIndex] = pictures[pictureIndex].name;
    }

    public void onClickCard1()
    {
        Switcher(0);
    }
    public void onClickCard2()
    {
        Switcher(1);
    }
    public void onClickCard3()
    {
        Switcher(2);
    }

    private void ExPHealingCard(float percent)
    {
        s_gameManager.SetPlayerHealthPoint(percent);
    }

    private void ExPAttackDamageCard(float percent)
    {
        s_gameManager.PlayerAttackDamageUp(percent);
    }

    private void ExPAttackSpeedCard(float percent)
    {
        s_gameManager.PlayerAttackSpeedUp(percent);
    }

    private void ExPShieldSizeCard(float percent)
    {
        s_gameManager.PlayerShieldSizeUp(percent);
    }

    private void Switcher(int switchIndex)
    {
        switch(picturesName[switchIndex])
        {
            case "100PHealingCard":
                ExPHealingCard(100);
                break;
            case "20PAttackDamageUpCard":
                ExPAttackDamageCard(20);
                break;
            case "20PAttackSpeedUpCard":
                ExPAttackSpeedCard(20);
                break;
            case "30PShieldSizeUpCard":
                ExPShieldSizeCard(30);
                break;
        }
        s_gameManager.IsSelect = true;
        this.gameObject.SetActive(false);
    }
}