using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Linq;
using TMPro;
public class CardGameUI : MonoBehaviour
{
    public List<GameObject> list_of_prefabs = new List<GameObject>();
    public GameObject[] prefabList;
    private Deck1 _deck = new Deck1();
    public List<Card> dealer = new List<Card>();
    public List<Card> player = new List<Card>();
    public List<Card> river = new List<Card>();
    private Tuple<int, int> player_pos;
    private Tuple<int, int> dealer_pos;
    private Tuple<int, int> river_pos;
    public Button betButton;
    public int wallet;
    private int current_bet;
    public TextMeshProUGUI show_bet;
    public TextMeshProUGUI win_loss;
    public GameObject win_loss_obj;
    public int player_turn;
    public Slider raise_slide;
    public int bet_val;
    public bool raise_on;
    public GameObject full_slide;
    public int raise_hold;
    public TextMeshProUGUI raise_display;
    public Button raise_button;
    //public Button foldButton;
    public void from_start()
    {
        raise_hold = 0;
        full_slide.SetActive(false);
        raise_on = false;
        bet_val = 10;
        win_loss_obj.SetActive(false);
        wallet = PlayerPrefs.GetInt("Shells");
        Debug.Log("Wallet is" + wallet.ToString());
        current_bet = 0;
        raise_slide.maxValue = wallet;
        betButton.onClick.AddListener(betClicked);
        raise_button.onClick.AddListener(Raise_Clicked);
        betButton.interactable = false;
        _deck = new Deck1();
        _deck.shuffle();
        player = new List<Card>();
        dealer = new List<Card>();
        river = new List<Card>();
        Debug.Log("WORKING");
        player.Add(_deck.Draw_card());
        UpdateCardDisplay(player[0].get_value(), player[0].get_suit(), new Vector3(-0.11f,0.84f,0.21f), Quaternion.Euler(270,90,90));
        player.Add(_deck.Draw_card());
        StartCoroutine(Countdown1());
        dealer.Add(_deck.Draw_card());
        StartCoroutine(Countdown2());
        dealer.Add(_deck.Draw_card());
        StartCoroutine(Countdown3());
        player_turn = 0;
    }

