using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    [TextArea]
    public string itemDescription;
    public int itemQuantity;

    private Sprite itemSprite;

    void Start() {
        itemSprite = GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            GameManager.instance.AddItem(itemName, itemQuantity, itemSprite, itemDescription);
            Destroy(gameObject);
        }
    }
}
