using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Card", menuName = "Card")]
public class CardObject : ScriptableObject
{
    public string cardName;
    public Sprite art;
    public string description;
    public int cost;
    public int attack;
    public int health;
    public string faction;
    public string cardType;
    //public GameObject cardAbility; // We'll probably have to write prefabs with the card ability and instantiate them on activation

    public string cardAbility;
    public List<int> abilityParams;
    // Other potential stats

    public bool hasActiveAbility;
    public bool hasPassiveAbility;
    public bool hasBattleAbility;
    public bool hasDestoryAbility;
    public List<int> activeAbilityCost;
    
}
