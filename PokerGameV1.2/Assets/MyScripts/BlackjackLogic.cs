using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<GameObject> cardPrefabs;        // put all the cards in here in unity in order
    private List<GameObject> shuffled_cards = new List<GameObject>();
    public Transform tableCentre;
    public int currentCard = 0;                 // keep track of which card to draw next
    private int playerCurrency;
    private int player_score;
    private int dealer_score;
    public List<GameObject> playerCards;        //player deck
    public List<GameObject> dealerCards;        //dealer deck
    private bool playerTurnActive = false;
    private bool playerChoseToHit = false;
    private bool waitingForPlayer = true;
    //put player current money here later
    public void StartGame()
    {
        shuffle();
        Deal();
    }

    void Deal()// set rotation 270=face up, 90=face down
    {
        Debug.Log("check");
        //player
        Vector3 cardPos = new Vector3(0.1f, 1.3f, 0.9f);
        Quaternion rotation = Quaternion.Euler(270f, 0f, 0f);
        drawCard(cardPos, rotation, true);
        //decrements current card to add the card that was just drawn to the player/dealer decks
        incrementCurrentCard(false);
        playerCards.Add(shuffled_cards[currentCard]);
        incrementCurrentCard(true);
        cardPos = new Vector3(-0.1f, 1.3f, 0.9f);
        rotation = Quaternion.Euler(270, 0, 0);
        drawCard(cardPos, rotation, true);
        incrementCurrentCard(false);
        playerCards.Add(shuffled_cards[currentCard]);
        incrementCurrentCard(true);

        //dealer
        cardPos = new Vector3(0.1f, 1.3f, 0.5f);
        rotation = Quaternion.Euler(270f, 0f, 0f);
        drawCard(cardPos, rotation, false);
        incrementCurrentCard(false);
        dealerCards.Add(shuffled_cards[currentCard]);
        incrementCurrentCard(true);
        cardPos = new Vector3(-0.05f, 1.3f, 0.5f);
        rotation = Quaternion.Euler(90, 0, 0);
        GameObject hiddenDealerCard = drawCard(cardPos, rotation, false);
        incrementCurrentCard(false);
        dealerCards.Add(shuffled_cards[currentCard]);
        incrementCurrentCard(true);

        StartCoroutine(player_turn(hiddenDealerCard));
    }
    // 1.3f = height to have card resting on table when offset from tableCenter
    // draw a random card (requires deck to have been shuffled to be random)
    GameObject drawCard(Vector3 cardPos, Quaternion rotation, bool isPlayer)
    {
        cardPos = tableCentre.position + cardPos;
        GameObject card = Instantiate(shuffled_cards[currentCard], cardPos, rotation, tableCentre);
        incrementCurrentCard(true);
        return card;
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
    private IEnumerator player_turn(GameObject hiddenDealerCard)
    {
        bool bust = false;
        bool blackjack = false;
        playerTurnActive = true;
        Debug.Log("turn");
        do
        {
            player_score = calc_score(playerCards);
            if (player_score == 21)
            {
                playerTurnActive = false;
                break;//check if bust
            }

            while (waitingForPlayer)
            {
                yield return new WaitForSeconds(0.1f);//wait for player input
            }
            waitingForPlayer = true;
            if (playerChoseToHit)
            {
                Debug.Log("hit");
                Vector3 nextCardPos = new Vector3(0.1f + 0.2f * playerCards.Count, 1.3f, 0.9f);  // shift card position
                Quaternion rot = Quaternion.Euler(270, 0, 0);
                drawCard(nextCardPos, rot, true);
                incrementCurrentCard(false);
                playerCards.Add(cardPrefabs[currentCard]);
                incrementCurrentCard(true);

                player_score = calc_score(playerCards);
                Debug.Log(player_score);
                if (player_score > 21)
                {
                    bust = true;
                    playerTurnActive = false;
                    break;
                }
            }
            else
            {
                playerTurnActive = false;
                break; // Player chose Stand
            }
        }
        while (playerTurnActive);

        if (player_score == 21)
        {
            blackjack = true;
        }
        Destroy(hiddenDealerCard);
        Vector3 cardPos = new Vector3(-0.05f, 1.3f, 0.5f);
        Quaternion rotation = Quaternion.Euler(270, 0, 0);
        drawCard(cardPos, rotation, false);
        incrementCurrentCard(false);
        dealerCards.Add(shuffled_cards[currentCard]);
        incrementCurrentCard(true);
        dealer_score = calc_score(dealerCards);

        if (bust == false)
        {
            dealer_turn(blackjack);
        }
        else
        {
            //Display.lose_screen();
            Debug.Log("lose");
        }
    }
    private void dealer_turn(bool player_blackjack)
    {
        bool dealer_blackjack = false;
        bool bust = false;
        while (dealer_score < 17 && player_blackjack == false)
        {
            System.Threading.Thread.Sleep(500);
            Vector3 cardPos = new Vector3(-0.15f, 1.3f, 0.5f);
            Quaternion rotation = Quaternion.Euler(270, 0, 0);
            drawCard(cardPos, rotation, false);
            incrementCurrentCard(false);
            dealerCards.Add(shuffled_cards[currentCard]);
            incrementCurrentCard(true);
            dealer_score = calc_score(dealerCards);
        }
        if (dealer_score > 21)
        {
            bust = true;
        }
        if (bust == false)
        {
            if (dealerCards.Count() == 2 && dealer_score == 21)
            {
                dealer_blackjack = true;
            }
            if (player_blackjack == true && dealer_blackjack == true)
            {
                //Display.push_screen();
            }
            else if (player_blackjack == true && dealer_blackjack == false)
            {
                //Display.win_screen();
            }
            else if (player_blackjack == false && dealer_blackjack == true)
            {
                //Display.lose_screen();
            }
            else if (player_score > dealer_score)
            {
                //Display.win_screen();
            }
            else if (player_score == dealer_score && player_blackjack == false && dealer_blackjack == false)
            {
                //Display.push_screen();
            }
            else
            {
                //Display.lose_screen();
            }
        }
        else
        {
            //Display.win_screen();
        }

    }
    public void OnPlayerHit()
    {
        Debug.Log("hitbutton");
        playerChoseToHit = true;
        waitingForPlayer = false;
    }

    public void OnPlayerStand()
    {
        playerChoseToHit = false;
        waitingForPlayer = false;
    }
    public void shuffle()
    {
        System.Random RNG = new System.Random();
        List<GameObject> list = new List<GameObject>(cardPrefabs);
        shuffled_cards.Clear();

        for (int i = 0; i < 52; i++)
        {
            int num = RNG.Next(0, list.Count());
            shuffled_cards.Add(list[num]);
            list.RemoveAt(num);
        }
    }
    private int calc_score(List<GameObject> cards)
    {
        int count = 0;
        foreach (GameObject c in cards)
        {
            CardData data = c.GetComponent<CardData>();
            Quaternion flip = c.transform.rotation;
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
}