using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, ISelectHandler, ISubmitHandler
{
    [SerializeField]
    public int itemID;
    [SerializeField]
    protected string itemName;
    [SerializeField]
    public int quantity;
    [SerializeField]
    protected Sprite itemSprite;
    [SerializeField] [TextArea]
    protected string itemDescription;
    public ItemType itemType;

    public bool isFull;
    public TMP_Text quantityText;
    public Image itemImage;

    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionName;
    public TMP_Text itemDescriptionText;

    public AudioSource slotHover;
    public AudioSource slotClick;

    public void FillSlot(int itemID, string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType) {
        this.itemID = itemID;
        this.itemName = itemName;
        this.quantity = quantity;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;
        this.itemType = itemType;
        isFull = true;

        if (itemType == ItemType.consumable || itemType == ItemType.key) {
            quantityText.text = quantity.ToString();
            quantityText.enabled = true;
        }
        else {
            quantityText.text = "E";
        }
        
        itemImage.sprite = itemSprite;
        itemImage.enabled = true;
    }

    public virtual void EmptySlot() {
        this.itemID = 0;
        this.itemName = "";
        this.quantity = 0;
        this.itemSprite = null;
        this.itemDescription = "";
        this.itemType = ItemType.none;
        isFull = false;

        quantityText.enabled = false;
        itemImage.enabled = false;

        itemDescriptionName.text = itemName;
        itemDescriptionText.text = itemDescription;
        itemDescriptionImage.enabled = false;
    }

    public virtual void OnSelect(BaseEventData eventData) {
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
        slotHover.Play();
    }

    public virtual void OnSubmit(BaseEventData eventData) {
        if (isFull) {
            Debug.Log(itemName);
            if (itemType == ItemType.consumable) {
                bool usable = GameManager.instance.UseItem(itemID);
                if (usable) {
                    GameManager.instance.RemoveItem(itemID);
                }
            } else if (itemType == ItemType.sword || itemType == ItemType.bow || itemType == ItemType.shield) {
                bool equippable = GameManager.instance.EquipItem(itemID);
                if (equippable) {
                    quantityText.enabled = true;
                }
            }
            slotClick.Play();
        }
    }

    public int GetItemID() {
        return itemID;
    }

    public int GetItemQuantity() {
        return quantity;
    }

    public void SetItemQuantity(int quantity) {
        this.quantity = quantity;
        quantityText.text = quantity.ToString();
    }

}
