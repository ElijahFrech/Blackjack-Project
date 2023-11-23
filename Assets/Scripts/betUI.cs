using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class betUI : MonoBehaviour
{
    [SerializeField] private Text betAmountText;
    [SerializeField] private Text currentBetText;
    [SerializeField] private Text currentUserMoneyText;
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [SerializeField] private Button removeButton;
    [SerializeField] private Button betButton;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject standButton;

    int[] betArray = { 1, 5, 10, 25, 50, 100 };

    int currentBet = 1;
    public int currentBetAmount = 0;

    public int userMoney = 500;

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
            upButton.enabled = false;
            downButton.enabled = false;
            removeButton.enabled = false;
            betButton.enabled = false;
        }

        currentBetText.text = string.Format("CURRENT BET: {0} $", currentBetAmount);
        currentUserMoneyText.text = string.Format("USER MONEY: {0} $", userMoney);
    }

    //ADD AN IF THAT CHECKS IF WE BETTING LESS THAN USER MONEY
    public void increaseBet()
    {

        if (counter < 5)
        {
            counter++;
        }
        currentBet = betArray[counter];
        // Debug.Log("counter" + counter.ToString());
        // Debug.Log("CurrentBET" + currentBet.ToString());


        betAmountText.text = string.Format("{0}", currentBet);
    }

    public void decreaseBet()
    {

        if (counter > 0)
        {
            counter--;
        }
        currentBet = betArray[counter];
        // Debug.Log("counter" + counter.ToString());
        // Debug.Log("CurrentBET" + currentBet.ToString());



        betAmountText.text = string.Format("{0}", currentBet);
    }
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
                standButton.SetActive(true);
            }else
            {
                playButton.SetActive(false);
                standButton.SetActive(false);
            }
        }
        else
        {
            Debug.Log("You don't have enough money to make this bet.");
        }
    }

    public void removeBet()
    {
        if (currentBetAmount >= currentBet)
        {
            userMoney += currentBet;
            currentBetAmount -= currentBet;
        }
        else
        {
            Debug.Log("You cant remove more bets than you have in your betting stake.");
        }

        currentBetText.text = string.Format("CURRENT BET: {0} $", currentBetAmount);
        currentUserMoneyText.text = string.Format("USER MONEY: {0} $", userMoney);
    }

}
