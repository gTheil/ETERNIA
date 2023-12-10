using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public bool isLocked;
    public int keyID;

    protected override void Start() {
        base.Start();
        StartCoroutine(SetState());
    }

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
        state = true;
        col.enabled = false;
        base.Interact();
    }

    private IEnumerator SetState() {
        yield return new WaitForSecondsRealtime(0.1f);
        if (state)
            col.enabled = false;
    }
}
