using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    public Interactable[] interactsWith;
    public Sprite activatedSprite;

    private SpriteRenderer spr;
    private Sprite deactivatedSprite;

    protected override void Start() {
        base.Start();
        spr = GetComponent<SpriteRenderer>();
        deactivatedSprite = spr.sprite;
        StartCoroutine(SetState());
    }

    public override void Interact() {
        base.Interact();
        foreach (Interactable i in interactsWith) {
            i.Interact();
        }
        state = !state;
        if (state)
            spr.sprite = activatedSprite;
        else
            spr.sprite = deactivatedSprite;
    }

    protected override IEnumerator SetState() {
        yield return new WaitForSecondsRealtime(0.1f);
        if (state)
            spr.sprite = activatedSprite;
        else
            spr.sprite = deactivatedSprite;
    }
}
