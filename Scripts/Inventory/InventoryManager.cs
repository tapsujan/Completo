using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    private int stackSize = 100;

    int selectedSlot;
    private void Awake()
    {
        instance = this;
        ChangeSelectedSlot(0);
    }
    private void Update()
    {
        //Get Selected Slot
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 10)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }
    //change Selected Slot
    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot != newValue)
        {
            inventorySlots[selectedSlot].DeSelect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        
    }
    public bool AddItem(Item item)
    {
        // check stackability
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (
                itemInSlot != null 
                && /*check if item is same*/ itemInSlot.item == item 
                && itemInSlot.count < stackSize 
                && item.stackable == true
                )
            {   
                //add item
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        // find empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }
    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGameObject = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGameObject.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }
    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if (use == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        }
        return null;
    }

}
