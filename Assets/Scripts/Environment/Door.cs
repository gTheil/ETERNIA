using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public bool isLocked;
    public int keyID;

    public override void Interact() {
        if (isLocked) {
            bool hasKey = GameManager.instance.CheckInventory(ItemType.key, keyID);
            if (hasKey) {
                Debug.Log("unlocked");
                GameManager.instance.RemoveItem(keyID);
                OpenDoor();
            }
            else {
                Debug.Log("need key");
            }
        } else {
            OpenDoor();
        }
    }

    private void OpenDoor() {
        Debug.Log("open");
        col.enabled = false;
    }
}
