using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public enum Rank
    {
        Ace = 1,
        Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten,
        Jack, Queen, King
    }

    public Suit suit;
    public Rank rank;
    public GameObject cardPrefab; // Reference to the associated prefab

    public Card(Suit suit, Rank rank, GameObject cardPrefab)
    {
        this.suit = suit;
        this.rank = rank;
        this.cardPrefab = cardPrefab;
    }

    public int GetValue()
    {
        if (rank == Rank.Jack || rank == Rank.Queen || rank == Rank.King)
        {
            return 10;
        }
        else if (rank == Rank.Ace)
        {
            return 11; // You may want to handle the Ace value differently depending on the situation
        }
        else
        {
            return (int)rank;
        }
    }

    public override string ToString()
    {
        return $"{rank} of {suit}";
    }
}