    void betClicked()
    {
        Debug.Log("Button Clicked");
        if(player_turn == 0)
        {
            Debug.Log(wallet.ToString());
            if (wallet > 10)
            {   
                current_bet = current_bet + bet_val;
                show_bet.text = "Current Pot: \n" + (current_bet* 2).ToString();
                wallet = wallet - bet_val;
                PlayerPrefs.SetInt("Shells", wallet);
                PlayerPrefs.Save();
                Debug.Log("Money is " +PlayerPrefs.GetInt("Shells").ToString());
                Debug.Log("0 turn");
                raise_slide.maxValue = wallet;
                betButton.interactable = false;
                river.Add(_deck.Draw_card());
                StartCoroutine(Countdown4());
                river.Add(_deck.Draw_card());
                StartCoroutine(Countdown5());
                river.Add(_deck.Draw_card());
                StartCoroutine(Countdown6());
                player_turn ++;
                bet_val = 10;
            }
            else
            {
                Debug.Log("Not enough money to play");
            }
        }
        else if(player_turn == 1)
        {
            if (wallet >= bet_val)
            {
                current_bet = current_bet + bet_val;
                show_bet.text = "Current Pot: \n" + (current_bet* 2).ToString();
                wallet = wallet - bet_val;
                PlayerPrefs.SetInt("Shells", wallet);
                PlayerPrefs.Save();
                Debug.Log("Money is " +PlayerPrefs.GetInt("Shells").ToString());
                raise_slide.maxValue = wallet;
                Debug.Log("1 turn");
                betButton.interactable = false;
                river.Add(_deck.Draw_card());
                StartCoroutine(Countdown7());
                player_turn ++;
                bet_val = 10;
            }
            else if (wallet < 10)
            {
                bet_val = wallet;
                current_bet = current_bet + bet_val;
                show_bet.text = "Current Pot: \n" + (current_bet* 2).ToString();
                wallet = wallet - bet_val;
                PlayerPrefs.SetInt("Shells", wallet);
                PlayerPrefs.Save();
                Debug.Log("Money is " +PlayerPrefs.GetInt("Shells").ToString());
                raise_slide.maxValue = wallet;
                Debug.Log("1 turn");
                betButton.interactable = false;
                river.Add(_deck.Draw_card());
                StartCoroutine(Countdown7());
                player_turn ++;
            }
            else if (wallet == 0)
            {
                Debug.Log("1 turn");
                betButton.interactable = false;
                river.Add(_deck.Draw_card());
                StartCoroutine(Countdown7());
                player_turn ++;
            }
        }
        else if(player_turn == 2)
        {
            if (wallet != 0)
            {
                current_bet = current_bet + bet_val;
                show_bet.text = "Current Pot: \n" + (current_bet * 2).ToString();
                wallet = wallet - bet_val;
                PlayerPrefs.SetInt("Shells", wallet);
                PlayerPrefs.Save();
                Debug.Log("Money is " +PlayerPrefs.GetInt("Shells").ToString());
                raise_slide.maxValue = wallet;
                Debug.Log("3 turn");
                betButton.interactable = false;
                river.Add(_deck.Draw_card());
                StartCoroutine(Countdown8());
                player_turn ++;
                bet_val = 10;
            }
            else if(wallet < 10)
            {
                bet_val = wallet;
                current_bet = current_bet + bet_val;
                show_bet.text = "Current Pot: \n" + (current_bet * 2).ToString();
                wallet = wallet - bet_val;
                PlayerPrefs.SetInt("Shells", wallet);
                PlayerPrefs.Save();
                Debug.Log("Money is " +PlayerPrefs.GetInt("Shells").ToString());
                raise_slide.maxValue = wallet;
                Debug.Log("3 turn");
                betButton.interactable = false;
                river.Add(_deck.Draw_card());
                StartCoroutine(Countdown8());
                player_turn ++;
            }
            else if(wallet == 0)
            {
                betButton.interactable = false;
                river.Add(_deck.Draw_card());
                StartCoroutine(Countdown8());
                player_turn ++;
            }
        }
        else
        {
            if (wallet != 0)
            {
                current_bet = current_bet + bet_val;
                show_bet.text = "Current Pot: \n" + (current_bet * 2).ToString();
                wallet = wallet - bet_val;
                PlayerPrefs.SetInt("Shells", wallet);
                PlayerPrefs.Save();
                Debug.Log("Money is " +PlayerPrefs.GetInt("Shells").ToString());
                raise_slide.maxValue = wallet;
            }
            betButton.interactable = false;
            Tuple<string ,int,int,int,int> player_vals = calc_win(player);
            dealer_turn(player_vals);
        }
    }

    public void update_raise()
    {
        raise_hold = (int)raise_slide.value;
        raise_display.text = raise_hold.ToString();
    }
    void Raise_Clicked()
    {
        if (raise_on == false)
        {
            full_slide.SetActive(true);
            raise_on = true;
        }
        else
        {
            bet_val = raise_hold;
            full_slide.SetActive(false);
            raise_on = false;
        }
    }
    
    private IEnumerator Countdown1()
    {
        yield return new WaitForSeconds(.5f);
        UpdateCardDisplay(player[1].get_value(), player[1].get_suit(), new Vector3(-0.05f,0.855f,0.18f), Quaternion.Euler(270,90,90));
    }
    
    private IEnumerator Countdown2()
    {
        yield return new WaitForSeconds(1);
        UpdateCardDisplay(dealer[0].get_value(), dealer[0].get_suit(), new Vector3(-0.11f,0.84f,0.83f), Quaternion.Euler(90,90,90));
    }

    private IEnumerator Countdown3()
    {
        yield return new WaitForSeconds(1.5f);
        UpdateCardDisplay(dealer[1].get_value(), dealer[1].get_suit(), new Vector3(-0.05f,0.855f,0.8f), Quaternion.Euler(90,90,90));
        betButton.interactable = true;
    }

