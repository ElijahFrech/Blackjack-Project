using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Fields
    [SerializeField] CardManager cardManager;
    [SerializeField] betUI betUI;
    public static bool makeBet = false;
    public static bool dealersTurn = false;
    // private Player user;
    // private Player dealer;

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

   private GameState state;

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
    public static bool DealersTurn { get { return dealersTurn; } set { dealersTurn = value; } }
    // Start is called before the first frame update
    void Start()
    {
        
        // user = new Player();
        // dealer = new Player();

    }

    // private void MakeBet(){

    //     makeBet = true;
    // }

    // Update is called once per frame
    void Update()
    {

//Debug.Log(cardManager.player.GetHandValue());
//Debug.Log(betUI.currentBetAmount);
//Debug.Log(makeBet);
        switch (state)
        {
            case GameState.PlayerBetting:
                if (makeBet)
                {
                    state = GameState.DealerDealing;
                    /*HERE MUNIR NEEDS TO CHANGE THE CODE SO ALL THE BETTING BUTTONS GET DISABLED SOMEHOW
                    */
                    makeBet = false;
                }
                break;
            case GameState.DealerDealing:
                cardManager.DealerCards();
                cardManager.DealCards();
                state = GameState.PlayerTurn;
                break;
            case GameState.PlayerTurn:
                if (cardManager.player.GetHandValue() > 21)
                {
                    Debug.Log("Player Busted");
                    state = GameState.DealerTurn;
                }
                break;
            // case GameState.JustBecameDealerTurn:
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
}
