using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MeleeAttack
{
    protected override void Start() {
        attackDamage = transform.parent.GetComponent<Projectile>().originActor.gameObject.GetComponent<CombatActor>().rangedDamage;
        attackPush = transform.parent.GetComponent<Projectile>().originActor.gameObject.GetComponent<CombatActor>().rangedPush;
    }

    protected override void OnTriggerEnter2D(Collider2D col) {
        string originTag = transform.parent.GetComponent<Projectile>().originActor.gameObject.tag;
        if (col.transform.parent.tag != originTag) {
            Damage dmg = new Damage {
            damageAmount = attackDamage,
            origin = transform.position,
            pushForce = attackPush
            };
            col.transform.parent.SendMessage("TakeDamage", dmg);
            attackHitSound.Play();
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
