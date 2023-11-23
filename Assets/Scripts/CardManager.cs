using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> cardPrefabs = new List<GameObject>(); // List of card prefabs
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject card1Placeholder;
    [SerializeField] private GameObject card2Placeholder;
    [SerializeField] private GameObject dealerPlaceholder1;
    [SerializeField] private GameObject dealerPlaceholder2;
    [SerializeField] private GameObject hitButton;
    private GameObject previousUserCard1;
    private GameObject previousUserCard2;

    private GameObject previousDealerCard1;
    private GameObject previousDealerCard2;
    private int playerAmount;
    private int dealerAmount;
    public Player player = new Player();
    public Player dealer = new Player();
    private Vector3 lastSpawnedCardPosition;
    private List<GameObject> playerCardsHit = new List<GameObject>();

    //public bool makeBet = false;
    public List<Card> decks = new List<Card>();
    public List<Card> usedCards = new List<Card>();

    private int totalCardsInDeck;

    void Start()
    {
        InitializeDecks();
        totalCardsInDeck = decks.Count;
        //DealCards();
        //DealerCards();
        hitButton.SetActive(false);
    }

    void InitializeDecks()
    {
        for (int i = 0; i < 6; i++) // Create 6 decks
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
        GameManager.MakeBet = false;
        // Reset the last spawned card's position
        lastSpawnedCardPosition = Vector3.zero;

        player.ClearHand();
        // Destroy all cards in the playerCards list
        // Check if the playerCards list is not empty
        if (playerCardsHit.Count > 0)
        {
            // Destroy all cards in the playerCardsHit list
            foreach (GameObject card in playerCardsHit)
            {
                Destroy(card);
            }

            // Clear the playerCardsHit list
            playerCardsHit.Clear();
        }


        if (decks.Count < 2)
        {
            Debug.LogWarning("Not enough cards to deal.");
            return;
        }

        // Destroy previously spawned cards
        if (previousUserCard1 != null)
        {
            Destroy(previousUserCard1);
        }
        if (previousUserCard2 != null)
        {
            Destroy(previousUserCard2);
        }

        // Get two random cards from the deck
        Card card1 = decks[Random.Range(0, decks.Count)];
        Card card2 = decks[Random.Range(0, decks.Count)];

        // Remove the dealt cards from the deck
        decks.Remove(card1);
        decks.Remove(card2);

        // Pass the cards to the player class
        // Debug.Log("First Card");
        player.ReceiveCard(card1);
        // Debug.Log("Second Card");
        player.ReceiveCard(card2);

        // Instantiate the cards with default rotations
        GameObject card1Object = Instantiate(card1.cardPrefab, spawnPoint.transform.position, Quaternion.identity);
        GameObject card2Object = Instantiate(card2.cardPrefab, spawnPoint.transform.position, Quaternion.identity);

        // Set the z rotation to 180 degrees
        card1Object.transform.eulerAngles = new Vector3(0, 0, 180);
        card2Object.transform.eulerAngles = new Vector3(0, 0, 180);

        MoveCardToPosition(card1Object, card1Placeholder.transform.position);
        MoveCardToPosition(card2Object, card2Placeholder.transform.position);

        //Debug.Log("Player card 1: " + card1.suit + card1.rank);
        //Debug.Log("Player card 2: " + card2.suit + card2.rank);
        // playerAmount = (int)card1.rank + (int)card2.rank;
        //Debug.Log("Player Sum: " + playerAmount);


        // Set the current cards as the previous cards
        previousUserCard1 = card1Object;
        previousUserCard2 = card2Object;
        if (player.GetHandValue() == 21)
        {
            hitButton.SetActive(false);
        }
        else
        {
            hitButton.SetActive(true);
        }

    }

    public void PlayerHit()
    {

        if (decks.Count == 0)
        {
            Debug.LogWarning("No more cards in the deck.");
            return;
        }

        // Get a random card from the deck
        Card newCard = decks[Random.Range(0, decks.Count)];

        // Pass the new card to the player class
        player.ReceiveCard(newCard);

        // Remove the dealt card from the deck
        decks.Remove(newCard);



        // Calculate the position for the new card
        Vector3 spawnPosition;

        if (lastSpawnedCardPosition == Vector3.zero) // Initial spawn
        {
            spawnPosition = card2Placeholder.transform.position + new Vector3(-0.2f, 0.0001f, 0);
            Debug.Log("FIRST" + spawnPosition);
        }
        else
        {
            spawnPosition = lastSpawnedCardPosition + new Vector3(-0.075f, 0.0001f, 0);
            Debug.Log("NotFIRST" + spawnPosition);
        }

        // Instantiate the new card with default rotation
        GameObject newCardObject = Instantiate(newCard.cardPrefab, spawnPosition, Quaternion.identity);

        playerCardsHit.Add(newCardObject);

        // Set the z rotation to 180 degrees
        newCardObject.transform.eulerAngles = new Vector3(0, 0, 180);

        // Log the newly drawn card
        // Debug.Log("Player drew: " + newCard.rank + " of " + newCard.suit);
        // Debug.Log("Player's Card Amount: " + player.GetHandValue());
        // Update the last spawned card's position
        lastSpawnedCardPosition = spawnPosition;
        if (player.GetHandValue() >= 21)
        {
            hitButton.SetActive(false);
        }


    }


    public void DealerCards()
    {
        if (decks.Count < 2)
        {
            Debug.LogWarning("Not enough cards to deal.");
            return;
        }

        // Destroy previously spawned cards
        if (previousDealerCard1 != null)
        {
            Destroy(previousDealerCard1);
        }
        if (previousDealerCard2 != null)
        {
            Destroy(previousDealerCard2);
        }

        // Get two random cards from the deck
        Card card1 = decks[Random.Range(0, decks.Count)];
        Card card2 = decks[Random.Range(0, decks.Count)];
        dealer.ReceiveCard(card1);
        // Debug.Log("Second Card");
        dealer.ReceiveCard(card2);

        // Remove the dealt cards from the deck
        decks.Remove(card1);
        decks.Remove(card2);

        // Instantiate the cards with default rotations
        GameObject card1Object = Instantiate(card1.cardPrefab, spawnPoint.transform.position, Quaternion.identity);
        GameObject card2Object = Instantiate(card2.cardPrefab, spawnPoint.transform.position, Quaternion.identity);

        // Set the z rotation to 180 degrees
        card1Object.transform.eulerAngles = new Vector3(0, 0, 180);
        card2Object.transform.eulerAngles = new Vector3(0, 0, 0);

        MoveCardToPosition(card1Object, dealerPlaceholder1.transform.position);
        MoveCardToPosition(card2Object, dealerPlaceholder2.transform.position);

        // Debug.Log("Dealer card 1: " + card1.suit + card1.rank);
        // Debug.Log("Dealer card 2: " + card2.suit + card2.rank);

        // Set the current cards as the previous cards
        previousDealerCard1 = card1Object;
        previousDealerCard2 = card2Object;
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

    public void DealerHit()
    {


    }

    public void MakeBet()
    {
        hitButton.SetActive(true);
        GameManager.MakeBet = true;
    }

    public void DealersTurn()
    {
        GameManager.DealersTurn = true;
    }




    // Other functions like ReturnUsedCardsToDeck, CheckReshuffleCondition, etc.

    // ...
}
