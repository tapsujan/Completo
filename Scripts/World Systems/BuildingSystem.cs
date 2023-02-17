using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] private TileBase hT, hTBuild, hTDig, hTMine, hTChop, hTPlow, hTUse, hTNotBuild;
    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private Tilemap secondaryTilemap;
    [SerializeField] private Tilemap tempTilemap;

    [SerializeField] private GameObject lootPrefab;

    //lootables
    private GameObject[] DroppedItemPrefab;
    private GameObject DroppedItemPrefabLowChance;
    private int maxPossibleDrop;

    [SerializeField] private Camera mainCamera;

    private Vector3Int playerPos;
    private Vector3Int highlightedTilePos;
    private bool highlighted;
    private bool action;
    Vector3Int mouseGridPos;
    private void Start()
    {
    }
    private void Update()
    {
        playerPos = mainTilemap.WorldToCell(transform.position);
        Item item = InventoryManager.instance.GetSelectedItem(false);
        if (item != null)
        {
            HighlightTile(item);
        }
        else
        {
            tempTilemap.SetTile(mouseGridPos, null);
        }
    }
    private void HighlightTile(Item currentItem)
    {
        mouseGridPos = GetMouseOnGridPos();
        if (currentItem == null)
        {
            tempTilemap.SetTile(mouseGridPos, null);
        }
        else
        {
            RuleTileWithData tile1 = mainTilemap.GetTile<RuleTileWithData>(mouseGridPos);
            RuleTileWithData tile2 = secondaryTilemap.GetTile<RuleTileWithData>(mouseGridPos);
            TileBase selectedHT = GetSelectedHT(tile1, tile2, currentItem);
            if (highlightedTilePos != mouseGridPos)
            {
                tempTilemap.SetTile(highlightedTilePos, null);
                if (InRange(playerPos, mouseGridPos, (Vector3Int)currentItem.range))
                {
                    //use method to get supposed tile
                    tempTilemap.SetTile(mouseGridPos, selectedHT);
                    highlightedTilePos = mouseGridPos;
                }
            }
            if (Input.GetMouseButtonDown(0) && selectedHT != null)
            {
                ActionType currentItemActionType = currentItem.action;
                if (currentItemActionType == ActionType.Build)
                {
                    if (selectedHT != hTNotBuild)
                    {
                        secondaryTilemap.SetTile(mouseGridPos, currentItem.tile);
                        InventoryManager.instance.GetSelectedItem(true);
                    }
                }
                else if (currentItemActionType == ActionType.Dig)
                {

                }
                else if (currentItemActionType == ActionType.Mine)
                {

                }
                else if (currentItemActionType == ActionType.Chop)
                {
                    //play animation
                    //play sound
                    //play coroutine that destroys thingy
                    DroppedItemPrefab = tile2.DroppedItemPrefab;
                    DroppedItemPrefabLowChance = tile2.DroppedItemPrefabLowChance;
                    maxPossibleDrop = tile2.maxPossibleDrop;
                    SpawnNewItemPrefab((Vector3Int)mouseGridPos);
                    tempTilemap.SetTile(highlightedTilePos, null);
                    secondaryTilemap.SetTile(highlightedTilePos, null);
                }
                else if (currentItemActionType == ActionType.Plow)
                {

                }
                else if (currentItemActionType == ActionType.Use)
                {

                }
                else if (currentItemActionType == ActionType.Plant)
                {

                }
            }
        }
    }
    //Get mouse position & mouse grid position cell on tilemap
    private Vector3Int GetMouseOnGridPos()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mouseCellPos = mainTilemap.WorldToCell(mousePos);
        mouseCellPos.z = 0;

        return mouseCellPos;
    }
    //Get item Range
    private bool InRange(Vector3Int playerPosition, Vector3Int mousePosition, Vector3Int range)
    {
        Vector3Int distance = playerPosition - mousePosition;
        if (Mathf.Abs(distance.x) >= range.x ||
            Mathf.Abs(distance.y) >= range.y)
        {
            return false;
        }

        return true;
    }
    //Check conditions and return Highlight tile
    private TileBase GetSelectedHT(RuleTileWithData tilemap1RuleTile, RuleTileWithData tilemap2RuleTile, Item currentItem)
    {
        ActionType currentItemAT = currentItem.action;
        ActionType tile1AT = ActionType.Null;
        ActionType tile2AT = ActionType.Null;
        if(tilemap1RuleTile != null)
        {
            tile1AT = tilemap1RuleTile.item.action;
        }
        if(tilemap2RuleTile != null)
        {
            tile2AT = tilemap2RuleTile.item.action;
        }

        //Highlight Map Grid Table, page 7
        if (currentItemAT == ActionType.Build)
        {
            if (tilemap2RuleTile != null)
            {
                return hTNotBuild;
            }
            else if (tile1AT == ActionType.Build || tile1AT == ActionType.Dig || tile1AT == ActionType.Plow || tile1AT == ActionType.Plant)
            {
                return hTBuild;
            }
            return hTNotBuild;
        }
        else if (currentItemAT == ActionType.Dig)
        {
            if (tile1AT == ActionType.Build || tile1AT == ActionType.Dig || tile1AT == ActionType.Plow || tile1AT == ActionType.Plant)
            {
                return hTDig;
            }
            return null;
        }
        else if (currentItemAT == ActionType.Mine)
        {
            if (tile2AT == ActionType.Mine)
            {
                return hTMine;
            }
            else if (tile2AT == ActionType.Chop)
            {
                return hTChop;
            }
            else if (tile2AT == ActionType.Use)
            {
                return hTNotBuild;
            }
            else if (tile1AT == ActionType.Plant)
            {
                return hTDig;
            }
            return null;
        }
        else if (currentItemAT == ActionType.Chop)
        {
            if (tile2AT == ActionType.Chop)
            {
                return hTChop;
            }
            else if (tile1AT == ActionType.Plant)
            {
                return hTDig;
            }
            return null;
        }
        else if (currentItemAT == ActionType.Plow)
        {
            if (tile2AT == ActionType.Dig)
            {
                return hTPlow;
            }
            if (tile2AT == ActionType.Plow)
            {
                return hTPlow;
            }
            else if (tile1AT == ActionType.Plant)
            {
                return hTDig;
            }
            return null;
        }
        else if (currentItemAT == ActionType.Use)
        {
            if (tile2AT == ActionType.Use)
            {
                return hTUse;
            }
            return null;
        }
        return null;
    }
    public void SpawnNewItemPrefab(Vector3Int position)
    {
        if (DroppedItemPrefab.Length == 1)
        {
            SpawnNewDroppedItem(DroppedItemPrefab[0], /*n = random between 1 and Max n*/ (int)Random.Range(1, maxPossibleDrop), position);
        }
        else if (DroppedItemPrefab.Length == 0)
        {
            Debug.LogError("Lootable Not Set On: " + transform);
        }
        else
        {
            SpawnNewDroppedItem(DroppedItemPrefab[/*Random item from list*/(int)Random.Range(0, DroppedItemPrefab.Length)], 1, position);
        }
    }
    private void SpawnNewDroppedItem(GameObject prefab, int count, Vector3Int position)
    {
        //Spawn n amount of times
        for (int i = 0; i < count; i++)
        {
            Instantiate(prefab, new Vector3(position.x, position.y, -4), Quaternion.identity);
        }
    }
}