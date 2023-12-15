using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public AudioSource attackHitSound;

    protected int attackDamage;
    protected float attackPush;

    // Start is called before the first frame update
    protected virtual void Update()
    {
        attackDamage = transform.parent.GetComponent<CombatActor>().meleeDamage;
        attackPush = transform.parent.GetComponent<CombatActor>().meleePush;
    }

    protected virtual void OnTriggerEnter2D(Collider2D col) {
        Debug.Log(attackDamage);
        if (col.transform.parent.tag != transform.parent.tag) {
            Damage dmg = new Damage {
            damageAmount = attackDamage,
            origin = transform.parent.position,
            pushForce = attackPush
            };
            transform.parent.SendMessage("OnDamageDealt");
            col.transform.parent.SendMessage("TakeDamage", dmg);
            //attackHitSound.Play();
        }
    }
}
