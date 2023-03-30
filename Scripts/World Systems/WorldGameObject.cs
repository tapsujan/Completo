using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGameObject : MonoBehaviour //, IDamagable
{
    //lootables
    [Header("LootTable")]
    public string LootTableName;
    public LootTableRoulette[] roulette;
    public float[] Probability;
    [SerializeField] private GameObject lootPrefab;
    //placeholder
    [SerializeField] public static int healthPoints = 5;
    private BuildingSystem buildingSystem = BuildingSystem.instance;

    private void Start()
    {
        Instantiate();
    }
    public void Instantiate()
    {
        /*AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA*/
    }
    /*
    public void Damage(Vector3 force, int damage, bool canBeDamaged, ActionType actionType, float multiplier)
    {

        if (canBeDamaged)
        {
            healthPoints -= damage;
            if (healthPoints <= 0)
            {
                ObjectDestroyed(multiplier);
            }
        }
    }
    */
    public void Damage(int damage,  bool canBeDamaged, ActionType actionType)
    {
        if (canBeDamaged)
        {
            healthPoints -= damage;
            if (healthPoints <= 0)
            {
                //ObjectDestroyed()
                SetProbability();
            }
        }
    }
    /*
    public void ObjectDestroyed(float multiplier)
    {
        //play Destroy animation

        //Coroutine - Animation - Sound - Wiat

        //Drop items

        //Drop: Get RuleTile ItemID, Give ItemID to Loot.cs instance, Loot instance translates to array[ItemID]
        //Do ^ in separate method, repeat for shit, void Stuff(int ItemID, ActionType currentActionType)
        //SpawnNewItemPrefab(tile2.itemID, mouseGridPos, tile2.maxPossibleDrop);
        //tempTilemap.SetTile(highlightedTilePos, null);
        //tilemap2.SetTile(highlightedTilePos, null);

       // SetProbability(multiplier);
    }
    */
    public void SetProbability(/*float multiplier*/)
    {
        int length = roulette.Length;
        for (int i = 0; i < length; i++)
        {
            LootSystem lootSystem = LootSystem.instance;
            float roll = Random.Range((float)0, 1);
            if (roll <= Probability[i])
            {
                LootTableRoulette currentRoulette = roulette[i];
                var (itemToDrop, rouletteQ) = lootSystem.SetProbability(currentRoulette, currentRoulette.item.Length, i);
                SpawnNewDroppedItem(itemToDrop, rouletteQ, transform.position);
            }
        }
    }
    void SpawnNewDroppedItem(Item itemToDrop, int count, Vector3 position)
    {
        if(position == null)
        {
            position = transform.position;
        }
        Debug.Log("SpawnNewDroppedItem() Called");
        //Spawn n amount of times
        for (int i = 0; i < count; i++)
        {
            GameObject newItem = Instantiate(lootPrefab, new Vector3(position.x + Random.Range(-0.5f, 0.5f),
                                                                     position.y + Random.Range(-0.5f, 0.5f),
                                                                     -4), Quaternion.identity);
            Loot currentLoot = newItem.GetComponent<Loot>();
            currentLoot.item = itemToDrop;
            currentLoot.Initialize(itemToDrop);
        }
    }
}
