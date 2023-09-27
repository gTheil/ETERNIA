using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage;
    public float attackPush;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "CombatActor") {
            Damage dmg = new Damage {
                damageAmount = attackDamage,
                origin = transform.position,
                pushForce = attackPush
            };

            col.SendMessage("TakeDamage", dmg);
        }
    }
}
