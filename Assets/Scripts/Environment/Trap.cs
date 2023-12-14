using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Interactable
{
    public int damage;

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

    private void OnTriggerStay2D(Collider2D col) {
        Damage dmg = new Damage {
        damageAmount = damage,
        origin = transform.position,
        pushForce = 0f
        };
        col.transform.parent.SendMessage("TakeDamage", dmg);
    }

    protected override IEnumerator SetState() {
        yield return new WaitForSecondsRealtime(0.1f);
        if (state) {
            col.enabled = false;
            spr.enabled = false;
        }
    }
}
