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
    private int playerCurrency;                 //put player current money here later
    private int player_score;
    private int dealer_score;
    public List<GameObject> playerCards;       //player deck
    public List<GameObject> dealerCards;       //dealer deck
    public List<GameObject> createdCards;      //all cards that were instantiated
    private bool playerTurnActive = false;
    private bool playerChoseToHit = false;
    private bool waitingForPlayer = true;
    public GameObject hitButton;
    public GameObject standButton;
    public GameObject exitButton;
    public GameObject winner;
    public GameObject loser;
    public GameObject push;
    public Camera BlackJackCam;
    public Camera playerCam;

    public void Start()
    {
        winner.SetActive(false);
        loser.SetActive(false);
        push.SetActive(false);
        BlackJackCam.enabled = false;
        playerCam.enabled = true;
        hitButton.SetActive(false);
        standButton.SetActive(false);
        exitButton.SetActive(false);
    }
    public void StartGame()
    {
        BlackJackCam.enabled = true;
        playerCam.enabled = false;
        hitButton.SetActive(true);
        standButton.SetActive(true);
        shuffle();
        Deal();
    }

    void Deal()// set rotation 270=face up, 90=face down
    {
        Debug.Log("check");
        //player
        Vector3 cardPos = new Vector3(-0.06f, 1, -0.3f);
        Quaternion rotation = Quaternion.Euler(270f, 0f, 0f);
        createdCards.Add(drawCard(cardPos, rotation, true));
        //decrements current card to add the card that was just drawn to the player/dealer decks
        incrementCurrentCard(false);
        playerCards.Add(shuffled_cards[currentCard]);
        incrementCurrentCard(true);
        cardPos = new Vector3(0, 1, -0.3f);
        rotation = Quaternion.Euler(270, 0, 0);
        createdCards.Add(drawCard(cardPos, rotation, true));
        incrementCurrentCard(false);
        playerCards.Add(shuffled_cards[currentCard]);
        incrementCurrentCard(true);

        //dealer
        cardPos = new Vector3(-0.06f, 1, -0);
        rotation = Quaternion.Euler(270f, 0f, 0f);
        createdCards.Add(drawCard(cardPos, rotation, false));
        incrementCurrentCard(false);
        dealerCards.Add(shuffled_cards[currentCard]);
        incrementCurrentCard(true);
        cardPos = new Vector3(0, 1, -0);
        rotation = Quaternion.Euler(90, 0, 0);
        createdCards.Add(drawCard(cardPos, rotation, true));

        StartCoroutine(player_turn());
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
    private IEnumerator player_turn()
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
                Vector3 nextCardPos = new Vector3(-0.06f + 0.06f * playerCards.Count, 1f, -0.3f);  // shift card position
                Quaternion rot = Quaternion.Euler(270, 0, 0);
                createdCards.Add(drawCard(nextCardPos, rot, true));
                incrementCurrentCard(false);
                playerCards.Add(shuffled_cards[currentCard]);
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
        if (bust == false)
        {
            Destroy(createdCards[3]);
            Vector3 cardPos = new Vector3(0f, 1f, -0);
            Quaternion rotation = Quaternion.Euler(270, 0, 0);
            createdCards.Add(drawCard(cardPos, rotation, false));
            incrementCurrentCard(false);
            dealerCards.Add(shuffled_cards[currentCard]);
            incrementCurrentCard(true);
            dealer_score = calc_score(dealerCards);
            StartCoroutine(dealer_turn(blackjack));
        }
        else
        {
            loser.SetActive(true);
            Debug.Log("bust");
            exitButton.SetActive(true);
            hitButton.SetActive(false);
            standButton.SetActive(false);
            playerCards.Clear();
            dealerCards.Clear();
            while (waitingForPlayer)
            {
                yield return new WaitForSeconds(0.1f);//wait for player input
            }
            waitingForPlayer = true;
            BlackJackCam.enabled = false;
            playerCam.enabled = true;
            for (int i = 0; i < createdCards.Count; i++)
            {
                Destroy(createdCards[i]);
            }
            createdCards.Clear();
            loser.SetActive(false);
            exitButton.SetActive(false);
        }
    }
    private IEnumerator dealer_turn(bool player_blackjack)
    {
        bool dealer_blackjack = false;
        bool bust = false;
        while (dealer_score < 17 && player_blackjack == false)
        {
            System.Threading.Thread.Sleep(500);
            Vector3 cardPos = new Vector3(-0.06f + 0.06f * dealerCards.Count, 1f, -0);
            Quaternion rotation = Quaternion.Euler(270, 0, 0);
            createdCards.Add(drawCard(cardPos, rotation, false));
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
                push.SetActive(true);
            }
            else if (player_blackjack == true && dealer_blackjack == false)
            {
                winner.SetActive(true);
            }
            else if (player_blackjack == false && dealer_blackjack == true)
            {
                loser.SetActive(true);
            }
            else if (player_score > dealer_score)
            {
                winner.SetActive(true);
            }
            else if (player_score == dealer_score && player_blackjack == false && dealer_blackjack == false)
            {
                push.SetActive(true);
            }
            else
            {
                loser.SetActive(true);
            }
        }
        else
        {
            winner.SetActive(true);
        }
        playerCards.Clear();
        dealerCards.Clear();
        exitButton.SetActive(true);
        hitButton.SetActive(false);
        standButton.SetActive(false);
        while (waitingForPlayer)
        {
            yield return new WaitForSeconds(0.1f);//wait for player input
        }
        waitingForPlayer = true;
        for (int i = 0; i < createdCards.Count; i++)
        {
            Destroy(createdCards[i]);
        }
        BlackJackCam.enabled = false;
        playerCam.enabled = true;
        createdCards.Clear();
        exitButton.SetActive(false);
        winner.SetActive(false);
        loser.SetActive(false);
        push.SetActive(false);
    }
    public void OnPlayerExit()
    {
        waitingForPlayer = false;
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
