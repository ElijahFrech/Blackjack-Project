using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static GameManager;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> cardPrefabs = new List<GameObject>(); // List of card prefabs
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject card1Placeholder;
    [SerializeField] private GameObject card2Placeholder;
    [SerializeField] private GameObject dealerPlaceholder1;
    [SerializeField] private GameObject dealerPlaceholder2;
    [SerializeField] private GameObject doubleButton;
 
    public Player player = new Player();
    public Player dealer = new Player();

    private Vector3 playerLastSpawnedCardPosition;
    private Vector3 dealerLastSpawnedCardPosition;

    public List<GameObject> playerCards = new List<GameObject>();
    public List<GameObject> dealerCards = new List<GameObject>();

    //public bool makeBet = false;
    public List<Card> decks = new List<Card>();
    public List<Card> usedCards = new List<Card>();

    //This method runs at the start of the game.
    void Start()
    {
        InitializeDecks();
    }

    //Initialize the decks
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

    //Making a prefab of a single card.
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

        return cardPrefab;
    }

    //Shuffle the catrds in the deck
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

    //Distribute the first two cards for player.
    public void DealCards()
    {
        player.ClearHand();

        GameManager.MakeBet = false;
        // Reset the last spawned card's position
        playerLastSpawnedCardPosition = Vector3.zero;

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

        // Pass the cards to the player class
        player.ReceiveCard(card1);
        player.ReceiveCard(card2);

        // Instantiate the cards with default rotations
        GameObject card1Object = Instantiate(card1.cardPrefab, spawnPoint.transform.position, Quaternion.identity);
        GameObject card2Object = Instantiate(card2.cardPrefab, spawnPoint.transform.position, Quaternion.identity);

        // Set the z rotation to 180 degrees
        card1Object.transform.eulerAngles = new Vector3(0, 0, 180);
        card2Object.transform.eulerAngles = new Vector3(0, 0, 180);

        MoveCardToPosition(card1Object, card1Placeholder.transform.position);
        MoveCardToPosition(card2Object, card2Placeholder.transform.position);

        // Set the current cards as the previous cards
        playerCards.Add(card1Object);
        playerCards.Add(card2Object);

    }

    //Generate a new card for player.
    public void PlayerHit()
    {
        if (decks.Count < 10)
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

        if (playerLastSpawnedCardPosition == Vector3.zero) // Initial spawn
        {
            spawnPosition = card2Placeholder.transform.position + new Vector3(0.040f, 0.001f, 0.030f);
        }
        else
        {
            spawnPosition = playerLastSpawnedCardPosition + new Vector3(0.040f, 0.001f, 0.030f);
        }

        // Instantiate the new card with default rotation
        GameObject newCardObject = Instantiate(newCard.cardPrefab, spawnPosition, Quaternion.identity);

        playerCards.Add(newCardObject);

        // Set the z rotation to 180 degrees
        newCardObject.transform.eulerAngles = new Vector3(0, 0, 180);

        // Update the last spawned card's position
        playerLastSpawnedCardPosition = spawnPosition;

        //Disappear the Double button
        doubleButton.SetActive(false);
    }

    //Distribut the first two cards for dealer.
    public void DealerCards()
    {
        dealer.ClearHand();

        if (decks.Count < 10)
        {
            Debug.LogWarning("Not enough cards to deal.");
            return;
        }

        // Get two random cards from the deck
        Card card1 = decks[Random.Range(0, decks.Count)];
        Card card2 = decks[Random.Range(0, decks.Count)];

        dealer.ReceiveCard(card1);
        dealer.ReceiveCard(card2);

        // Remove the dealt cards from the deck
        decks.Remove(card1);
        decks.Remove(card2);

        // Instantiate the cards with default rotations
        GameObject card1Object = Instantiate(card1.cardPrefab, spawnPoint.transform.position, Quaternion.identity);
        GameObject card2Object = Instantiate(card2.cardPrefab, spawnPoint.transform.position, Quaternion.identity);

        // Set the z rotation to 180 degrees
        card1Object.transform.eulerAngles = new Vector3(0, 0, 0);
        card2Object.transform.eulerAngles = new Vector3(0, 0, 180);

        MoveCardToPosition(card1Object, dealerPlaceholder2.transform.position);
        MoveCardToPosition(card2Object, dealerPlaceholder1.transform.position);

        dealerLastSpawnedCardPosition = card1Object.transform.position;

        // Set the current cards as the previous cards
        dealerCards.Add(card1Object);
        dealerCards.Add(card2Object);
    }

    //Move the card to specific location according to the parameters.
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

    //Dealer distributes the card for himself according to the cards
    public void DealerHit()
    {
        // Get a random card from the deck
        Card newCard = decks[Random.Range(0, decks.Count)];

        // Pass the new card to the dealer class
        dealer.ReceiveCard(newCard);

        // Remove the dealt card from the deck
        decks.Remove(newCard);

        // Calculate the position for the new card
        Vector3 spawnPosition = dealerLastSpawnedCardPosition + new Vector3(-0.15f, 0, 0);

        // Instantiate the new card with default rotation
        GameObject newCardObject = Instantiate(newCard.cardPrefab, spawnPosition, Quaternion.identity);

        dealerCards.Add(newCardObject);

        // Set the z rotation to 180 degrees
        newCardObject.transform.eulerAngles = new Vector3(0, 0, 180);

        // Update the last spawned card's position
        dealerLastSpawnedCardPosition = spawnPosition;
    }

    //Rotate the dealer's second card face up
    public void rotateDealerCard()
    {
        //Turn the around the faced up card
        dealerCards[0].transform.eulerAngles = new Vector3(0, 0, 180);
    }

    //Make the MakeBet boolean of GameManager to true.
    public void makeBetButton(){
        GameManager.MakeBet = true;
    }

    //Player stands. Rotate the second card of dealer and it's dealer's turn.
    public void standButton(){
        rotateDealerCard();
        GameManager.state = GameManager.GameState.DealerTurn;
    }

    //Double the bet money and it's dealer's turn.
    public void DoubleButton()
    {
        if (GameManager.hasEnoughMoney)
        {
            betUI.userMoney -= betUI.currentBetAmount;
            betUI.currentBetAmount = betUI.currentBetAmount * 2;

            PlayerHit();
            rotateDealerCard();

            GameManager.state = GameManager.GameState.DealerTurn;
        }
    }
}
