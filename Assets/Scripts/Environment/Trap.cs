using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Interactable
{
    public int damage;

    public override void Interact() {
        col.enabled = !col.enabled;
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
