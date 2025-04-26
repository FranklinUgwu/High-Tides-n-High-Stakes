using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Deck1
{
    private List<Card> cards = new List<Card>();
    private List<Card> shuffled_cards = new List<Card>();
    public Deck1()
    {
        for (int i = 0; i < 4; i++)
        {
            string s = "";
            switch (i)
            {
                case 0:
                    s = "Spade";
                    break;
                case 1:
                    s = "Heart";
                    break;
                case 2:
                    s = "Club";
                    break;
                case 3:
                    s = "Diamond";
                    break;
            }

            for (int j = 1; j < 14; j++)
            {
                cards.Add(new Card(j, s));
            }
        }
    }
    public Card Draw_card()
    {
        Card c = shuffled_cards[0];
        shuffled_cards.RemoveAt(0);
        return c;
    }
    public void shuffle()
    {
        System.Random RNG = new System.Random();
        List<Card> list = cards;
        shuffled_cards.Clear();
        for(int i = 0; i < 52; i++)
        {
            int num = RNG.Next(0,list.Count);
            shuffled_cards.Add(list[num]);
            list.RemoveAt(num);
        }
        
    }
}