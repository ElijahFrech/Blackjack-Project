using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> cardPrefabs = new List<GameObject>(); // List of card prefabs
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject card1Placeholder;
    [SerializeField] private GameObject card2Placeholder;

    public List<Card> decks = new List<Card>();
    public List<Card> usedCards = new List<Card>();

    private int totalCardsInDeck;
    private int reshuffleThreshold;

    void Start()
    {
        InitializeDecks();
        totalCardsInDeck = decks.Count;
        reshuffleThreshold = (int)(totalCardsInDeck * 0.6f);
        DealCards();
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
        string suitName = suit.ToString();
        string rankName = rank.ToString();

        // Construct the prefab name with suit and rank
        string prefabName = $"Card_{suitName}_{rankName}";

        // Find the card prefab by name
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

    public void DealCards()
    {
        if (decks.Count < 2)
        {
            Debug.LogWarning("Not enough cards to deal.");
            return;
        }

        // Get two random cards from the deck
        Card card1 = decks[Random.Range(0, decks.Count)];
        Card card2 = decks[Random.Range(0, decks.Count)];

        // Remove the dealt cards from the deck
        decks.Remove(card1);
        decks.Remove(card2);

        // Instantiate and move the cards
        GameObject card1Object = Instantiate(card1.cardPrefab, spawnPoint.transform.position, Quaternion.identity);
        GameObject card2Object = Instantiate(card2.cardPrefab, spawnPoint.transform.position, Quaternion.identity);

        MoveCardToPosition(card1Object, card1Placeholder.transform.position);
        MoveCardToPosition(card2Object, card2Placeholder.transform.position);
    }

    void MoveCardToPosition(GameObject cardObject, Vector3 targetPosition)
    {
        float duration = 1.0f; // Adjust this for the desired animation speed
        float elapsedTime = 0;

        Vector3 startingPosition = cardObject.transform.position;

        while (elapsedTime < duration)
        {
            cardObject.transform.position = Vector3.Lerp(startingPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
        }

        cardObject.transform.position = targetPosition;
    }




    // Other functions like ReturnUsedCardsToDeck, CheckReshuffleCondition, etc.

    // ...
}
