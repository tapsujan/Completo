using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSystem : MonoBehaviour
{
    //loot
    //Item to drop list (ItemArray list.txt)
    public Item[] rouletteItem;
    public float[] rouletteProbability;

    private float multiplier;
    public static LootSystem instance;

    private void Awake()
    {
        instance = this;
    }
    //Wena tuplascal :)
    public (Item, int) SetProbability(LootTableRoulette rouletteBuffer, int itemLength, int i)
    {
        //Import
        rouletteItem = rouletteBuffer.item;
        rouletteProbability = rouletteBuffer.probability;
        float generalProbability = 0;
        int rouletteQ = Random.Range(rouletteBuffer.minDrop[i], rouletteBuffer.maxDrop[i]);
        for (int j = 0; j < itemLength; j++)
        {
            //Get general probability
            Item item = rouletteItem[j];
            //rouletteProbability[j] *= item.rarityMultiplier * multiplier;
            generalProbability += rouletteProbability[j];
        }
        for (int j = 0; j < itemLength; j++)
        {
            //Normalize
            rouletteProbability[j] /= generalProbability;
        }
        if (rouletteBuffer != null)
        {
            //roll for item in roulette
            Item itemToDrop = Roll(rouletteBuffer);
            return (itemToDrop, rouletteQ);
        }
        return (null, 0);
    }
    public Item Roll(LootTableRoulette roulette)
    {
        //SetProbability)

        //Roll()
        //delete tree
        float rouletteRoll = Random.Range((float)0, 1);
        float r = 1;
        for (int i = roulette.item.Length - 1; i >= 0; i--)
        {
            //Index out of array (!)
            r -= roulette.probability[i];
            Debug.Log("r = " + r);
            if (rouletteRoll >= r)
            {
                return roulette.item[i];
            }
        }
        return null;
    }
}
