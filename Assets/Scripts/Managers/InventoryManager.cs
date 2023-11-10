using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryMenu;
    public GameObject equipmentMenu;
    public GameObject shopMenu;
    public ItemSlot[] itemSlots;
    public ItemSlot[] equipSlots;
    public ShopSlot[] shopSlots;
    public List<ShopItemSO> equipmentShopInventory = new List<ShopItemSO>();
    public List<ShopItemSO> consumableShopInventory = new List<ShopItemSO>();
    public List<ItemSO> scriptableItems = new List<ItemSO>();
    public List<EquipmentSO> scriptableEquips = new List<EquipmentSO>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
            Inventory();

        if (Input.GetButtonDown("Equipment"))
            Equipment();

        if (Input.GetButtonDown("Shop1Debug"))
            Shop("equipment");

        if (Input.GetButtonDown("Shop2Debug"))
            Shop("consumable");
    }

    public void Inventory() {
        if (inventoryMenu.activeSelf) {
            Time.timeScale = 1;
            equipmentMenu.SetActive(false);
            inventoryMenu.SetActive(false);
            shopMenu.SetActive(false);
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
        else {
            Time.timeScale = 0;
            equipmentMenu.SetActive(false);
            shopMenu.SetActive(false);
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
            shopMenu.SetActive(false);
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
        else {
            Time.timeScale = 0;
            inventoryMenu.SetActive(false);
            shopMenu.SetActive(false);
            equipmentMenu.SetActive(true);
            for (int i = 0; i < equipSlots.Length; i++){
                if (equipSlots[i].isFull) {
                    equipSlots[i].gameObject.GetComponent<Selectable>().Select();
                    return;
                }
            }

            Player player = GameManager.instance.GetPlayer().GetComponent<Player>();
            GameManager.instance.UpdateStatsUI(player.GetHitPoint(), player.GetHitPointMax(), player.GetBlockPoint(), player.GetBlockPointMax(), player.GetMeleeDamage(), player.GetRangedDamage(), player.GetBlockFactor());
        }
    }

    public void Shop(string shopType) {
        if (shopMenu.activeSelf) {
            Time.timeScale = 1;
            for (int i = 0; i < shopSlots.Length; i++){
                shopSlots[i].EmptySlot();
            }
            inventoryMenu.SetActive(false);
            equipmentMenu.SetActive(false);
            shopMenu.SetActive(false);
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        } else {
            Time.timeScale = 0;
            inventoryMenu.SetActive(false);
            equipmentMenu.SetActive(false);
            shopMenu.SetActive(true);

            for (int i = 0; i < shopSlots.Length; i++){
                shopSlots[i].EmptySlot();
            }

            if (shopType == "equipment") {
                for (int i = 0; i < shopSlots.Length; i++) {
                    if (i < equipmentShopInventory.Count) {
                        shopSlots[i].FillSlot(equipmentShopInventory[i].itemID, equipmentShopInventory[i].itemName, equipmentShopInventory[i].itemQuantity,
                        equipmentShopInventory[i].itemSprite, equipmentShopInventory[i].itemDescription, equipmentShopInventory[i].itemType, equipmentShopInventory[i].itemPrice);
                    } else {
                        return;
                    }   
                }
            } else if (shopType == "consumable") {
                for (int i = 0; i < shopSlots.Length; i++) {
                    if (i < consumableShopInventory.Count) {
                        shopSlots[i].FillSlot(consumableShopInventory[i].itemID, consumableShopInventory[i].itemName, consumableShopInventory[i].itemQuantity,
                        consumableShopInventory[i].itemSprite, consumableShopInventory[i].itemDescription, consumableShopInventory[i].itemType, consumableShopInventory[i].itemPrice);
                    } else {
                        return;
                    }   
                }
            }

            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(shopSlots[0].gameObject);
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