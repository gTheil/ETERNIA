using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Interactable
{
    private SpriteRenderer spr;

    protected override void Start() {
        base.Start();
        spr = GetComponent<SpriteRenderer>();
        StartCoroutine(SetState());
    }

    public override void Interact() {
        state = !state;
        col.enabled = !col.enabled;
        spr.enabled = !spr.enabled;
        base.Interact();
    }

    protected override IEnumerator SetState() {
        yield return new WaitForSecondsRealtime(0.1f);
        if (state) {
            col.enabled = false;
            spr.enabled = false;
        }
    }
}
