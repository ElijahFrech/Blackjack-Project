using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Fields
    [SerializeField] CardManager cardManager;
    [SerializeField] betUI betUI;
    [SerializeField] private GameObject hitButton;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject standButton;
    [SerializeField] private GameObject winOrLosePanel;
    [SerializeField] private TextMeshProUGUI winOrLoseText;

    public static bool makeBet = false;
    public static bool dealersTurn = false;
    public static bool deActivateUIButtons = false;

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

    DealerState GetDealerState()
    {
        int sum = cardManager.dealer.GetHandValue();

        if (sum < 17)   // magic number
            return DealerState.MustHit;
        else if (sum >= 17 && sum <= 21)    // magic number
            return DealerState.MustStay;
        else
            return DealerState.Busted;
    }

    //Public properties
    public static bool MakeBet { get { return makeBet; } set { makeBet = value; } }
    //public static bool DealersTurn { get { return dealersTurn; } set { dealersTurn = value; } }
    // Start is called before the first frame update
    void Start()
    {
        hitButton.SetActive(false);
        // user = new Player();
        // dealer = new Player();

    }

    // private void MakeBet(){

    //     makeBet = true;
    // }

    // Update is called once per frame
    void Update()
    {
        player = cardManager.player;
        dealer = cardManager.dealer;
        //Debug.Log("DEALER =" +dealer.GetHandValue());
        //Debug.Log("PLAYER=" + player.GetHandValue());
        //Debug.Log(cardManager.player.GetHandValue());
        //Debug.Log(betUI.currentBetAmount);
        //Debug.Log(makeBet);
        //Debug.Log(deActivateUIButtons);
        switch (state)
        {
            case GameState.PlayerBetting:

               
                if (betUI.OkayButtonClicked == true)
                {
                    deActivateUIButtons = false;
                    DestroyCards();
                    betUI.OkayButtonClicked = false;
                }

                /*DISABLE HIT AND STAND BUTTONS EXCEPT PLAY SO THE PLAYER HAS TO ACCEPT THE BET BEFORE HITTING OR STANDING*/
                if (makeBet) /*WHEN CLICKING ON PLAY BUTTON THE STATE GETS CHANGED AND THE PLAYER CAN HIT OR STAND*/
                {
                    state = GameState.DealerDealing;
                    /*HERE MUNIR NEEDS TO CHANGE THE CODE SO ALL THE BETTING BUTTONS GET DISABLED SOMEHOW
                    *//*ENABLE HIT AND STAND*/
                    deActivateUIButtons = true;
                    makeBet = false;
                }
                else
                {
                    deActivateUIButtons = false;

                    //playButton.SetActive(false);
                    //standButton.SetActive(false);
                }
                break;
            case GameState.DealerDealing:
                cardManager.DealerCards();
                cardManager.DealCards();

                if (player.GetHandValue() == 21)
                {
                    hitButton.SetActive(false);
                    state = GameState.DealerTurn;
                }
                else
                {
                    hitButton.SetActive(true);
                    state = GameState.PlayerTurn;
                }

                break;
            case GameState.PlayerTurn:
                /*THE USER CAN HIT BUT NOT STAND FOR THE MOMENT WE HAVE TO IMPLEMENT THAT */
                //Debug.Log("Player Turn");
                // Debug.Log(cardManager.player.GetHandValue());
                //if (cardManager.player.GetHandValue() > 21) /*LOGIC FOR LOSING*/
                //{
                //    Debug.Log(cardManager.player.GetHandValue());
                //    /*IF PLAYER GETS BUSTED TAKE CURRENTBETAMOUNT IN BETUI AND SUBSTRACT IT FROM PLAYERBALANCE*/
                //    betUI.userMoney = betUI.userMoney - betUI.currentBetAmount;
                //    Debug.Log("USER MONEY" + betUI.userMoney);
                //    Debug.Log("CURRENT BET AMOUNT" + betUI.currentBetAmount);

                //    Debug.Log("Player Busted");
                //    state = GameState.DealerWin;
                //}
                //else if (dealersTurn) /*HOW TO ADD THE MONEY IF THE PLAYER WINS*/
                //{

                //    Debug.Log("YOU WON BRO");
                //    //state = GameState.DealerTurn;
                //}

                //Debug.Log((player.hand.Count).ToString());
                //Disappear the "Play" button
                playButton.SetActive(false);
                standButton.SetActive(true);

                //Validate if the player hand value is 21 or more
                if (player.GetHandValue() >= 21)
                {
                    cardManager.rotateDealerCard();
                    hitButton.SetActive(false);
                    deActivateUIButtons = false;

                    if (player.GetHandValue() == 21)
                    {
                        state = GameState.DealerTurn;
                    }
                    else if (player.GetHandValue() > 21)
                    {
                        state = GameState.DealerWin;
                        standButton.SetActive(false);
                    }
                }
                break;
            case GameState.DealerTurn:
                //Disappear "Hit" button, "Play" button and "Stand" button
                hitButton.SetActive(false);
                playButton.SetActive(false);
                standButton.SetActive(false);

                //Rotate dealer's second card

                //cardManager.rotateDealerCard();

                //Dealer's hand value
               // int dealerHandValue = dealer.GetHandValue();


                if(dealer.GetHandValue() < 17)
                {
                    cardManager.DealerHit();
                }
                else if(dealer.GetHandValue() > 21)
                {
                    state = GameState.PlayerWin;
                }
                else if(dealer.GetHandValue() > player.GetHandValue())
                {
                    state = GameState.DealerWin;
                }
                else if (dealer.GetHandValue() == player.GetHandValue())
                {
                    state = GameState.Push;

                }
                else
                {
                    //cardManager.DealerHit();
                }
                break;

                /*

                if (dealerHandValue > player.GetHandValue() && dealerHandValue >= 17)
                {
                    state = GameState.DealerWin;
                }
                else if (dealerHandValue < player.GetHandValue() && dealerHandValue < 17)
                {
                    //cardManager.DealerHit();

                    if (dealerHandValue > player.GetHandValue() && !(dealerHandValue > 21))
                    {
                        state = GameState.DealerWin;
                        
                    }
                    else
                    {
                        state = GameState.PlayerWin;
                        
                    }

                }
                else if (player.GetHandValue() > 21) 
                {
                    state = GameState.DealerWin;
                    

                } else if (dealerHandValue == player.GetHandValue())
                {
                    state = GameState.Push;
                    
                }
                break;*/

            case GameState.PlayerWin:
                int winningAmount = betUI.currentBetAmount * 2;

                betUI.userMoney += winningAmount;                   //Pay the user the double amount of chips
                //deActivateUIButtons = false;

                winOrLoseText.text = "You Won";
                winOrLosePanel.SetActive(true);
                Debug.Log("Player won");

                //if (betUI.OkayButtonClicked == true)
                //{
                //DestroyCards();
                //}
                
                state = GameState.PlayerBetting;

                break;

            case GameState.DealerWin:
                betUI.currentBetAmount = 0;                         //Deduct the indicated(bet amount) amount of chips from player
                //deActivateUIButtons = false;

                winOrLoseText.text = "You Lost";
                winOrLosePanel.SetActive(true);
                Debug.Log("Player lost");

                
                
                state = GameState.PlayerBetting;

                break;

            case GameState.Push:
                betUI.userMoney += betUI.currentBetAmount;          //Player neither win neither lose
                //deActivateUIButtons = false;

                winOrLoseText.text = "It is a draw";
                winOrLosePanel.SetActive(true);

                state = GameState.PlayerBetting;

                break;

                ///*HERE WE HAVE TO CREATE A BOOLEAN THAT CHANGES WHEN CLICKING THE STAND BUTTON  
                //}else if(bool standButtonWasHit = false)
                //                 { /*WHEN HITTING ON STAND AND NOT BEING BUSTED IT IS THE DEALER TURN TO PLAY*/
                //                     state = GameState.JustBecameDealerTurn;
                //                 }
                //                 break;
                // case GameState.JustBecameDealerTurn:
                /*TURNS FIRST CARD UPSIDE DOWN AND HITS */
                //     cardManager.DealerCards();
                //     state = GameState.DealerTurn;
                //     break;
                // case GameState.DealerTurn:
                //     switch (GetDealerState())
                //     {
                //         case DealerState.MustHit:
                //             cardManager.DealerHit();
                //             break;
                //         case DealerState.MustStay:
                //             state = GameState.Push;
                //             break;
                //         case DealerState.Busted:
                //             state = GameState.PlayerWin;
                //             break;
                //     }
                //     break;
                // case GameState.PlayerWin:
                //     if (cardManager.player.GetHandValue() > 21)
                //     {
                //         state = GameState.DealerWin;
                //     }
                //     break;
                // case GameState.DealerWin:
                //     if (cardManager.dealer.GetHandValue() > 21)
                //     {
                //         state = GameState.PlayerWin;
                //     }
                //     break;
                // case GameState.Push:
                //     if (cardManager.player.GetHandValue() > 21)
                //     {
                //         state = GameState.DealerWin;
                //     }
                //     else if (cardManager.dealer.GetHandValue() > 21)
                //     {
                //         state = GameState.PlayerWin;
                //     }
                //     break;
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
