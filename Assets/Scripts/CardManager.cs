using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardManager : MonoBehaviour
{
    public TextMeshProUGUI p1score;
    public TextMeshProUGUI p2score;
    public TextMeshProUGUI p3score;
    public TextMeshProUGUI p4score;
    public GameObject p1won;
    public GameObject p2won;
    public GameObject p3won;
    public GameObject p4won;
    public static int score1 = 0;
    public static int score2 = 0;
    public static int score3 = 0;
    public static int score4 = 0;

    public AudioSource winSet;
    public AudioSource loseSet;
    public CardSuit trumpSuit;

    private void Start()
    {
        trumpSuit = (CardSuit)Random.Range(0, 5);
    }

    public int CompareCards(List<GameObject> p1SelectedCards,
        List<GameObject> p2SelectedCards,
        List<GameObject> p3SelectedCards,
        List<GameObject> p4SelectedCards,
        CardSuit selectedSuit)
    {
        if(p1SelectedCards.Count == 0 || p2SelectedCards.Count == 0 ||
            p3SelectedCards.Count == 0 || p4SelectedCards.Count == 0)
        {
            Debug.LogError("Not all players have selected cards.");
            return -1;
        }

        GameObject card1 = RemoveFirstCard(p1SelectedCards);
        GameObject card2 = RemoveFirstCard(p2SelectedCards);
        GameObject card3 = RemoveFirstCard(p3SelectedCards);
        GameObject card4 = RemoveFirstCard(p4SelectedCards);
        Debug.Log("card 1 is: " + card1);
        Debug.Log("card 2 is: " + card2);
        Debug.Log("card 3 is: " + card3);
        Debug.Log("card 4 is: " + card4);

        GameObject winningCard = card1;
        int winningPlayer = 1;
        if (IsCardHigher(card2, winningCard, selectedSuit))
        {
            winningCard = card2;
            winningPlayer = 2;
        }
        if (IsCardHigher(card3, winningCard, selectedSuit))
        {
            winningCard = card3;
            winningPlayer = 3;
        }
        if (IsCardHigher(card4, winningCard, selectedSuit))
        {
            winningCard = card4;
            winningPlayer = 4;
        }
        if (winningCard != null)
        {
            Debug.Log("The winning card is: " + winningCard.name);

            if (winningCard == card1)
            {
                score1++;
                p1won.SetActive(true);
                winSet.Play();
            }
            else if (winningCard == card2)
            {
                score2++;
                p2won.SetActive(true);
                loseSet.Play();
            }
            else if (winningCard == card3)
            {
                score3++;
                p3won.SetActive(true);
                winSet.Play();
            }
            else
            {
                score4++;
                p4won.SetActive(true);
                loseSet.Play();
            }

            p1score.text = "Sets collected: " + score1.ToString();
            p2score.text = "Player 2 Sets: " + score2.ToString();
            p3score.text = "Player 3 Sets: " + score3.ToString();
            p4score.text = "Player 4 Sets: " + score4.ToString();
            return winningPlayer;
        }

        return -1;
        
    }

    private GameObject RemoveFirstCard(List<GameObject> playerSelectedCards)
    {
        GameObject firstCard = playerSelectedCards[0];
        return firstCard;
    }

    private bool IsTrumpSuit(CardSuit suit)
    {
        return suit == trumpSuit;
    }

    private bool IsCardHigher(GameObject card1, GameObject card2, CardSuit selectedSuit)
    {
        Card card1Info = card1.GetComponent<Card>();
        Card card2Info = card2.GetComponent<Card>();

        bool card1Matches = card1Info.cardSuit == selectedSuit;
        bool card2Matches = card2Info.cardSuit == selectedSuit;

        int card1Value = card1Info.cardValue;
        int card2Value = card2Info.cardValue;

        if (IsTrumpSuit(card1Info.cardSuit))
        {
            card1Value += 52;
        }
        if (IsTrumpSuit(card2Info.cardSuit))
        {
            card2Value += 52;
        }
        if (!card1Matches && !IsTrumpSuit(card1Info.cardSuit))
        {
            card1Value = 0;
        }
        if (!card2Matches && !IsTrumpSuit(card2Info.cardSuit))
        {
            card2Value = 0;
        }

        return card1Value > card2Value;
    }
}

