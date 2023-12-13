using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Interactable
{
    protected override void Start() {
        base.Start();
        StartCoroutine(SetState());
    }

    public override void Interact() {
        state = !state;
        col.enabled = !col.enabled;
        base.Interact();
    }
}
