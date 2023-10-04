using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> cardPrefabs = new List<GameObject>(); // List of card prefabs
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject card1Placeholder;
    [SerializeField] private GameObject card2Placeholder;
    private GameObject previousCard1;
    private GameObject previousCard2;

    public List<Card> decks = new List<Card>();
    public List<Card> usedCards = new List<Card>();

    private int totalCardsInDeck;

    void Start()
    {
        InitializeDecks();
        totalCardsInDeck = decks.Count;
        DealCards();

    }

    void InitializeDecks()
    {
        //for (int i = 0; i < 6; i++) // Create 6 decks
        //{
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
        //}
    }

    GameObject GetCardPrefab(Card.Suit suit, Card.Rank rank)
    {
        string suitName = suit.ToString();
        string rankName;

        if (rank == Card.Rank.Ace)
        {
            rankName = "Ace";
        }
        else if (rank >= Card.Rank.Two && rank <= Card.Rank.Ten)
        {
            rankName = ((int)rank).ToString("D2");
        }
        else
        {
            rankName = rank.ToString();
        }

        // Construct the prefab name with suit and rank
        string prefabName = $"Card_{suitName}_B_{rankName}";

        // Find the card prefab by name
        GameObject cardPrefab = cardPrefabs.Find(prefab => prefab.name == prefabName);

        //if (cardPrefab == null)
        //{
        //    Debug.LogWarning("Prefab not found for: " + prefabName);
        //}
        //else
        //{
        //    Debug.Log("Found prefab for: " + prefabName);
        //}

        return cardPrefab;
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

        // Destroy previously spawned cards
        if (previousCard1 != null)
        {
            Destroy(previousCard1);
        }
        if (previousCard2 != null)
        {
            Destroy(previousCard2);
        }

        // Get two random cards from the deck
        Card card1 = decks[Random.Range(0, decks.Count)];
        Card card2 = decks[Random.Range(0, decks.Count)];

        // Remove the dealt cards from the deck
        decks.Remove(card1);
        decks.Remove(card2);

        // Instantiate the cards with default rotations
        GameObject card1Object = Instantiate(card1.cardPrefab, spawnPoint.transform.position, Quaternion.identity);
        GameObject card2Object = Instantiate(card2.cardPrefab, spawnPoint.transform.position, Quaternion.identity);

        // Set the z rotation to 180 degrees
        card1Object.transform.eulerAngles = new Vector3(0, 0, 180);
        card2Object.transform.eulerAngles = new Vector3(0, 0, 180);

        MoveCardToPosition(card1Object, card1Placeholder.transform.position);
        MoveCardToPosition(card2Object, card2Placeholder.transform.position);

        Debug.Log("Player card 1: " + card1.suit + card1.rank);
        Debug.Log("Player card 2: " + card2.suit + card2.rank);

        // Set the current cards as the previous cards
        previousCard1 = card1Object;
        previousCard2 = card2Object;
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
