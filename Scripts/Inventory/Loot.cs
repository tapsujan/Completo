using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private float moveSpeed = 8;
    [SerializeField] public Item item;

    public void Initialize(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.image;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bool canAdd = InventoryManager.instance.AddItem(item);
            if (canAdd)
            {
                StartCoroutine(MoveAndCollect(collision.transform));
            }
        }
    }

    private IEnumerator MoveAndCollect(Transform target)
    {
        Destroy(GetComponent<CircleCollider2D>());

        while (transform.position != target.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return 0;
        }

        Destroy(gameObject);
    }
}
