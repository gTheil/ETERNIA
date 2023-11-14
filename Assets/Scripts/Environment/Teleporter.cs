using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : Interactable
{
    public Transform teleportTo;

    public override void Interact() {
        GameManager.instance.GetPlayer().transform.position = teleportTo.position;
    }
}
