using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class betUI : MonoBehaviour
{
    [SerializeField] private Text betAmountText;
    [SerializeField] private Text currentBetText;
    [SerializeField] private Text currentPoolMoneyText;


    int[] betArray = {1, 5, 10, 25, 50, 100};

    int currentBetAmount = 1;
    int currentBetAmountIndex = 0;
    int poolMoney = 500;
    
    // Start is called before the first frame update
    void Start()
    {
        currentBetText.text = string.Format("CURRENT BET: {0} $", currentBetAmount);
        betAmountText.text = string.Format("{0}", currentBetAmount);
        currentPoolMoneyText.text = string.Format("POOL MONEY: {0} $", poolMoney);

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
                currentBetAmount = betArray[counter];
                currentBetAmountIndex = counter;
            }
            counter++;
        }
    }
    //ADD AN IF THAT CHECKS IF WE BETTING LESS THAN POOL MONEY
    public void increaseBet()
    {
        FindCurrentBetAmount();

        if(!(currentBetAmount == 100))
        {
            currentBetAmount = betArray[currentBetAmountIndex + 1];
            poolMoney -= currentBetAmount;
        }

        betAmountText.text = string.Format("{0}", currentBetAmount);
    }

    public void decreaseBet()
    {
        FindCurrentBetAmount();

        if (!(currentBetAmount == 1))
        {
            currentBetAmount = betArray[currentBetAmountIndex - 1];
            poolMoney -= currentBetAmount;
        }

        betAmountText.text = string.Format("{0}", currentBetAmount);
    }

    public void removeBet()
    {
        currentBetAmount = 1;

        betAmountText.text = string.Format("{0}", currentBetAmount);
        currentBetText.text = string.Format("CURRENT BET: {0} $", currentBetAmount);



    }

    public void makeBet()
    {   
        GameManager.MakeBet = true;
        currentBetText.text = string.Format("CURRENT BET: {0} $", currentBetAmount);
        currentPoolMoneyText.text = string.Format("POOL MONEY: {0} $", poolMoney);
    }
}
