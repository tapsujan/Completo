using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Sprite selectedSprite, notSelectedSprite;

    private void Awake()
    {
        DeSelect();
    }
    public void Select()
    {
        image.sprite = selectedSprite;
    }
    public void DeSelect()
    {
        image.sprite = notSelectedSprite;
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        InventoryItem draggableItem = dropped.GetComponent<InventoryItem>();
        if (transform.childCount == 0)
        {
            draggableItem.parentAfterDrag = transform;
        }
        else if (transform.childCount != 0)
        {
            Transform oldParent = draggableItem.parentAfterDrag;
            Transform itemInSlot = transform.GetChild(0);
            InventoryItem staticItem = itemInSlot.gameObject.GetComponent<InventoryItem>();

            if (itemInSlot != dropped)
            {
                staticItem.parentAfterDrag = oldParent;
                itemInSlot.SetParent(oldParent);
                draggableItem.parentAfterDrag = transform;

            }
        }
    }
}