    private IEnumerator Countdown4()
    {
        yield return new WaitForSeconds(0.5f);
        UpdateCardDisplay(river[0].get_value(), river[0].get_suit(), new Vector3(-0.29f,0.855f,0.5f), Quaternion.Euler(270,90,90));
    }
    private IEnumerator Countdown5()
    {
        yield return new WaitForSeconds(1);
        UpdateCardDisplay(river[1].get_value(), river[1].get_suit(), new Vector3(-0.18f,0.855f,0.5f), Quaternion.Euler(270,90,90));
    }
    private IEnumerator Countdown6()
    {
        yield return new WaitForSeconds(1.5f);
        UpdateCardDisplay(river[2].get_value(), river[2].get_suit(), new Vector3(-0.07f,0.855f,0.5f), Quaternion.Euler(270,90,90));
        betButton.interactable = true;
    }
    private IEnumerator Countdown7()
    {
        yield return new WaitForSeconds(0.5f);
        UpdateCardDisplay(river[3].get_value(), river[3].get_suit(), new Vector3(0.04f,0.855f,0.5f), Quaternion.Euler(270,90,90));
        betButton.interactable = true;
    }
    private IEnumerator Countdown8()
    {
        yield return new WaitForSeconds(0.5f);
        UpdateCardDisplay(river[4].get_value(), river[4].get_suit(), new Vector3(0.15f,0.855f,0.5f), Quaternion.Euler(270,90,90));
        betButton.interactable = true;
    }
    void Update()
    {

    }
    // This method will update the visual representation of the cards
    public void UpdateCardDisplay(int cardValue, string cardSuit, Vector3 position, Quaternion qat)
    {
        // Log the received card information
        //Debug.Log($"Received Card: {cardValue} of {cardSuit}");
        string prefabname = "";
        if (cardValue == 1) 
        {
            prefabname = "Card_"+cardSuit+"Ace";
        }
        else if(cardValue == 11) 
        {
            prefabname = "Card_"+cardSuit+"Jack";
        }
        else if(cardValue == 12)
        {
            prefabname = "Card_"+cardSuit+"Queen";
        }
        else if(cardValue == 13)
        {
            prefabname = "Card_"+cardSuit+"King";
        }
        else 
        {
            prefabname = "Card_"+cardSuit+ cardValue.ToString();  
        }
        GameObject fab_hold = null;
        foreach(GameObject pfab in prefabList)
        {
            if (pfab.name == prefabname)
            {
                fab_hold = pfab;
            }
        }
        //if (where == "player")
        //Vector3 position = new Vector3(-0.247f,0.8389155f,0.294f);
        // Instantiate the card at the given position and rotation
        if (fab_hold != null) 
        {
            GameObject newCard = Instantiate(fab_hold, position, qat);
            list_of_prefabs.Add(newCard);
            newCard.transform.localScale = new Vector3(0.75f,0.75f,0.75f);
        }
        // Optionally, you can set additional properties like card value/suit, or attach it to a parent
        // For example, setting the card's name for easier identification
        //newCard.name = $"{cardValue}_{cardSuit}";

        // If you want to set any other UI components, like showing the card's value on a UI Text, you can do it here
    }



