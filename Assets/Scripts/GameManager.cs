using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Fields
    [SerializeField] CardManager cardManager;
    //[SerializeField] betUI betUI;
    [SerializeField] private GameObject hitButton;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject standButton;
    [SerializeField] private GameObject doubleButton;
    [SerializeField] private GameObject winOrLosePanel;
    [SerializeField] private TextMeshProUGUI winOrLoseText;

    public static bool makeBet = false;
    public static bool dealersTurn = false;
    public static bool deActivateUIButtons = false;
    public static bool hasEnoughMoney = false;

    public Player player;
    public Player dealer;

    public enum GameState
    {
        PlayerBetting,
        DealerDealing,
        PlayerTurn,
        JustBecameDealerTurn,
        DealerTurn,
        PlayerWin,
        DealerWin,
        Push,
    };

    public enum DealerState
    {
        MustHit,
        MustStay,
        Busted,
    };

    public static GameState state;

    //Public properties
    public static bool MakeBet { get { return makeBet; } set { makeBet = value; } }

    // Start is called before the first frame update
    void Start()
    {
        hitButton.SetActive(false);
        doubleButton.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        player = cardManager.player;
        dealer = cardManager.dealer;

        switch (state)
        {
            case GameState.PlayerBetting:
                if (betUI.OkayButtonClicked)
                {
                    if(betUI.userMoney <= 0){
                        betUI.userMoney = 500;
                        betUI.currentBetAmount = 0;
                        SceneManager.LoadScene(0);

                    }
                    else{

                        deActivateUIButtons = false;
                        DestroyCards();
                        betUI.OkayButtonClicked = false;
                    }

                }

                if (makeBet)
                {
                    state = GameState.DealerDealing;
                    deActivateUIButtons = true;
                    makeBet = false;
                }
                
                break;

            case GameState.DealerDealing:
                cardManager.DealerCards();
                cardManager.DealCards();

                if (player.GetHandValue() == 21)
                {
                    hitButton.SetActive(false);
                    doubleButton.SetActive(false);
                    cardManager.rotateDealerCard();
                    state = GameState.DealerTurn;
                }
                else
                {
                    hitButton.SetActive(true);
                    doubleButton.SetActive(true);

                    state = GameState.PlayerTurn;
                }

                break;

            case GameState.PlayerTurn:
                //Disappear the "Play" button and the "stand" button
                playButton.SetActive(false);
                standButton.SetActive(true);


                //Validate if the player hand value is 21 or more
                if (player.GetHandValue() >= 21)
                {
                    Debug.Log("Player hand value is 21 or more");
                    cardManager.rotateDealerCard();
                    
                    hitButton.SetActive(false);

                    if (player.GetHandValue() == 21)
                    {

                        state = GameState.DealerTurn;
                    }
                    else if (player.GetHandValue() > 21)
                    {
                        state = GameState.DealerWin;
                        standButton.SetActive(false);
                        doubleButton.SetActive(false);
                    }
                }
                break;

            case GameState.DealerTurn:
                //Disappear "Hit" button, "Play" button, "Stand" button and "double" button
                hitButton.SetActive(false);
                playButton.SetActive(false);
                standButton.SetActive(false);
                doubleButton.SetActive(false);

                if(player.GetHandValue() > 21)
                {
                    state = GameState.DealerWin;
                }
                else if (dealer.GetHandValue() < 17)
                {
                    cardManager.DealerHit();
                }
                else if (dealer.GetHandValue() > 21)
                {
                    state = GameState.PlayerWin;
                }
                else if (dealer.GetHandValue() > player.GetHandValue())
                {
                    state = GameState.DealerWin;
                }
                else if (dealer.GetHandValue() == player.GetHandValue())
                {
                    state = GameState.Push;

                }else if (dealer.GetHandValue() < player.GetHandValue()){
                    state = GameState.PlayerWin;

                }

                break;

            case GameState.PlayerWin:
                int winningAmount = betUI.currentBetAmount * 2;

                betUI.userMoney += winningAmount;                   //Pay the user the double amount of chips
                betUI.currentBetAmount = 0;

                winOrLoseText.text = "You Won";
                winOrLosePanel.SetActive(true);
                Debug.Log("Player won");

                state = GameState.PlayerBetting;

                break;

            case GameState.DealerWin:
                betUI.currentBetAmount = 0;           

                if(betUI.userMoney == 0){
                    winOrLoseText.text = "GAME OVER YOU LOST";
                    winOrLosePanel.SetActive(true);
                    Debug.Log("Player is broke");

                }  else{
                    winOrLoseText.text = "You Lost";
                    winOrLosePanel.SetActive(true);
                    Debug.Log("Player lost");

                }

                state = GameState.PlayerBetting;

                break;

            case GameState.Push:
                betUI.userMoney += betUI.currentBetAmount;
                betUI.currentBetAmount = 0;

                winOrLoseText.text = "It is a draw";
                winOrLosePanel.SetActive(true);

                state = GameState.PlayerBetting;

                break;
        }

        //Verify if the user has enough money for doubling
        if(betUI.userMoney >= betUI.currentBetAmount)
        {
            hasEnoughMoney = true;
        }
        else
        {
            hasEnoughMoney = false;
            doubleButton.SetActive(false);
        }
    }

    private void DestroyCards()
    {
        if (cardManager.playerCards.Count > 0)
        {
            // Destroy all the player cards
            foreach (GameObject card in cardManager.playerCards)
            {
                Destroy(card);
            }
        }
        // Clear the playerCards list
        cardManager.playerCards.Clear();

        if (cardManager.dealerCards.Count > 0)
        {
            // Destroy all the dealer cards
            foreach (GameObject card in cardManager.dealerCards)
            {
                Destroy(card);
            }
        }
        // Clear the dealerCards list
        cardManager.dealerCards.Clear();
    }
}
