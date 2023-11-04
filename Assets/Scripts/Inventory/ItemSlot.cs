using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, ISelectHandler, ISubmitHandler
{
    private int itemID;
    private string itemName;
    private int quantity;
    private Sprite itemSprite;
    private string itemDescription;

    public bool isFull;
    public TMP_Text quantityText;
    public Image itemImage;

    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionName;
    public TMP_Text itemDescriptionText;

    public void FillSlot(int itemID, string itemName, int quantity, Sprite itemSprite, string itemDescription) {
        this.itemID = itemID;
        this.itemName = itemName;
        this.quantity = quantity;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;
        isFull = true;

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
        itemImage.sprite = itemSprite;
        itemImage.enabled = true;
    }

    private void EmptySlot() {
        this.itemID = 0;
        this.itemName = "";
        this.quantity = 0;
        this.itemSprite = null;
        this.itemDescription = "";
        isFull = false;

        quantityText.enabled = false;
        itemImage.enabled = false;

        itemDescriptionName.text = itemName;
        itemDescriptionText.text = itemDescription;
        itemDescriptionImage.enabled = false;
    }

    public void OnSelect(BaseEventData eventData) {
        if (isFull) {
            itemDescriptionImage.enabled = true;
            itemDescriptionImage.sprite = itemSprite;
            itemDescriptionName.text = itemName;
            itemDescriptionText.text = itemDescription;
            Debug.Log(itemName + ": " + quantity);
        } else {
            itemDescriptionImage.enabled = false;
            itemDescriptionName.text = "";
            itemDescriptionText.text = "";
        }
    }

    public void OnSubmit(BaseEventData eventData) {
        if (isFull) {
            Debug.Log(itemName);
            bool usable = GameManager.instance.UseItem(itemID);
            if (usable) {
                this.quantity -= 1;
                quantityText.text = this.quantity.ToString();
                if (this.quantity <= 0)
                    EmptySlot();
            }
        }
    }

    public string GetItemName() {
        return itemName;
    }

    public int GetItemQuantity() {
        return quantity;
    }

    public void SetItemQuantity(int quantity) {
        this.quantity = quantity;
        quantityText.text = quantity.ToString();
    }

}
