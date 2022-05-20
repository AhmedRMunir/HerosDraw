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

}