    private Tuple<string, int, int, int, int> calc_win(List<Card> given_hand)
    {
        String player_hand_temp1;
        List<Card> hand = given_hand;
        List<int> player_ranks = new List<int>();
        foreach(Card c in hand) 
        {
            if (c.get_value() != 1)
            {
                player_ranks.Add(c.get_value());
            }
            else
            {
                player_ranks.Add(14);
            }
        }
        hand.AddRange(river);
        List<int> values_unchanged = new List<int>();
        List<string> suits = new List<string>();
        List<int> values = new List<int>();
        List<int> count_values = new List<int>();
        int straight_base = -1;
        int high_card = -1;
        int max_repeat_val = -1;
        int second_repeat_val = -1;
        //Console.SetCursorPosition(0,cursor_place);
        foreach(Card c in hand)
        {
            //Console.WriteLine(c.get_value());
            suits.Add(c.get_suit());
            values_unchanged.Add(c.get_value());
            int val_pos = values.IndexOf(c.get_value());
            if (val_pos == -1)
            {
                values.Add(c.get_value());
                count_values.Add(1);
            }
            else
            {
                count_values[val_pos] ++;
            }
        }
        /*
        Console.SetCursorPosition(0,20);
        values.ForEach(Console.WriteLine);
        Console.WriteLine("Break");
        count_values.ForEach(Console.WriteLine);
        */
        List<String> hand_hierarchy = new List<String> {"Royal Flush", "Straight Flush", "Four of a Kind", "Full House", "Flush","Straight","Three of a Kind","Two Pair","One Pair","High Card"};
        List<int> pairs = new List<int>();
        List<int> trips = new List<int>();
        bool four_of = false;
        for(int i = 0; i < count_values.Count; i++)
        {
            int addval = values[i];
            if (count_values[i] == 2) 
            {
                if (addval == 1)
                {
                    addval = 14;
                }
                pairs.Add(addval);
            }
            else if(count_values[i] == 3)
            {
                if (addval == 1)
                {
                    addval = 14;
                }
                trips.Add(addval);
            }
            else if(count_values[i] == 4)
            {
                four_of = true;
            }
        }
        pairs.Sort();
        trips.Sort();

        /*int first_max = count_values.Max();
        count_values.RemoveAt(count_values.IndexOf(first_max));
        int second_max = count_values.Max();*/
        if (four_of)
        {
            Debug.Log("Found 4");
            player_hand_temp1 = "Four of a Kind";
        }
        else if (trips.Count == 2) 
        {
            Debug.Log("Found Full");
            max_repeat_val = trips[1];
            second_repeat_val = trips[0];
            player_hand_temp1 = "Full House";
        }
        else if(trips.Count == 1)
        {
            if (pairs.Count > 0)
            {
                max_repeat_val = trips[0];
                second_repeat_val = pairs[pairs.Count - 1];
                player_hand_temp1 = "Full House";
            }
            else
            {
                player_hand_temp1 = "Three of a Kind";
                max_repeat_val = trips[0];
                if (player_ranks.IndexOf(max_repeat_val) != -1)
                {
                    player_ranks.RemoveAt(player_ranks.IndexOf(max_repeat_val));
                    high_card = player_ranks.Max();
                }
                else {
                    high_card = player_ranks.Max();
                }
            }
        }
        else {
            if(pairs.Count >= 2) 
            {
                player_hand_temp1 = "Two Pair";
                max_repeat_val = pairs[pairs.Count() - 1];
                second_repeat_val = pairs[pairs.Count() - 2];
                if (player_ranks.IndexOf(max_repeat_val) != -1)
                {
                    player_ranks.RemoveAt(player_ranks.IndexOf(max_repeat_val));
                    if (player_ranks.IndexOf(second_repeat_val) != -1) 
                    {
                        high_card = -1;
                        player_ranks.Add(player_ranks.IndexOf(max_repeat_val));
                    }
                    else
                    {
                        high_card = player_ranks.Max();
                        player_ranks.Add(player_ranks.IndexOf(max_repeat_val));
                    }
                }
                else if(player_ranks.IndexOf(second_repeat_val) != -1 && player_ranks[0] != player_ranks[1])
                {
                    player_ranks.RemoveAt(player_ranks.IndexOf(second_repeat_val));
                    high_card = player_ranks.Max();
                    player_ranks.Add(player_ranks.IndexOf(second_repeat_val));
                }
                else
                {
                    high_card = player_ranks.Max();
                }
            }
            else if(pairs.Count == 1)
            {
                player_hand_temp1 ="One Pair";
                max_repeat_val = pairs[pairs.Count() - 1];
                if (player_ranks.IndexOf(max_repeat_val) != -1)
                {
                    player_ranks.RemoveAt(player_ranks.IndexOf(max_repeat_val));
                    high_card = player_ranks.Max();
                    player_ranks.Add(player_ranks.IndexOf(max_repeat_val));
                }
                else
                {
                    high_card = player_ranks.Max();
                }
            }
            else
            {
                high_card = player_ranks.Max();
                player_hand_temp1 = "High Card";
            }
        }
        /*
        else if(first_max == 3)
        {
            if (second_max == 2)
            {
                player_hand_temp1 = "Full House";
            }
            else
            {
                player_hand_temp1 = "Three of a Kind";
            }
        }
        else if(first_max == 2)
        {
            if (second_max == 2)
            {
                player_hand_temp1 = "Two Pair";
            }
            else
            {
                player_hand_temp1 = "Pair";
            }
        }
        else
        {
            player_hand_temp1 = "High Card";
        }
        */
        bool player_straight = false;
        values.Sort();
        //Debug.Log(values[0].ToString() + values[1].ToString() + values[2].ToString()+ values[3].ToString()+ values[4].ToString()+ values[5].ToString()+ values[6].ToString());
        List<int> straight_values = values;
        if (straight_values.IndexOf(1) != -1) 
        {
            straight_values.Add(14);
        }
        int previous = -20;
        int max_chain = 0;
        int chain = 0;
        List<int> hold_list = new List<int>();
        List<int> chain_list = new List<int>();
        foreach(int val in straight_values)
        {
            if (val == previous + 1)
            {
                chain ++;
                Debug.Log("Current Chain = " + chain.ToString());
                hold_list.Add(val);
                if (chain > max_chain) 
                {
                    max_chain = chain;
                    chain_list = hold_list;
                }
            }
            else{
                chain = 1;
                hold_list = new List<int>();
            }
            previous = val;
        }
        Debug.Log("Max chain is " + max_chain.ToString());
        if (max_chain == 5) {
            Debug.Log("Straight Found");
            player_straight = true;
            straight_base = chain_list[0];
        }
        else if (max_chain == 6) {
            player_straight = true;
            straight_base = chain_list[1];
        }
        else if (max_chain == 7) {
            player_straight = true;
            straight_base = chain_list[1];
        }
        List<string> suit_hold = new List<string> {"Spade","Heart","Club","Diamond"};
        List<int> suit_count = new List<int> {0,0,0,0};
        bool player_flush = false;
        String player_hand_temp2 = "";
        foreach(string this_suit in suits)
        {
            int the_index = suit_hold.IndexOf(this_suit);
            suit_count[the_index] ++;
        }
        if (suit_count.Max() >= 5)
        {
            player_flush = true;
        }
        if(player_flush == true && player_straight == true)
        {
            if (chain == 5)
            {
                player_hand_temp2 = straightflush(chain_list, suits, values_unchanged);
            }
            else if (chain == 6)
            {
                List<int> check_chain = chain_list.GetRange(1,5);
                player_hand_temp2 = straightflush(check_chain, suits, values_unchanged);
                if (player_hand_temp2 != "Straight Flush")
                {
                    check_chain = chain_list.GetRange(0,5);
                    player_hand_temp2 = straightflush(check_chain, suits, values_unchanged);
                }
                else
                {
                    straight_base = check_chain[0];
                }
                if (player_hand_temp2 == "Straight Flush")
                {
                    straight_base = check_chain[0];
                }
            }
            else if (chain == 7) 
            {
                List<int> check_chain = chain_list.GetRange(2,5);
                player_hand_temp2 = straightflush(check_chain, suits, values_unchanged);
                if (player_hand_temp2 != "Straight Flush")
                {
                    check_chain = chain_list.GetRange(1,5);
                    player_hand_temp2 = straightflush(check_chain, suits, values_unchanged);
                }
                else
                {
                    straight_base = check_chain[0];
                }
                if (player_hand_temp2 != "Straight Flush")
                {
                    check_chain = chain_list.GetRange(0,5);
                    player_hand_temp2 = straightflush(check_chain, suits, values_unchanged);
                    if (player_hand_temp2 == "Straight Flush")
                    {
                        straight_base = check_chain[0];
                    }
                }
                else 
                {
                    straight_base = check_chain[0];
                }
            }
        }
        else if (player_flush == true)
        {
            player_hand_temp2 = "Flush";
        }
        else if (player_straight == true) 
        {
            if (chain == 7) 
            {
                straight_base = chain_list[2];
            }
            else if (chain == 6) 
            {
                straight_base = chain_list[1];
            }
            else 
            {
                straight_base = chain_list[0];
            }
            player_hand_temp2 = "Straight";
        }
        else
        {
            player_hand_temp2 = "High Card";
        }
        if (player_hand_temp2 == "Straight Flush" && straight_base == 10) {
            player_hand_temp2 = "Royal Flush";
        }
        String main_hand = "";
        bool hand_found = false;
        Debug.Log(player_hand_temp1);
        Debug.Log(player_hand_temp2);
        foreach(String hand_type in hand_hierarchy)
        {
            if ((player_hand_temp1 == hand_type || player_hand_temp2 == hand_type) && hand_found == false)
            {
                main_hand = hand_type;
                hand_found = true;
            }
        }
        //Console.WriteLine(main_hand);
        Tuple<String, int, int, int, int> return_vals = new Tuple<String, int, int, int, int> (main_hand, max_repeat_val, second_repeat_val, straight_base, high_card);

        return return_vals;
    }
    private String straightflush(List<int> chain_list, List<string> suits, List<int> values_unchanged)
    {
        String player_hand_temp2;
        int count_for_suit = 0;
        string suit_check = suits[values_unchanged.IndexOf(chain_list[0])];
        foreach(int straightval in chain_list)
        {
            if (suits[values_unchanged.IndexOf(straightval)] == suit_check)
            {
                count_for_suit ++;
            }
        }
        if (count_for_suit == 5)
        {
            player_hand_temp2 = "Straight Flush";
        }
        else 
        {
            player_hand_temp2 = "Flush";
        }
        return player_hand_temp2;
    }
    private void dealer_turn(Tuple<String, int, int, int, int> player_vals)
    {

        Tuple<String, int, int, int, int> dealer_vals = calc_win(dealer);
        bool player_win = false;
        bool dealer_win = false;
        bool split = false;
        bool found_winner = false;
        List<String> hand_hierarchy = new List<String> {"Royal Flush", "Straight Flush", "Four of a Kind", "Full House", "Flush","Straight","Three of a Kind","Two Pair","One Pair","High Card"};
        foreach(String winning_check in hand_hierarchy)
        {
            if (found_winner == false)
            {
                if(player_vals.Item1 == winning_check && dealer_vals.Item1 == winning_check)
                {
                    found_winner = true;
                    if (winning_check == "Straight Flush")
                    {
                        if (player_vals.Item4 > dealer_vals.Item4)
                        {
                            player_win = true;
                        }
                        else 
                        {
                            dealer_win = true;
                        }
                    }
                    else if (winning_check == "Four of a Kind")
                    {
                        if(player_vals.Item2 > dealer_vals.Item2)
                        {
                            player_win = true;
                        }
                        else if (player_vals.Item2 < dealer_vals.Item2)
                        {
                            dealer_win = true;
                        }
                        else
                        {
                            split = true;
                        }
                    }
                    else if (winning_check == "Full House" || winning_check == "Two Pair")
                    {
                        if(player_vals.Item2 == dealer_vals.Item2)
                        {
                            if (player_vals.Item3 == dealer_vals.Item3)
                            {
                                if(player_vals.Item5 == dealer_vals.Item5)
                                {
                                    split = true;
                                }
                                else if (player_vals.Item5 > dealer_vals.Item5)
                                {
                                    player_win = true;
                                }
                                else
                                {
                                    dealer_win = true;
                                }
                            }
                            else if(player_vals.Item3 > dealer_vals.Item3)
                            {
                                player_win = true;
                            }
                            else
                            {
                                dealer_win = true;
                            }
                        }
                        else if(player_vals.Item2 > dealer_vals.Item2)
                        {
                            player_win = true;
                        }
                        else
                        {
                            dealer_win = true;
                        }
                    }
                    else if (winning_check == "Flush")
                    {
                        split = true;
                    }
                    else if (winning_check == "Straight")
                    {
                        if(player_vals.Item4 == dealer_vals.Item4)
                        {
                            split = true;
                        }
                        else if (player_vals.Item4 > dealer_vals.Item4)
                        {
                            player_win = true;
                        }
                        else
                        {
                            dealer_win = true;
                        }
                    }
                    else if(winning_check == "Three of a Kind" || winning_check == "One Pair")
                    {
                        if (player_vals.Item2 == dealer_vals.Item2)
                        {
                            if (player_vals.Item5 == dealer_vals.Item5)
                            {
                                split = true;
                            }
                            else if (player_vals.Item5 > dealer_vals.Item5)
                            {
                                player_win = true;
                            }
                            else
                            {
                                dealer_win = true;
                            }
                        }
                        else if (player_vals.Item2 > dealer_vals.Item2) 
                        {
                            player_win = true;
                        }
                        else
                        {
                            dealer_win = true;
                        }
                    }
                    else
                    {
                        if (player_vals.Item5 == dealer_vals.Item5)
                        {
                            split = true;
                        }
                        else if (player_vals.Item5 > dealer_vals.Item5)
                        {
                            player_win = true;
                        }
                        else
                        {
                            dealer_win = true;
                        }
                    }
                }
                else if (player_vals.Item1 == winning_check)
                {
                    player_win = true;
                    found_winner = true;
                }
                else if (dealer_vals.Item1 == winning_check)
                {
                    dealer_win = true;
                    found_winner = true;
                }
            }
        }
        list_of_prefabs[2].transform.position += new Vector3(0,0.2f,-0.1f);
        list_of_prefabs[2].transform.Rotate(260,0,0);
        list_of_prefabs[3].transform.Rotate(260,0,0);
        if(player_win)
        {
            win_loss.text = "Player Wins!! \n You won with " + player_vals.Item1;
            win_loss_obj.SetActive(true);
            wallet = wallet + (current_bet * 2);
            PlayerPrefs.SetInt("Shells", wallet);
            Debug.Log("Player Wins!!  The winning hand was" + player_vals.Item1 + "max_val was " + player_vals.Item2 + "second val was " + player_vals.Item3+ "high card was" + player_vals.Item5);
            Debug.Log("Dealer Lost!!  The losing hand was" + dealer_vals.Item1 + "max_val was " + dealer_vals.Item2 + "second val was " + dealer_vals.Item3+ "high card was" + dealer_vals.Item5);
        }
        else if (dealer_win)
        {
            win_loss.text = "Player Loses!! \n You lost to " + dealer_vals.Item1;
            win_loss_obj.SetActive(true);
            Debug.Log("Dealer Wins!!  The winning hand was" + dealer_vals.Item1 + "max_val was " + dealer_vals.Item2 + "second val was " + dealer_vals.Item3+ "high card was" + dealer_vals.Item5);
            Debug.Log("Player Lost!!  The losing hand was" + player_vals.Item1 + "max_val was " + player_vals.Item2 + "second val was " + player_vals.Item3+ "high card was" + player_vals.Item5);
        }
        else
        {
            win_loss.text = "Split Pot!! \n You drew with " + player_vals.Item1;
            win_loss_obj.SetActive(true);
            Debug.Log("Split Pot!!  The Dealers hand was" + dealer_vals.Item1 + "max_val was " + dealer_vals.Item2 + "second val was " + dealer_vals.Item3+ "high card was" + dealer_vals.Item5);
            Debug.Log("Split Pot!!  The Players hand was" + player_vals.Item1 + "max_val was " + player_vals.Item2 + "second val was " + player_vals.Item3+ "high card was" + player_vals.Item5);
        }
        player.Clear();
        dealer.Clear();
        river.Clear();
        StartCoroutine(ready_restart());
        betButton.onClick.RemoveListener(betClicked);
        raise_button.onClick.RemoveListener(Raise_Clicked);
        show_bet.text = "Current Pot: \n 0";
    }

    private IEnumerator ready_restart()
    {
        yield return new WaitForSeconds(3);
        foreach(GameObject dest_card in list_of_prefabs)
        {
            Destroy(dest_card);
        }
        list_of_prefabs.Clear();
        from_start();
    }
}
