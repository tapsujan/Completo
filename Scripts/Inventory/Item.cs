using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Create New Item")]
public class Item : ScriptableObject
{
    [Header("Only Gameplay")]
    public ItemType type;
    public ActionType action;
    public Vector2Int range;
    public int rarity;
    public int rarityMultiplier;

    [Header("Only UI")]
    public bool stackable;
    public int stackSize;

    [Header("Both")]
    public Sprite image;

    [Header("Consumables Only")]
    public Effect effect;
    public int magnitude;

    [Header("Building Only")]
    public TileBase tile;
}

public enum ItemType
{
    Building,
    Tool,
    Consumable
}

public enum ActionType
{
    Null,
    Build,
    Dig,
    Mine,
    Chop,
    Till,
    Use,
    Plant
}

public enum Effect
{
    Buff,
    Healing,
    Poison,
}