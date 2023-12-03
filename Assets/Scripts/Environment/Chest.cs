using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    public bool isLocked;
    public int keyID;
    public Item item;
    public int gold;

    //private bool isOpen = false;

    public override void Interact() {
        if (!state) {
            if (isLocked) {
                bool hasKey = GameManager.instance.CheckInventory(ItemType.key, keyID);
                if (hasKey) {
                    Debug.Log("unlocked");
                    GameManager.instance.RemoveItem(keyID);
                    OpenChest();
                }
                else {
                    Debug.Log("need key");
                }
            } else {
                OpenChest();
            }
        }
    }

    private void OpenChest() {
        Debug.Log("open");
        if (item != null) {
            GameManager.instance.AddItem(item.itemID, item.itemName, item.itemQuantity, item.itemSprite, item.itemDescription, item.itemType);
        } else {
            GameManager.instance.GetPlayer().GetComponent<Player>().gold += this.gold;
        }
        state = true;
        base.Interact();
    }
}
