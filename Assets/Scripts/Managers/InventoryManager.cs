using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryMenu;
    public ItemSlot[] itemSlots;
    public List<ScriptableItem> scriptableItems = new List<ScriptableItem>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory")) {
            if (inventoryMenu.activeSelf) {
                Time.timeScale = 1;
                inventoryMenu.SetActive(false);
                EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
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

    public void AddItem(int itemID, string itemName, int quantity, Sprite itemSprite, string itemDescription) {
        for (int i = 0; i < itemSlots.Length; i++) {
            if (itemSlots[i].isFull && itemSlots[i].GetItemName() == itemName) {
                itemSlots[i].SetItemQuantity(itemSlots[i].GetItemQuantity() + quantity);
                return;
            } else if (!itemSlots[i].isFull) {
                itemSlots[i].FillSlot(itemID, itemName, quantity, itemSprite, itemDescription);
                return;
            }
        }
    }

    public bool UseItem(int itemID) {
        for (int i = 0; i < scriptableItems.Count; i++) {
            if (i == itemID) {
                bool usable = scriptableItems[i].UseItem();
                return usable;
            }
        }
        return false;
    }
}
