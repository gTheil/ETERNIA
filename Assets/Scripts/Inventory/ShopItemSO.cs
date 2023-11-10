using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShopItemSO : ScriptableObject
{
    public int itemID;
    public string itemName;
    [TextArea]
    public string itemDescription;
    public int itemQuantity;
    public ItemType itemType;
    public int itemPrice;

    public Sprite itemSprite;
}
