using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Item item;
    [Header("UI")]
    public Image image;
    public TextMeshProUGUI countText;

    // Instanciate new item
    public void InitializeItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }
    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool activeText = count != 1;
        countText.gameObject.SetActive(activeText);
    }
    // Drag start click
    public void OnBeginDrag(PointerEventData eventdata)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }
    // During drag
    public void OnDrag(PointerEventData eventdata)
    {
        transform.position = Input.mousePosition;

    }
    // Stop drag
    public void OnEndDrag(PointerEventData eventdata)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}
