using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Suit
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}

public class CardData : MonoBehaviour
{
    public int value;  // 2–10, 11=Jack, 12=Queen, 13=King, 1=Ace (optional)

    public Suit suit;
}