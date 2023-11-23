using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Fields
    [SerializeField] CardManager cardManager;
    [SerializeField] betUI betUI;
    private static bool makeBet = false;
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

    // Update is called once per frame
    void Update()
    {

Debug.Log(cardManager.player.GetHandValue());

    }
}
