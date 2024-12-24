using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardSuit
{
    Clubs,
    Diamonds,
    Hearts,
    Spades,
    NOTrump
}

public class Card : MonoBehaviour
{
    public int cardValue;
    public CardSuit cardSuit;

    public DealCard dealCard;

}

