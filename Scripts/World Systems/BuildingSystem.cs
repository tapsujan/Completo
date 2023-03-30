using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] private TileBase hT, hTBuild, hTDig, hTMine, hTChop, hTPlow, hTUse, hTNotBuild;
    [SerializeField] private Tilemap tilemap1;
    [SerializeField] private Tilemap tilemap2;
    [SerializeField] private Tilemap tempTilemap;

    [SerializeField] private Camera mainCamera;

    public int multiplier = 1;

    private Vector3Int playerPos;
    private Vector3Int highlightedTilePos;
    private bool highlighted;
    Vector3Int mouseGridPos;


 
    public static BuildingSystem instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
    }
    private void Update()
    {
        playerPos = tilemap1.WorldToCell(transform.position);
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
        RuleTileWithData tile1 = tilemap1.GetTile<RuleTileWithData>(mouseGridPos);
        RuleTileWithData tile2 = tilemap2.GetTile<RuleTileWithData>(mouseGridPos);
        if (currentItem == null)
        {
            tempTilemap.SetTile(mouseGridPos, null);
        }
        else
        {
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
                ActionType currentActionType = currentItem.action;
                Action(currentActionType);
                if (currentActionType == ActionType.Build)
                {
                    if (selectedHT != hTNotBuild)
                    {
                        tilemap2.SetTile(mouseGridPos, currentItem.tile);
                        InventoryManager.instance.GetSelectedItem(true);
                    }
                }
                else if (currentActionType == ActionType.Dig)
                {

                }
                else if (currentActionType == ActionType.Mine)
                {

                }
                else if (currentActionType == ActionType.Chop)
                {
                    WorldGameObject currentGameObject = tile2.m_DefaultGameObject.GetComponent<WorldGameObject>();
                    currentGameObject.Damage(1, true, ActionType.Chop);
                    //currentGameObject.Damage(new Vector3(0, 0, 0), 1, true, ActionType.Chop, multiplier);
                }
                else if (currentActionType == ActionType.Till)
                {

                }
                else if (currentActionType == ActionType.Use)
                {

                }
                else if (currentActionType == ActionType.Plant)
                {

                }
            }
        }
    }
    public void Action(ActionType actionType)
    {

    }
    //Get mouse position & mouse grid position cell on tilemap
    private Vector3Int GetMouseOnGridPos()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mouseCellPos = tilemap1.WorldToCell(mousePos);
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
        if (tilemap1RuleTile != null)
        {
            tile1AT = tilemap1RuleTile.item.action;
        }
        if (tilemap2RuleTile != null)
        {
            tile2AT = tilemap2RuleTile.item.action;
        }

        //Highlight Map Grid Table, page 7 (!)
        if (currentItemAT == ActionType.Build)
        {
            if (tilemap2RuleTile != null)
            {
                return hTNotBuild;
            }
            else if (tile1AT == ActionType.Build || tile1AT == ActionType.Dig || tile1AT == ActionType.Till || tile1AT == ActionType.Plant)
            {
                return hTBuild;
            }
            return hTNotBuild;
        }
        else if (currentItemAT == ActionType.Dig)
        {
            if (tile1AT == ActionType.Build || tile1AT == ActionType.Dig || tile1AT == ActionType.Till || tile1AT == ActionType.Plant)
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
        else if (currentItemAT == ActionType.Till)
        {
            if (tile2AT == ActionType.Dig)
            {
                return hTPlow;
            }
            if (tile2AT == ActionType.Till)
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
}