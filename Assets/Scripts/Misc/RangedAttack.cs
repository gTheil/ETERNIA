using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MeleeAttack
{
    private string originTag;
    private Collider2D coll;

    private void Start() {
        attackDamage = transform.parent.GetComponent<Projectile>().originActor.gameObject.GetComponent<CombatActor>().rangedDamage;
        attackPush = transform.parent.GetComponent<Projectile>().originActor.gameObject.GetComponent<CombatActor>().rangedPush;
        originTag = transform.parent.GetComponent<Projectile>().originActor.gameObject.tag;
        coll = GetComponent<Collider2D>();
        coll.enabled = true;
    }

    protected override void Update() {
        
    }

    protected override void OnTriggerEnter2D(Collider2D col) {
        if (col.transform.parent.tag != originTag) {
            Damage dmg = new Damage {
            damageAmount = attackDamage,
            origin = transform.position,
            pushForce = attackPush,
            originTag = this.originTag
            };
            col.transform.parent.SendMessage("TakeDamage", dmg);
            attackHitSound.Play();
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
