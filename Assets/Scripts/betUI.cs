using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class betUI : MonoBehaviour
{
    [SerializeField] private Text betAmountText;
    [SerializeField] private Text currentBetText;
    [SerializeField] private Text currentUserMoneyText;

    int[] betArray = { 1, 5, 10, 25, 50, 100 };

    int currentBet = 1;
    int currentBetAmount = 0;

    int userMoney = 500;

    int counter;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        currentBetText.text = string.Format("CURRENT BET: {0} $", currentBetAmount);
        betAmountText.text = string.Format("{0}", currentBet);
        currentUserMoneyText.text = string.Format("USER MONEY: {0} $", userMoney);
        currentBet = betArray[counter];

    }

    //ADD AN IF THAT CHECKS IF WE BETTING LESS THAN POOL MONEY
    public void increaseBet()
    {

        if (counter < 5)
        {
            counter++;
        }
        currentBet = betArray[counter];
        Debug.Log("counter" + counter.ToString());
        Debug.Log("CurrentBET" + currentBet.ToString());


        betAmountText.text = string.Format("{0}", currentBet);
    }

    public void decreaseBet()
    {

        if (counter > 0)
        {
            counter--;
        }
        currentBet = betArray[counter];
        Debug.Log("counter" + counter.ToString());
        Debug.Log("CurrentBET" + currentBet.ToString());



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
        { Debug.Log("You cant remove more bets than you have in your betting stake.");
        }

        currentBetText.text = string.Format("CURRENT BET: {0} $", currentBetAmount);
        currentUserMoneyText.text = string.Format("USER MONEY: {0} $", userMoney);
    }

}
