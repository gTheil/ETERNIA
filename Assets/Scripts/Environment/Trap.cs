using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Interactable
{
    public int damage;

    public override void Interact() {
        state = !state;
        col.enabled = !col.enabled;
        base.Interact();
    }

    private void OnTriggerStay2D(Collider2D col) {
        Damage dmg = new Damage {
        damageAmount = damage,
        origin = transform.position,
        pushForce = 0f
        };
        col.transform.parent.SendMessage("TakeDamage", dmg);
    }

}
