using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DealCard : MonoBehaviour
{
    public GameObject[] dealtCard;
    public GameObject[] playerDeck;
    public GameObject[] player2Deck;
    public GameObject[] player3Deck;
    public GameObject[] player4Deck;
    public int cardGenerate;
    public GameObject dealButton;
    public Button nextButton;
    public GameObject winText;
    public GameObject loseText;
    public GameObject p1won;
    public GameObject p2won;
    public GameObject p3won;
    public GameObject p4won;

    private int winningPlayer = 1;
    private HashSet<int> dealtCardNumbers = new HashSet<int>();
    private const int NumCards = 13;
    public TextMeshProUGUI trumpText;

    public GameObject cardSlotPrefab;
    public Transform cardSlotContainer;
    public Transform cardSlotAbove;
    public GameObject wrongCard;

    private List<GameObject> cardSlots = new List<GameObject>();
    private List<GameObject> displayedCards = new List<GameObject>();
    public List<GameObject> p2deck = new List<GameObject>();
    public List<GameObject> p3deck = new List<GameObject>();
    public List<GameObject> p4deck = new List<GameObject>();

    public CardManager cardManager;
    public List<GameObject> p1SelectedCards = new List<GameObject>();
    public List<GameObject> p2SelectedCards = new List<GameObject>();
    public List<GameObject> p3SelectedCards = new List<GameObject>();
    public List<GameObject> p4SelectedCards = new List<GameObject>();
    public TextMeshProUGUI buttonText;
    public AudioSource cardDeal;
    public AudioSource winGame;
    public AudioSource loseGame;

    private void Start()
    {
        if (playerDeck == null)
        {
            playerDeck = new GameObject[NumCards];
        }
    }

    public void DealMyCard()
    {
        cardDeal.Play();
        CreateCardSlots();
        trumpText.text = "TRUMP SUIT:\n" + cardManager.trumpSuit;
        DisplayCards(playerDeck);
        DealToDeck(player2Deck, p2deck);
        DealToDeck(player3Deck, p3deck);
        DealToDeck(player4Deck, p4deck);
        NextTurn();
    }

    public void NextTurn()
    {
        p1won.SetActive(false);
        p2won.SetActive(false);
        p3won.SetActive(false);
        p4won.SetActive(false);
        nextButton.interactable = false;
        buttonText.text = " ";
        if (p2SelectedCards.Count > 0)
        {
            DeactivateTransform();
            DeactivateCard(p1SelectedCards);
            DeactivateCard(p2SelectedCards);
            DeactivateCard(p3SelectedCards);
            DeactivateCard(p4SelectedCards);

        }
        if (winningPlayer == 1)
        {
            nextButton.gameObject.SetActive(false);
            AttachClickHandlers(displayedCards, p1SelectedCards);
        }
        else if (winningPlayer == 2)
        {
            StartCoroutine(Player2Won());
            AttachClickHandlers(displayedCards, p2SelectedCards);
        }
        else if (winningPlayer == 3)
        {
            StartCoroutine(Player3Won());
            AttachClickHandlers(displayedCards, p3SelectedCards);
        }
        else if (winningPlayer == 4)
        {
            StartCoroutine(Start4());
            AttachClickHandlers(displayedCards, p4SelectedCards);
        }
    }

    public void CreateCardSlots()
    {
        for (int i = 0; i < NumCards; i++)
        {
            GameObject cardSlot = Instantiate(cardSlotPrefab, cardSlotContainer);
            cardSlots.Add(cardSlot);
            cardSlot.SetActive(false);
        }
    }

    public void DisplayCards(GameObject[] deck)
    {
        //activating all card slots
        foreach (var cardSlot in cardSlots)
        {
            cardSlot.SetActive(true);
        }
        for (int i = 0; i < NumCards; i++)
        {
            do
            {
                //generating a random card index within the range of dealtCard array
                cardGenerate = Random.Range(2, dealtCard.Length);
            } while (dealtCardNumbers.Contains(cardGenerate));

            if (cardGenerate < dealtCard.Length)
            {
                GameObject card = dealtCard[cardGenerate];

                //geting Button and Image components of the current card slot
                Button cardButton = cardSlots[i].GetComponent<Button>();
                Image cardImage = cardSlots[i].GetComponent<Image>();

                //activating the Button and Image components
                cardSlots[i].SetActive(true);
                //dealtCard[cardGenerate].SetActive(true);

                SpriteRenderer cardRenderer = dealtCard[cardGenerate]?.GetComponent<SpriteRenderer>();

                if (cardRenderer != null)
                {
                    cardButton.gameObject.SetActive(true);
                    cardImage.gameObject.SetActive(true);

                    Sprite cardSprite = cardRenderer.sprite;
                    cardButton.image.sprite = cardSprite;

                    //storing the current card index in a variable
                    displayedCards.Add(card);

                    Debug.Log("Assigned sprite: " + cardSprite.name); //log the assigned sprite's name
                }
                else
                {
                    //log an error if SpriteRenderer component is not found
                    Debug.LogError("SpriteRenderer component not found on the card object.");
                }



                dealtCardNumbers.Add(cardGenerate); // add generated card index to the set of dealt card numbers

                Debug.Log("Card Slot Prehab Active: " + cardSlots[i].activeSelf); //log whether the card slot prefab is active
            }

            else
            {
                Debug.LogError("CardGenerate is out of range for the dealtCard array.");
            }
        }
        dealButton.SetActive(false);
    }

    public void AttachClickHandlers(List<GameObject> displayedCards, List<GameObject> playerSelectedCards)
    {
        for (int i = 0; i < cardSlots.Count; i++)
        {
            if (i >= displayedCards.Count) break;

            GameObject displayedCard = displayedCards[i];
            Card cardInfo = displayedCard.GetComponent<Card>();
            Button cardButton = cardSlots[i].GetComponent<Button>();
            cardButton.onClick.RemoveAllListeners();

            if (winningPlayer == 1)
            {
                cardButton.onClick.AddListener(() =>
                {
                    HandleCardClick(displayedCard, cardButton, p1SelectedCards);
                    if (p1SelectedCards.Count > 0)
                    {
                        StartCoroutine(DealCardsWithDelay());
                    }
                });

            }
            else if (winningPlayer == 2)
            {
                cardButton.onClick.AddListener(() =>
                {
                    HandleCardClick(displayedCard, cardButton, p2SelectedCards);
                    if (p1SelectedCards.Count > 0)
                    {
                        GameEnding();
                    }
                });
            }
            else if (winningPlayer == 3)
            {
                cardButton.onClick.AddListener(() =>
                {
                    HandleCardClick(displayedCard, cardButton, p3SelectedCards);

                    if (p1SelectedCards.Count > 0)
                    {
                        StartCoroutine(Start2());
                    }

                });
            }
            else if (winningPlayer == 4)
            {
                cardButton.onClick.AddListener(() =>
                {
                    HandleCardClick(displayedCard, cardButton, p4SelectedCards);
                    if (p1SelectedCards.Count > 0)
                    {
                        StartCoroutine(Player4Won());
                        GameEnding();
                    }
                });
            }
        }
        return;
    }

    private bool IsTrumpSuit(CardSuit suit)
    {
        CardSuit trumpSuit = cardManager.trumpSuit;
        return suit == trumpSuit;
    }

    private bool HasCardOfSuit(List<GameObject> playerSelectedCards)
    {
        Card firstCard = playerSelectedCards[0].GetComponent<Card>();
        CardSuit firstSuit = firstCard.cardSuit;
        List<GameObject> suitableCards = displayedCards.FindAll(cardObject => cardObject.GetComponent<Card>().cardSuit == firstSuit);
        if (suitableCards.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void HandleCardClick(GameObject card, Button cardButton, List<GameObject> playerSelectedCards)
    {
        int cardIndex = cardSlots.IndexOf(cardButton.gameObject);
        Debug.Log("cardslot index : " + cardIndex);

        Card cardInfo = card.GetComponent<Card>();
        CardSuit selectedSuit = cardInfo.cardSuit;

        bool isSuitAcceptable;

        if (playerSelectedCards.Count > 0 && HasCardOfSuit(playerSelectedCards))
        {
            Card firstCard = playerSelectedCards[0].GetComponent<Card>();
            CardSuit firstSuit = firstCard.cardSuit;
            isSuitAcceptable = selectedSuit == firstSuit;
        }
        else if (IsTrumpSuit(selectedSuit))
        {
            CardSuit trumpSuit = cardManager.trumpSuit;
            isSuitAcceptable = selectedSuit == trumpSuit;
        }
        else
        {
            isSuitAcceptable = true;
        }

        if (isSuitAcceptable)
        {
            p1SelectedCards.Add(card);

            GameObject selectedCardCopy = Instantiate(card, Vector3.zero, Quaternion.identity);
            selectedCardCopy.transform.SetParent(cardSlotAbove.transform, false);
            Debug.Log("you clicked on: " + card.name);
            Debug.Log("displayed card is: " + displayedCards[cardIndex]);

            selectedCardCopy.SetActive(true);
            ClearCardSlot(cardIndex);
            displayedCards.Remove(card);
        }
        else
        {
            Debug.Log("Wrong suit");
            wrongCard.SetActive(true);
            Invoke("WrongSuit", 1f);
            return;
        }
    }
     
    void WrongSuit()
    {
        wrongCard.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }

    private void DeactivateCard(List<GameObject> playerSelectedCards)
    {
        playerSelectedCards[0].SetActive(false);
        playerSelectedCards.RemoveAt(0);
    }

    public void DeactivateTransform()
    {
        for (int i = 0; i < cardSlotAbove.childCount; i++)
        {
            Transform child = cardSlotAbove.GetChild(i);
            if (child != null)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void ClearCardSlot(int index)
    {
        if (index >= 0 && index < cardSlots.Count)
        {
            GameObject cardSlot = cardSlots[index];
            Destroy(cardSlot);
            cardSlots.RemoveAt(index);
        }
        else
        {
            Debug.LogError("Invalid index for clearing card slot.");
        }
    }

    public void DealToDeck(GameObject[] deck, List<GameObject> playerDeck)
    {
        if (deck == null || playerDeck == null)
        {
            Debug.LogError("One of the decks has not been initialised.");
            return;
        }
        for (int i = 0; i < 13; i++)
        {
            do
            {
                cardGenerate = Random.Range(2, dealtCard.Length);
            } while (dealtCardNumbers.Contains(cardGenerate));

            if (cardGenerate < dealtCard.Length)
            {
                dealtCardNumbers.Add(cardGenerate);
                //dealtCard[cardGenerate].SetActive(true);

                playerDeck.Add(deck[cardGenerate]);
            }
            else
            {
                Debug.LogError("CardGenerate is out of range for the playerDeck array.");
            }
        }
    }

    public void GenerateCard(GameObject[] deck, List<GameObject> pDeck, List<GameObject> playerSelectedCards, CardSuit suit)
    {
        List<GameObject> suitableCards = pDeck.FindAll(cardObject => cardObject.GetComponent<Card>().cardSuit == suit);

        if (suitableCards.Count > 0)
        {
            GameObject selectedCard = suitableCards[Random.Range(0, suitableCards.Count)];
            playerSelectedCards.Add(selectedCard);
            selectedCard.SetActive(true);
            pDeck.Remove(selectedCard);
        }
        else if (suitableCards.Count == 0)
        {
            GameObject selectedCard = pDeck[Random.Range(0, pDeck.Count)];
            playerSelectedCards.Add(selectedCard);
            selectedCard.SetActive(true);
            pDeck.Remove(selectedCard);
        }
        else
        {
            Debug.LogError("No cards found in the player's deck for the specified suit.");
        }

    }


    private IEnumerator DealCardsWithDelay()
    {

        nextButton.gameObject.SetActive(true);
        Card suit = p1SelectedCards[0].GetComponent<Card>();
        CardSuit selectedSuit = suit.cardSuit;

        yield return new WaitForSeconds(0.5f);
        GenerateCard(player2Deck, p2deck, p2SelectedCards, selectedSuit);
        yield return new WaitForSeconds(0.5f);
        GenerateCard(player3Deck, p3deck, p3SelectedCards, selectedSuit);
        yield return new WaitForSeconds(0.5f);
        GenerateCard(player4Deck, p4deck, p4SelectedCards, selectedSuit);
        yield return new WaitForSeconds(0.5f);

        GameEnding();
    }


    private IEnumerator Player2Won()
    {
        List<CardSuit> suits = new List<CardSuit> { CardSuit.Clubs, CardSuit.Diamonds, CardSuit.Hearts, CardSuit.Spades };
        System.Random random = new System.Random();
        CardSuit selectedSuit = suits[random.Next(suits.Count)];

        yield return new WaitForSeconds(0.5f);
        GenerateCard(player2Deck, p2deck, p2SelectedCards, selectedSuit);
        yield return new WaitForSeconds(0.5f);
        GenerateCard(player3Deck, p3deck, p3SelectedCards, selectedSuit);
        yield return new WaitForSeconds(0.5f);
        GenerateCard(player4Deck, p4deck, p4SelectedCards, selectedSuit);
        yield return new WaitForSeconds(0.5f);

        nextButton.gameObject.SetActive(false);
    }

    private IEnumerator Player3Won()
    {
        List<CardSuit> suits = new List<CardSuit> { CardSuit.Clubs, CardSuit.Diamonds, CardSuit.Hearts, CardSuit.Spades };
        System.Random random = new System.Random();
        CardSuit selectedSuit = suits[random.Next(suits.Count)];

        yield return new WaitForSeconds(0.5f);
        GenerateCard(player3Deck, p3deck, p3SelectedCards, selectedSuit);
        yield return new WaitForSeconds(0.5f);
        GenerateCard(player4Deck, p4deck, p4SelectedCards, selectedSuit);

        nextButton.gameObject.SetActive(false);
    }

    private IEnumerator Start2()
    {
        Card suit = p3SelectedCards[0].GetComponent<Card>();
        CardSuit selectedSuit = suit.cardSuit;

        nextButton.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        GenerateCard(player2Deck, p2deck, p2SelectedCards, selectedSuit);
        yield return new WaitForSeconds(0.5f);

        GameEnding();
    }

    private IEnumerator Start4()
    {
        List<CardSuit> suits = new List<CardSuit> { CardSuit.Clubs, CardSuit.Diamonds, CardSuit.Hearts, CardSuit.Spades };
        System.Random random = new System.Random();
        CardSuit selectedSuit = suits[random.Next(suits.Count)];

        yield return new WaitForSeconds(0.5f);
        GenerateCard(player4Deck, p4deck, p4SelectedCards, selectedSuit);

        nextButton.gameObject.SetActive(false);
    }

    private IEnumerator Player4Won()
    {
        Card suit = p4SelectedCards[0].GetComponent<Card>();
        CardSuit selectedSuit = suit.cardSuit;

        nextButton.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        GenerateCard(player2Deck, p2deck, p2SelectedCards, selectedSuit);
        yield return new WaitForSeconds(0.5f);
        GenerateCard(player3Deck, p3deck, p3SelectedCards, selectedSuit);
        yield return new WaitForSeconds(0.5f);

        GameEnding();
    }

    private void GameEnding()
    {
        if (p1SelectedCards.Count > 0 && p2SelectedCards.Count > 0 &&
            p3SelectedCards.Count > 0 && p4SelectedCards.Count > 0)
        {
            if (winningPlayer == 1)
            {
                Card suit = p1SelectedCards[0].GetComponent<Card>();
                CardSuit selectedSuit = suit.cardSuit;

                winningPlayer = cardManager.CompareCards(p1SelectedCards,
                    p2SelectedCards, p3SelectedCards, p4SelectedCards, selectedSuit);
            }
            else if (winningPlayer == 2)
            {
                Card suit = p2SelectedCards[0].GetComponent<Card>();
                CardSuit selectedSuit = suit.cardSuit;

                winningPlayer = cardManager.CompareCards(p1SelectedCards,
                    p2SelectedCards, p3SelectedCards, p4SelectedCards, selectedSuit);
            }
            else if (winningPlayer == 3)
            {
                Card suit = p3SelectedCards[0].GetComponent<Card>();
                CardSuit selectedSuit = suit.cardSuit;

                winningPlayer = cardManager.CompareCards(p1SelectedCards,
                    p2SelectedCards, p3SelectedCards, p4SelectedCards, selectedSuit);
            }
            else if (winningPlayer == 4)
            {
                Card suit = p4SelectedCards[0].GetComponent<Card>();
                CardSuit selectedSuit = suit.cardSuit;

                winningPlayer = cardManager.CompareCards(p1SelectedCards,
                    p2SelectedCards, p3SelectedCards, p4SelectedCards, selectedSuit);
            }

            Debug.Log("winning player: " + winningPlayer);
            nextButton.gameObject.SetActive(true);
            nextButton.interactable = true;
            buttonText.text = "NEXT";
        }

        int score1 = CardManager.score1;
        int score2 = CardManager.score2;
        int score3 = CardManager.score3;
        int score4 = CardManager.score4;

        if ((score1 + score2 + score3 + score4) == 13)
        {
            if ((score1 + score3) > (score2 + score4))
            {
                foreach (Transform child in cardSlotAbove)
                {
                    child.gameObject.SetActive(false);
                }
                p1won.SetActive(false);
                p2won.SetActive(false);
                p3won.SetActive(false);
                p4won.SetActive(false);
                nextButton.gameObject.SetActive(false);
                p2SelectedCards[0].SetActive(false);
                p3SelectedCards[0].SetActive(false);
                p4SelectedCards[0].SetActive(false);
                winGame.Play();
                winText.SetActive(true);
            }
            else
            {
                foreach (Transform child in cardSlotAbove)
                {
                    child.gameObject.SetActive(false);
                }
                p1won.SetActive(false);
                p2won.SetActive(false);
                p3won.SetActive(false);
                p4won.SetActive(false);
                nextButton.gameObject.SetActive(false);
                p2SelectedCards[0].SetActive(false);
                p3SelectedCards[0].SetActive(false);
                p4SelectedCards[0].SetActive(false);
                loseGame.Play();
                loseText.SetActive(true);
            }
        }
    }
}

