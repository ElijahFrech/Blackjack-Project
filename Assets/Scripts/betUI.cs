using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class betUI : MonoBehaviour
{
    [SerializeField] private GameObject upButton;
    [SerializeField] private GameObject downButton;
    [SerializeField] private GameObject removeButton;
    [SerializeField] private GameObject betButton;
    [SerializeField] private Text betAmountText;
    [SerializeField] private Text currentBetText;
    [SerializeField] private Text currentPoolMoneyText;


    int[] betArray = {5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100};

    int currentBetAmount = 5;
    int currentBetAmountIndex = 0;
    int poolMoney = 500;
    
    // Start is called before the first frame update
    void Start()
    {
        FindCurrentBetAmount();
    }

    private void FindCurrentBetAmount()
    {
        bool betAmountFound = false;
        int counter = 0;
        
        while (betAmountFound == false)
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

    public void increaseBet()
    {
        FindCurrentBetAmount();

        currentBetAmount = betArray[currentBetAmountIndex + 1];
        poolMoney -= currentBetAmount;

        betAmountText.text = string.Format("{0} $", currentBetAmount);
    }

    public void decreaseBet()
    {
        FindCurrentBetAmount();

        currentBetAmount = betArray[currentBetAmountIndex - 1];
        poolMoney -= currentBetAmount;

        betAmountText.text = string.Format("{0} $", currentBetAmount);
    }

    public void removeBet()
    {
        currentBetAmount = 5;

        betAmountText.text = string.Format("{0} $", currentBetAmount);
        currentBetText.text = string.Format("CURRENT BET: {0} $", currentBetAmount);



    }

    public void makeBet()
    {
        GameManager.MakeBet = true;
        currentBetText.text = string.Format("CURRENT BET: {0} $", currentBetAmount);
        currentPoolMoneyText.text = string.Format("POOL MONEY: {0} $", poolMoney);
    }
}
