using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DealerController : MonoBehaviour
{
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
    
    public DeckController deckController;
    public Transform dealerCardSpot;
    public Transform playerCardSpot;
    public float horiSpaceBetweenPlayerCards;
    public float vertSpaceBetweenPlayerCards;
    public float horiSpaceBetweenDealerCards;
    public ChipManager chipManager;
    public float stayHoldTime;
    
    private IList<CardController> dealerCards;
    private IList<CardController> playerCards;
    private float cardDepth = 0.001f;
    private GameState state;
    private float touchStartedTime;
    private CrosshairController crosshairController;
    
    public GameState GetGameState()
    {
        return state;
    }
    
    void Start()
    {
        crosshairController = GameObject.FindObjectOfType<CrosshairController>();
        
        dealerCards = new List<CardController>();
        playerCards = new List<CardController>();
        
        ResetRound();
        StartCoroutine(StateMachine());
    }
    
    IEnumerator StateMachine()  // ugh...
    {
        yield return new WaitForEndOfFrame();
        
        while (true)
        {
            // change to switch
            if (state == GameState.PlayerBetting)
            {   
                // It makes sense to put the state machine in DealerController because
                //  the dealer is the one who controls the actual game state in real life.
                // However, it does not make sense to use crosshair controller in dealer controller since
                //  it's the player who moves the chips not the dealer.
                // What should I do? hm
                GameObject touchingObject = crosshairController.GetTouchingObject();
                if (touchingObject != null && touchingObject.CompareTag("Chip"))
                {
                    chipManager.SelectChipsAbove(touchingObject);
                }
                else
                {
                    chipManager.DeselectChips();
                }
                
                if (Input.GetMouseButton(0))
                {
                    crosshairController.SetShouldDetectNewObject(false);
                    chipManager.MoveSelectedChips(crosshairController.transform.position);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    // TODO (OPTIONAL) : be able to split player chip towers
                    crosshairController.SetShouldDetectNewObject(true);
                    if (chipManager.BetSelectedChips()) // should not be inside DealerController. should be in something like PlayerController
                        state = GameState.DealerDealing;
                }
                
                yield return null;
            }
            else if (state == GameState.DealerDealing)
            {
                float delay = 0.25f;
                
                DealPlayerCard();
                yield return new WaitForSeconds(delay);
                
                DealDealerCard(true);
                yield return new WaitForSeconds(delay);
                
                DealPlayerCard();
                yield return new WaitForSeconds(delay);
                
                DealDealerCard(false);
                yield return new WaitForSeconds(delay);
                
                state = GameState.PlayerTurn;
            }
            else if (state == GameState.PlayerTurn)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    touchStartedTime = Time.time;
                }
                else if (Input.GetMouseButton(0))
                {
                    if (Time.time - touchStartedTime >= stayHoldTime)
                    {
                        state = GameState.JustBecameDealerTurn;
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (Time.time - touchStartedTime >= stayHoldTime)
                    {
                        state = GameState.JustBecameDealerTurn;
                    }
                    else
                    {
                        DealPlayerCard();
                        if (GetPlayerSum() > 21)    // magic number
                            state = GameState.DealerWin;
                    }
                }
                
                yield return null;
            }
            else if (state == GameState.JustBecameDealerTurn)
            {
                dealerCards[dealerCards.Count-1].flip();
                state = GameState.DealerTurn;
                
                yield return new WaitForSeconds(0.5f);
            }
            else if (state == GameState.DealerTurn)
            {
                DealerState dealerState = GetDealerState();
                if (dealerState == DealerState.MustStay)
                {
                    int dealerSum = GetDealerSum();
                    int playerSum = GetPlayerSum();
                    
                    if (playerSum < dealerSum)
                        state = GameState.DealerWin;
                    else if (playerSum > dealerSum)
                        state = GameState.PlayerWin;
                    else
                        state = GameState.Push;
                }
                else if (dealerState == DealerState.Busted)
                {
                    state = GameState.PlayerWin;
                }
                else
                {
                    DealDealerCard(true);
                    yield return new WaitForSeconds(0.5f);
                }
            }
            else if (state == GameState.DealerWin)
            {
                chipManager.DealerGetsChips();
                yield return new WaitForSeconds(0.5f);
                
                ResetRound();
            }
            else if (state == GameState.PlayerWin)
            {
                chipManager.DealerGivesChips(IsPlayerBlackJack());
                yield return new WaitForSeconds(1.0f);
                
                chipManager.PlayerGetsChips();  // shouldn't be in DealerController but PlayerController
                yield return new WaitForSeconds(0.5f);
                
                ResetRound();
            }
            else if (state == GameState.Push)
            {
                chipManager.PlayerGetsChips();  
                yield return new WaitForSeconds(0.5f);
                
                ResetRound();
            }
        }
    }
    
    // is it tie when one has blackjack and one has 21?
    bool IsPlayerBlackJack()
    {
        if (playerCards.Count != 2) return false;
        
        if ((playerCards[0].getValue() == 11 && playerCards[1].getValue() == 10) || 
            (playerCards[0].getValue() == 10 && playerCards[1].getValue() == 11))
            return true;
        
        return false;
    }
    
    int GetPlayerSum()
    {
        int sum = 0;
        for (int i = 0; i < playerCards.Count; i++)
        {
            sum += playerCards[i].getValue();
        }
        
        for (int i = 0; i < playerCards.Count && sum > 21; i++)
        {
            int cardVal = playerCards[i].getValue();
            if (cardVal == 11)  // magic number
            {
                playerCards[i].setValue(1);   // magic number
                sum -= 10;  // magic number
            }
        }
        
        return sum;
    }
    
    DealerState GetDealerState()
    {
        int sum = GetDealerSum();
        
        if (sum < 17)   // magic number
            return DealerState.MustHit;
        else if (sum >= 17 && sum <= 21)    // magic number
            return DealerState.MustStay;
        else
            return DealerState.Busted;
    }
    
    int GetDealerSum()
    {
        int sum = 0;
        bool isSoft = false;
        for (int i = 0; i < dealerCards.Count; i++)
        {
            int cardVal = dealerCards[i].getValue();
            if (cardVal == 11)  // magic number
                isSoft = true;
            sum += cardVal;
        }
        
        if (sum > 21 && isSoft)   // magic number
        {
            for (int i = 0; i < dealerCards.Count; i++)
            {
                int cardVal = dealerCards[i].getValue();
                if (cardVal == 11)  // magic number
                {
                    dealerCards[i].setValue(1);   // magic number
                    sum -= 10;  // magic number
                    break;
                }
            }
        }
        
        return sum;
    }

    void DealPlayerCard()
    {
        var nextCard = deckController.GetNextCard().GetComponent<CardController>();
        Vector3 cardPos = new Vector3(playerCardSpot.position.x + (horiSpaceBetweenPlayerCards * playerCards.Count), 
                                      playerCardSpot.position.y + (cardDepth * playerCards.Count),  
                                      playerCardSpot.position.z + (vertSpaceBetweenPlayerCards * playerCards.Count));
        nextCard.transform.position = cardPos;
        nextCard.flip();
        playerCards.Add(nextCard);
    }
    
    void DealDealerCard(bool showCard)
    {       
        var nextCard = deckController.GetNextCard().GetComponent<CardController>();
        Vector3 cardPos = new Vector3(dealerCardSpot.position.x - (horiSpaceBetweenDealerCards * dealerCards.Count), 
                                      dealerCardSpot.position.y + (cardDepth * dealerCards.Count), 
                                      dealerCardSpot.position.z);
        nextCard.transform.position = cardPos;
        if (showCard)
            nextCard.flip();
        dealerCards.Add(nextCard);
    }
    
    void ResetRound()
    {        
        foreach (var card in playerCards)
            Destroy(card.gameObject);
        playerCards.Clear();        
        
        foreach (var card in dealerCards)
            Destroy(card.gameObject);
        dealerCards.Clear();
        
        state = GameState.PlayerBetting;
    }
}
