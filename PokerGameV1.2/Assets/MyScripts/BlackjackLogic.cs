using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<GameObject> cardPrefabs;        // put all the cards in here in unity in order
    public Transform tableCentre;
    public int currentCard = 0;                 // keep track of which card to draw next
    private int playerCurrency;
    private int player_score;
    private int dealer_score;
    public List<GameObject> playerCards;        //player deck
    public List<GameObject> dealerCards;        //dealer deck
    private bool playerTurnActive = false;
    private bool playerChoseToHit = false;
    private bool waitingForPlayer = false;
    //put player current money here later
    void Start()
    {
        shuffle();
        Deal();
    }

    void Deal()// set rotation 270=face up, 90=face down
    {
        //player
        Vector3 cardPos = new Vector3(0.1f, 1.3f, 0.9f);
        Quaternion rotation = Quaternion.Euler(270f, 0f, 0f);
        drawCard(cardPos, rotation, true);
        //decrements current card to add the card that was just drawn to the player/dealer decks
        incrementCurrentCard(false);
        playerCards.Add(cardPrefabs[currentCard]);
        incrementCurrentCard(true);
        cardPos = new Vector3(-0.1f, 1.3f, 0.9f);
        rotation = Quaternion.Euler(270, 0, 0);
        drawCard(cardPos, rotation, true);
        incrementCurrentCard(false);
        playerCards.Add(cardPrefabs[currentCard]);
        incrementCurrentCard(true);

        //dealer
        cardPos = new Vector3(0.1f, 1.3f, 0.5f);
        rotation = Quaternion.Euler(270f, 0f, 0f);
        drawCard(cardPos, rotation, false);
        incrementCurrentCard(false);
        dealerCards.Add(cardPrefabs[currentCard]);
        incrementCurrentCard(true);
        cardPos = new Vector3(-0.05f, 1.3f, 0.5f);
        rotation = Quaternion.Euler(90, 0, 0);
        drawCard(cardPos, rotation, false);
        incrementCurrentCard(false);
        dealerCards.Add(cardPrefabs[currentCard]);
        incrementCurrentCard(true);

        player_turn();
    }
    // 1.3f = height to have card resting on table when offset from tableCenter
    // draw a random card (requires deck to have been shuffled to be random)
    void drawCard(Vector3 cardPos, Quaternion rotation, bool isPlayer)
    {
        cardPos = tableCentre.position + cardPos;
        GameObject card = Instantiate(cardPrefabs[currentCard], cardPos, rotation, tableCentre);
        incrementCurrentCard(true);
    }
    void incrementCurrentCard(bool upDown) //false to decrease counter true to increase it
    {
        if (upDown)
        {
            currentCard += 1;
            if (currentCard == 51)
            {
                currentCard = 0;
            }
        }
        else
        {
            currentCard -= 1;
            if (currentCard == -1)
            {
                currentCard = 51;
            }
        }
    }
    private IEnumerator player_turn()
    {
        bool bust = false;
        bool blackjack = false;

        while (playerTurnActive)
        {
            player_score = calc_score(playerCards);

            if (player_score == 21)
            {
                break;
            }

            waitingForPlayer = true;
            yield return new WaitUntil(() => waitingForPlayer == false); // Wait for player to click

            if (playerChoseToHit)
            {
                Vector3 nextCardPos = new Vector3(0.1f + 0.2f * playerCards.Count, 1.3f, 0.9f);  // shift card position
                Quaternion rot = Quaternion.Euler(270, 0, 0);
                drawCard(nextCardPos, rot, true);
                incrementCurrentCard(false);
                playerCards.Add(cardPrefabs[currentCard]);
                incrementCurrentCard(true);

                player_score = calc_score(playerCards);

                if (player_score > 21)
                {
                    bust = true;
                    break;
                }
            }
            else
            {
                break; // Player chose Stand
            }
        }
        /*
        while (player_turn == true);
        if (player_score == 21 && player.Count() == 2)
        {
            blackjack = true;
        }
        dealer[0].toggle_visable();
        dealer_score = calc_score(dealer);
        Display.Refresh(dealer, player, dealer_score, player_score);

        if (bust == false)
        {
            dealer_turn(blackjack);
        }
        else
        {
            Display.lose_screen();
        }*/

    }
    void shuffle()
    {

    }
    private int calc_score(List<GameObject> cards)
    {
        int count = 0;
        foreach (GameObject c in cards)
        {
            CardData data = c.GetComponent<CardData>();
            Quaternion flip = c.transform.rotation;
            if (flip == Quaternion.Euler(270, 0, 0))
            {
                if (data.value == 1)
                {
                    count += 11;
                }
                else if (data.value > 10)
                {
                    count += 10;
                }
                else
                {
                    count += data.value;
                }
            }
        }
        if (count > 21)
        {
            foreach (GameObject c in cards)
            {
                CardData data = c.GetComponent<CardData>();
                if (data.value == 1)
                {
                    count -= 10;
                }
                if (count <= 21) { break; }
            }
        }
        return count;
    }
    public void OnPlayerHit()
    {
        playerChoseToHit = true;
        waitingForPlayer = false;
    }

    public void OnPlayerStand()
    {
        playerChoseToHit = false;
        waitingForPlayer = false;
    }
}