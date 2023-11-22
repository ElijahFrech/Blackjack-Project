using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class betUI : MonoBehaviour
{
    [SerializeField] private Text betAmountText;
    [SerializeField] private Text currentBetText;
    [SerializeField] private Text currentUserMoneyText;

    int[] betArray = {1, 5, 10, 25, 50, 100};

    int currentBet = 1;
    int currentBetAmount = 0;
    int currentBetIndex = 0;
    int userMoney = 500;
    
    // Start is called before the first frame update
    void Start()
    {
        currentBetText.text = string.Format("CURRENT BET: {0} $", currentBetAmount);
        betAmountText.text = string.Format("{0}", currentBet);
        currentUserMoneyText.text = string.Format("USER MONEY: {0} $", userMoney);

        FindCurrentBetAmount();
    }

    private void FindCurrentBetAmount()
    {
        bool betAmountFound = false;
        int counter = 0;
        
        while (!betAmountFound)
        {
            if (betArray[counter] == int.Parse(betAmountText.text))
            {
                betAmountFound = true;
                currentBet = betArray[counter];
                currentBetIndex = counter;
                Debug.Log("COUNRTER"+counter.ToString());
            }
            if (counter < 5)
            {
                counter++;
            }
            
                
        }
    }
    //ADD AN IF THAT CHECKS IF WE BETTING LESS THAN POOL MONEY
    public void increaseBet()
    {
        FindCurrentBetAmount();

        if(!(currentBet == 100))
        {
            //currentBetIndex += 1; 
            currentBet = betArray[currentBetIndex + 1];
            Debug.Log("INDEX" +currentBetIndex.ToString());   
        }

        betAmountText.text = string.Format("{0}", currentBet);
    }

    public void decreaseBet()
    {
        FindCurrentBetAmount();

        if (!(currentBet == 1))
        {
            //currentBetIndex -= 1;
            currentBet = betArray[currentBetIndex - 1];
        }

        betAmountText.text = string.Format("{0}", currentBetAmount);
    }

    public void removeBet()
    {
        userMoney += currentBetAmount;
        currentBetAmount = 0;

        currentBetText.text = string.Format("CURRENT BET: {0} $", currentBetAmount);
    }

    public void makeBet()
    {
        //GameManager.MakeBet = true;
        currentBetAmount += currentBet;
        userMoney -= currentBet;

        currentBetText.text = string.Format("CURRENT BET: {0} $", currentBetAmount);
        currentUserMoneyText.text = string.Format("USER MONEY: {0} $", userMoney);
    }
}
