using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    public Interactable[] interactsWith;

    public override void Interact() {
        foreach (Interactable i in interactsWith) {
            i.Interact();
        }
    }
}
