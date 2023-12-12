using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public bool isLocked;
    public int keyID;
    public float displayTimer;

    private SpriteRenderer displaySprite;

    protected override void Start() {
        base.Start();
        displaySprite = GameManager.instance.GetDisplaySprite();
        StartCoroutine(SetState());
    }

    public override void Interact() {
        if (!state) {
            if (isLocked) {
                bool hasKey = GameManager.instance.CheckInventory(ItemType.key, keyID);
                if (hasKey) {
                    Debug.Log("unlocked");
                    GameManager.instance.RemoveItem(keyID);
                    OpenDoor();
                }
                else {
                    Debug.Log("need key");
                    StartCoroutine(DisplayItem());
                }
            } else {
                OpenDoor();
            }
        }
    }

    private void OpenDoor() {
        Debug.Log("open");
        state = true;
        col.enabled = false;
        base.Interact();
    }

    private IEnumerator DisplayItem() {
        Item keyNeeded = GameManager.instance.SearchDatabase(ItemType.key, keyID);
        if (keyNeeded != null) {
            displaySprite.sprite = keyNeeded.itemSprite;
            //play wrong sound
            displaySprite.enabled = true;
            yield return new WaitForSecondsRealtime(displayTimer);
            displaySprite.enabled = false;
        }
    }

    private IEnumerator SetState() {
        yield return new WaitForSecondsRealtime(0.1f);
        if (state)
            col.enabled = false;
    }
}
