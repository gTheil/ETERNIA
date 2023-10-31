using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryMenu;
    public ItemSlot[] itemSlots;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory")) {
            if (inventoryMenu.activeSelf) {
                Time.timeScale = 1;
                inventoryMenu.SetActive(false);
            }
            else {
                Time.timeScale = 0;
                inventoryMenu.SetActive(true);
                for (int i = 0; i < itemSlots.Length; i++){
                    if (itemSlots[i].isFull) {
                        itemSlots[i].gameObject.GetComponent<Selectable>().Select();
                        return;
                    }
                }
            }
        }
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription) {
        for (int i = 0; i < itemSlots.Length; i++) {
            if (itemSlots[i].isFull && itemSlots[i].GetItemName() == itemName) {
                itemSlots[i].SetItemQuantity(itemSlots[i].GetItemQuantity() + quantity);
                return;
            } else if (!itemSlots[i].isFull) {
                itemSlots[i].FillSlot(itemName, quantity, itemSprite, itemDescription);
                return;
            }
        }
    }
}
