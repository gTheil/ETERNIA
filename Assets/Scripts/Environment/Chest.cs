using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chest : Interactable
{
    public bool isLocked;
    public int keyID;
    public Item item;
    public int gold;
    public Sprite goldSprite;
    public float displayTimer;
    public Sprite openSprite;

    private SpriteRenderer spr;

    private SpriteRenderer displaySprite;
    private TMP_Text noItemText;

    protected override void Start() {
        base.Start();
        displaySprite = GameManager.instance.GetDisplaySprite();
        noItemText = GameManager.instance.GetNoItemText();
        spr = GetComponent<SpriteRenderer>();
        StartCoroutine(SetState());
    }

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
                    StartCoroutine(DisplayItem(false));
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
        spr.sprite = openSprite;
        StartCoroutine(DisplayItem(true));
        base.Interact();
    }

    private IEnumerator DisplayItem(bool chestOpen) {
        if (chestOpen) {
            if (item == null)
                displaySprite.sprite = goldSprite;
            else
                displaySprite.sprite = item.itemSprite;
            //play item get sound
        } else {
            Item keyNeeded = GameManager.instance.SearchDatabase(ItemType.key, keyID);
            displaySprite.sprite = keyNeeded.itemSprite;
            noItemText.text = "X";
            //play wrong sound
        }
        displaySprite.enabled = true;
        noItemText.enabled = true;
        yield return new WaitForSecondsRealtime(displayTimer);
        noItemText.text = "";
        displaySprite.enabled = false;
        noItemText.enabled = false;
    }

    protected override IEnumerator SetState() {
        yield return new WaitForSecondsRealtime(0.1f);
        if (state)
            spr.sprite = openSprite;
    }
}
