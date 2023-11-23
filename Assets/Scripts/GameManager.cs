using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Fields
    [SerializeField] CardManager cardManager;
    [SerializeField] betUI betUI;
    public static bool makeBet = false;
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
Debug.Log(makeBet);
        // switch (state)
        // {
        //     case GameState.PlayerBetting:
        //         if (makeBet)
        //         {
        //             cardManager.gameState = GameState.DealerDealing;
        //             makeBet = false;
        //         }
        //         break;
        //     case GameState.DealerDealing:
        //         cardManager.DealCards();
        //         cardManager.gameState = GameState.PlayerTurn;
        //         break;
        //     case GameState.PlayerTurn:
        //         if (cardManager.player.GetHandValue() > 21)
        //         {
        //             cardManager.gameState = GameState.DealerTurn;
        //         }
        //         break;
        //     case GameState.JustBecameDealerTurn:
        //         cardManager.DealerCards();
        //         cardManager.gameState = GameState.DealerTurn;
        //         break;
        //     case GameState.DealerTurn:
        //         switch (GetDealerState())
        //         {
        //             case DealerState.MustHit:
        //                 cardManager.DealerHit();
        //                 break;
        //             case DealerState.MustStay:
        //                 cardManager.gameState = GameState.Push;
        //                 break;
        //             case DealerState.Busted:
        //                 cardManager.gameState = GameState.PlayerWin;
        //                 break;
        //         }
        //         break;
        //     case GameState.PlayerWin:
        //         if (cardManager.player.GetHandValue() > 21)
        //         {
        //             cardManager.gameState = GameState.DealerWin;
        //         }
        //         break;
        //     case GameState.DealerWin:
        //         if (cardManager.dealer.GetHandValue() > 21)
        //         {
        //             cardManager.gameState = GameState.PlayerWin;
        //         }
        //         break;
        //     case GameState.Push:
        //         if (cardManager.player.GetHandValue() > 21)
        //         {
        //             cardManager.gameState = GameState.DealerWin;
        //         }
        //         else if (cardManager.dealer.GetHandValue() > 21)
        //         {
        //             cardManager.gameState = GameState.PlayerWin;
        //         }
        //         break;
        // }

    }
}
