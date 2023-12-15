using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : Interactable
{
    public bool isLocked;
    public int keyID;
    public float displayTimer;
    public Sprite openSprite;

    public AudioSource wrongSound;

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
                    OpenDoor();
                }
                else {
                    Debug.Log("need key");
                    wrongSound.Play();
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
        spr.sprite = openSprite;
        base.Interact();
    }

    private IEnumerator DisplayItem() {
        Item keyNeeded = GameManager.instance.SearchDatabase(ItemType.key, keyID);
        if (keyNeeded != null) {
            displaySprite.sprite = keyNeeded.itemSprite;
            noItemText.text = "X";
            wrongSound.Play();
            displaySprite.enabled = true;
            noItemText.enabled = true;
            yield return new WaitForSecondsRealtime(displayTimer);
            noItemText.text = "";
            displaySprite.enabled = false;
            noItemText.enabled = false;
        }
    }

    protected override IEnumerator SetState() {
        yield return new WaitForSecondsRealtime(0.1f);
        if (state) {
            col.enabled = false;
            spr.sprite = openSprite;
        }
    }
}
