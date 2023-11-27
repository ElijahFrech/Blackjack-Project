using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class betUI : MonoBehaviour
{
    [SerializeField] private Text betAmountText;
    [SerializeField] private Text currentBetText;
    [SerializeField] private Text currentUserMoneyText;
    [SerializeField] private GameObject upButton;
    [SerializeField] private GameObject downButton;
    [SerializeField] private GameObject removeButton;
    [SerializeField] private GameObject betButton;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject standButton;

    int[] betArray = { 1, 5, 10, 25, 50, 100 };

    int currentBet = 1;
    public int currentBetAmount = 0;
    public int userMoney = 500;
    public bool okayButtonClicked = false;
    int counter;

    void Start()
    {
        counter = 0;
        currentBetText.text = string.Format("CURRENT BET: {0} $", currentBetAmount);
        betAmountText.text = string.Format("{0}", currentBet);
        currentUserMoneyText.text = string.Format("USER MONEY: {0} $", userMoney);
        currentBet = betArray[counter];
        playButton.SetActive(false);
        standButton.SetActive(false);
    }

    void Update()
    {
        if (GameManager.deActivateUIButtons)
        {
            upButton.SetActive(false);
            downButton.SetActive(false);
            removeButton.SetActive(false);
            betButton.SetActive(false);
        }
        else
        {
            upButton.SetActive(true);
            downButton.SetActive(true);
            removeButton.SetActive(true);
            betButton.SetActive(true);
        }

        currentBetText.text = string.Format("CURRENT BET: {0} $", currentBetAmount);
        currentUserMoneyText.text = string.Format("USER MONEY: {0} $", userMoney);
    }

    //Increase the bet amount
    //ADD AN IF THAT CHECKS IF WE BETTING LESS THAN USER MONEY
    public void increaseBet()
    {

        if (counter < 5)
        {
            counter++;
        }
        currentBet = betArray[counter];

        betAmountText.text = string.Format("{0}", currentBet);
    }

    //Decrease the bet amount
    public void decreaseBet()
    {
        if (counter > 0)
        {
            counter--;
        }
        currentBet = betArray[counter];

        betAmountText.text = string.Format("{0}", currentBet);
    }

    //Set the current bet amount
    public void makeBet()
    {
        if (currentBet <= userMoney)
        {
            currentBetAmount += currentBet;
            userMoney -= currentBet;

            currentBetText.text = string.Format("CURRENT BET: {0} $", currentBetAmount);
            currentUserMoneyText.text = string.Format("USER MONEY: {0} $", userMoney);
            
            if(!(currentBetAmount == 0))
            {
                playButton.SetActive(true);
            }else
            {
                playButton.SetActive(false);
            }
        }
        else
        {
            Debug.Log("You don't have enough money to make this bet.");
        }
    }

    //Remove the bet amount
    public void removeBet()
    {
        if (currentBetAmount >= currentBet)
        {
            userMoney += currentBet;
            currentBetAmount -= currentBet;

            if(currentBetAmount == 0)
            {
                playButton.SetActive(false);
                standButton.SetActive(false);
            }
        }
        else
        {
            Debug.Log("You cant remove more bets than you have in your betting stake.");
        }

        currentBetText.text = string.Format("CURRENT BET: {0} $", currentBetAmount);
        currentUserMoneyText.text = string.Format("USER MONEY: {0} $", userMoney);
    }


    public bool OkayButtonClicked { 
        get { return okayButtonClicked; } 
        set { okayButtonClicked = value; } 
    }
}
