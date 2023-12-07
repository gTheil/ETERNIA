using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ShopSlot : ItemSlot
{
    public int itemPrice;

    public TMP_Text itemPriceText;

    public void FillSlot(int itemID, string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType, int itemPrice) {
        this.itemPrice = itemPrice;
        itemPriceText.text = "Price: " + itemPrice + "G\nPress Z to buy.";
        base.FillSlot(itemID, itemName, quantity, itemSprite, itemDescription, itemType);
        quantityText.enabled = false;
        itemPriceText.enabled = false;
    }

    public override void EmptySlot() {
        base.EmptySlot();
        this.itemPrice = 0;
    }

    public override void OnSelect(BaseEventData eventData) {
        if (isFull) {
            itemDescriptionImage.enabled = true;
            itemDescriptionImage.sprite = itemSprite;
            itemDescriptionName.text = itemName;
            itemDescriptionText.text = itemDescription;
            itemPriceText.enabled = true;
            Debug.Log(itemName + ": " + quantity);
        } else {
            itemDescriptionImage.enabled = false;
            itemPriceText.enabled = false;
            itemDescriptionName.text = "";
            itemDescriptionText.text = "";
        }
    }

    public override void OnSubmit(BaseEventData eventData) {
        if (GameManager.instance.GetPlayer().GetComponent<Player>().gold >= itemPrice) {
            if (GameManager.instance.AddItem(itemID, itemName, quantity, itemSprite, itemDescription, itemType))
                GameManager.instance.GetPlayer().GetComponent<Player>().gold -= itemPrice;
        }
    }
}
