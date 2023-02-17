using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object")]
public class Item : ScriptableObject
{
    [Header("Only Gameplay")]
    public ItemType type;
    public ActionType action;
    public Vector2Int range;

    [Header("Only UI")]
    public bool stackable;
    public int stackSize;

    [Header("Both")]
    public TileBase tile;
    public Sprite image;
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
    Plow,
    Use,
    Plant
}
