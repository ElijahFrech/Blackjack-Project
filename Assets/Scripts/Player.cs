using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private List<Card> hand = new List<Card>();

    public void ReceiveCard(Card card)
    {
        hand.Add(card);
    }

    public int GetHandValue()
    {
        int totalValue = 0;
        int numberOfAces = 0;

        foreach (Card card in hand)
        {
            int cardValue = card.GetValue();
            totalValue += cardValue;

            if (cardValue == 11) // Ace
            {
                numberOfAces++;
            }
        }

        // Handle Aces
        while (numberOfAces > 0 && totalValue > 21)
        {
            totalValue -= 10; // Treat Ace as 1 instead of 11
            numberOfAces--;
        }

        return totalValue;
    }


    public void ClearHand()
    {
        hand.Clear();
    }
}
