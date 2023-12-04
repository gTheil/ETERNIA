using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSlot : ItemSlot
{
    public int itemPrice;
    public string shopDescription;

    public void FillSlot(int itemID, string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType, int itemPrice) {
        this.itemPrice = itemPrice;
        shopDescription = "\n\nPrice: " + itemPrice + "G\nPress Z to buy.";
        itemDescription += shopDescription;
        base.FillSlot(itemID, itemName, quantity, itemSprite, itemDescription, itemType);
        quantityText.enabled = false;
    }

    public override void EmptySlot() {
        base.EmptySlot();
        this.itemPrice = 0;
    }

    public override void OnSubmit(BaseEventData eventData) {
        if (GameManager.instance.GetPlayer().GetComponent<Player>().gold >= itemPrice) {
            if (GameManager.instance.AddItem(itemID, itemName, quantity, itemSprite, itemDescription, itemType))
                GameManager.instance.GetPlayer().GetComponent<Player>().gold -= itemPrice;
        }
    }
}
