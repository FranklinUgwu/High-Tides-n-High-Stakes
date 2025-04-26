using System;

public class Card
{
    private int value;
    private string suit;
    private bool visable = true;
    public Card(int v,string s)
    {
        value = v;
        suit = s;
    }
    public int get_value()
    {
        return value;
    }
    public string get_suit()
    {
        return suit;
    }
    public void toggle_visable()
    {
        visable = !visable;
    }
    public bool is_visable()
    {
        return visable;
    }
}

