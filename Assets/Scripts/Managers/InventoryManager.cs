using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryMenu;
    public GameObject equipmentMenu;
    public ItemSlot[] itemSlots;
    public ItemSlot[] equipSlots;
    public List<ItemSO> scriptableItems = new List<ItemSO>();
    public List<EquipmentSO> scriptableEquips = new List<EquipmentSO>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
            Inventory();

        if (Input.GetButtonDown("Equipment"))
            Equipment();
    }

    public void Inventory() {
        if (inventoryMenu.activeSelf) {
            Time.timeScale = 1;
            equipmentMenu.SetActive(false);
            inventoryMenu.SetActive(false);
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
        else {
            Time.timeScale = 0;
            equipmentMenu.SetActive(false);
            inventoryMenu.SetActive(true);
            for (int i = 0; i < itemSlots.Length; i++){
                if (itemSlots[i].isFull) {
                    itemSlots[i].gameObject.GetComponent<Selectable>().Select();
                    return;
                }
            }
        }
    }

    public void Equipment() {
        if (equipmentMenu.activeSelf) {
            Time.timeScale = 1;
            inventoryMenu.SetActive(false);
            equipmentMenu.SetActive(false);
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
        else {
            Time.timeScale = 0;
            inventoryMenu.SetActive(false);
            equipmentMenu.SetActive(true);
            for (int i = 0; i < equipSlots.Length; i++){
                if (equipSlots[i].isFull) {
                    equipSlots[i].gameObject.GetComponent<Selectable>().Select();
                    return;
                }
            }
        }
    }

    public void AddItem(int itemID, string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType) {
        if (itemType == ItemType.consumable || itemType == ItemType.key) {
            for (int i = 0; i < itemSlots.Length; i++) {
                if (itemSlots[i].isFull && itemSlots[i].GetItemID() == itemID) {
                    itemSlots[i].SetItemQuantity(itemSlots[i].GetItemQuantity() + quantity);
                    return;
                } else if (!itemSlots[i].isFull) {
                    itemSlots[i].FillSlot(itemID, itemName, quantity, itemSprite, itemDescription, itemType);
                    return;
                }
            }
        } else if (itemType == ItemType.none) {
            Debug.Log("Item type not set.");
        } else {
            for (int i = 0; i < equipSlots.Length; i++) {
                if (!equipSlots[i].isFull) {
                    equipSlots[i].FillSlot(itemID, itemName, quantity, itemSprite, itemDescription, itemType);
                    return;
                }
            }
        }
    }

    public bool UseItem(int itemID) {
        for (int i = 0; i < scriptableItems.Count; i++) {
            if (scriptableItems[i].itemID == itemID) {
                bool usable = scriptableItems[i].UseItem();
                return usable;
            }
        }
        return false;
    }

    public bool EquipItem(int itemID) {
        for (int i = 0; i < scriptableEquips.Count; i++) {
            if (scriptableEquips[i].equipID == itemID) {
                bool equippable = scriptableEquips[i].EquipItem();
                if (equippable) {
                    switch (scriptableEquips[i].equipType) {
                        case EquipType.sword:
                            for (int j = 0; j < equipSlots.Length; j++) {
                                if (equipSlots[j].itemType == ItemType.sword)
                                    equipSlots[j].quantityText.enabled = false;
                            }
                            break;
                        case EquipType.bow:
                            for (int j = 0; j < equipSlots.Length; j++) {
                                if (equipSlots[j].itemType == ItemType.bow)
                                    equipSlots[j].quantityText.enabled = false;
                            }
                            break;
                        case EquipType.shield:
                            for (int j = 0; j < equipSlots.Length; j++) {
                                if (equipSlots[j].itemType == ItemType.shield)
                                    equipSlots[j].quantityText.enabled = false;
                            }
                            break;
                        default:
                            break;
                    }
                }
                return equippable;
            }
        }
        return false;
    }
}

public enum ItemType {
    consumable,
    sword,
    bow,
    shield,
    key,
    none,
};