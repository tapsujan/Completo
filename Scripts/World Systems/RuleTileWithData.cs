using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom Rule Tile")]
public class RuleTileWithData : RuleTile
{
    public Item item;
    public bool buildable;

    public GameObject[] DroppedItemPrefab;
    public GameObject DroppedItemPrefabLowChance;
    //Max n
    public int maxPossibleDrop;
}
