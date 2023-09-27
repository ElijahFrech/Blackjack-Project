using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> cardPrefabs = new List<GameObject>(); // List of card prefabs

    public List<Card> decks = new List<Card>();
    public List<Card> usedCards = new List<Card>();

    private int totalCardsInDeck;
    private int reshuffleThreshold;

    void Start()
    {
        InitializeDecks();
        totalCardsInDeck = decks.Count;
        reshuffleThreshold = (int)(totalCardsInDeck * 0.6f);
    }

    void InitializeDecks()
    {
        foreach (Card.Suit suit in System.Enum.GetValues(typeof(Card.Suit)))
        {
            foreach (Card.Rank rank in System.Enum.GetValues(typeof(Card.Rank)))
            {
                if (rank != Card.Rank.Ace)
                {
                    GameObject cardPrefab = GetCardPrefab(suit, rank);
                    Card card = new Card(suit, rank, cardPrefab);
                    decks.Add(card);
                }
            }
        }

        // Add Ace separately
        foreach (Card.Suit suit in System.Enum.GetValues(typeof(Card.Suit)))
        {
            GameObject cardPrefab = GetCardPrefab(suit, Card.Rank.Ace);
            Card card = new Card(suit, Card.Rank.Ace, cardPrefab);
            decks.Add(card);
        }

        ShuffleDecks();
    }

    GameObject GetCardPrefab(Card.Suit suit, Card.Rank rank)
    {
        string prefabName = $"{rank.ToString()}Of{suit.ToString()}"; // TODO: Fix this line to work with the syntax
        return cardPrefabs.Find(prefab => prefab.name == prefabName);
    }

    void ShuffleDecks()
    {
        for (int i = 0; i < decks.Count; i++)
        {
            Card temp = decks[i];
            int randomIndex = Random.Range(i, decks.Count);
            decks[i] = decks[randomIndex];
            decks[randomIndex] = temp;
        }
    }

    Card DealCard()
    {
        if (decks.Count == 0)
        {
            decks.AddRange(usedCards);
            usedCards.Clear();
            ShuffleDecks();
        }

        Card card = decks[0];
        decks.RemoveAt(0);
        usedCards.Add(card);

        if (decks.Count <= reshuffleThreshold)
        {
            ShuffleDecks();
        }

        return card;
    }

    // Other functions like ReturnUsedCardsToDeck, CheckReshuffleCondition, etc.

    // ...
}
