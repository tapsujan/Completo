using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create New LootTableRoulette")]
public class LootTableRoulette : ScriptableObject
{
    public Item[] item;
    public float[] probability;
    public int[] minDrop, maxDrop;
    public bool multiplied;
}
