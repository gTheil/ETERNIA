using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemID;
    public string itemName;
    [TextArea]
    public string itemDescription;
    public int itemQuantity;
    public ItemType itemType;
    public int itemPrice;

    public Sprite itemSprite;

    void Start() {
        itemSprite = GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            GameManager.instance.AddItem(itemID, itemName, itemQuantity, itemSprite, itemDescription, itemType);
            Destroy(gameObject);
        }
    }
}
