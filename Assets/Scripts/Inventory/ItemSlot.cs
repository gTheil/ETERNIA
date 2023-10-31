using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, ISelectHandler, ISubmitHandler
{
    private string itemName;
    private int quantity;
    private Sprite itemSprite;

    public string itemDescription;
    public bool isFull;
    public TMP_Text quantityText;
    public Image itemImage;

    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionName;
    public TMP_Text itemDescriptionText;

    public void FillSlot(string itemName, int quantity, Sprite itemSprite, string itemDescription) {
        this.itemName = itemName;
        this.itemSprite = itemSprite;
        this.quantity = quantity;
        this.itemDescription = itemDescription;
        isFull = true;

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
        itemImage.sprite = itemSprite;
        itemImage.enabled = true;
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
