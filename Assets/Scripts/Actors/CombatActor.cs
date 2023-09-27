using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActor : Actor
{
    public int hitPoint;
    public int maxHitPoint;
    public float pushRecoverySpeed = 0.2f;

    public Transform projectilePrefab;

    protected float immuneTime = 0.5f;
    protected float lastImmune;

    protected Vector3 pushDirection;

    protected override void FixedUpdate() {
        base.FixedUpdate();

        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);
        Debug.Log(moveDelta);
    }

    public void SpawnProjectile(Transform projectileToSpawn) {
        Transform projectile = Instantiate(projectileToSpawn, transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), boxCollider);
        projectile.GetComponent<Projectile>().moveX = anim.GetFloat("x");
        projectile.GetComponent<Projectile>().moveY = anim.GetFloat("y");
    }

    protected override void SetMoveDelta() {
        moveDelta = new Vector3(moveX, moveY, 0);
        moveDelta += pushDirection;
    }

    protected virtual void TakeDamage(Damage dmg) {
        if (Time.time - lastImmune > immuneTime) {
            lastImmune = Time.time;
            hitPoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 25, Color.red, transform.position, Vector3.up * 50f, 0.5f);

            if (hitPoint <= 0) {
                hitPoint = 0;
                Death();
            }
        }
    }

    protected virtual void Death() {

    }
}
