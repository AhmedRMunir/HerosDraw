using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Conditions
{
    public static bool collectingData = false;
    public static int maxLanes = 5;
    public static int maxTotalMana = 10;
    public static int actionsPerLevel = 0;

    public static int wins = 0;
    public static int losses = 0;
    // Levels Completed
    public static int levelsCompleted = 0;
    public static List<CardObject> deck = new List<CardObject>();

    public class info
    {
        public CardObject card;
        public int type;
        public int num;

        public info(CardObject card, int type, int num)
        {
            this.card = card;
            this.type = type;
            this.num = num;
        }
    }

    // a hashtable for the player's current deck
    public static Dictionary<string, info> deck_collection = new Dictionary<string, info>();

    // a hashtable for the player's complete collection of cards
    public static Dictionary<string, info> card_collection = new Dictionary<string, info>();

    // different card types and their corresponding max limit in the deck
    public static int REGULAR = 5;
    public static int RARE = 3;
    public static int CHAMPION = 1;

}
