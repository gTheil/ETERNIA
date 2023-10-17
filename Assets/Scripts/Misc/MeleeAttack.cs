using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public int attackDamage;
    public float attackPush;
    public AudioSource attackHitSound;

    private void OnTriggerEnter2D(Collider2D col) {
        Debug.Log(col.gameObject.name);
        if (col.transform.parent.tag != transform.parent.tag) {
            Damage dmg = new Damage {
            damageAmount = attackDamage,
            origin = transform.parent.position,
            pushForce = attackPush
            };
            transform.parent.SendMessage("OnDamageDealt");
            col.transform.parent.SendMessage("TakeDamage", dmg);
            attackHitSound.Play();
        }
    }
}
