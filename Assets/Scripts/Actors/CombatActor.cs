using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActor : Actor
{
    public Transform projectilePrefab;

    public void SpawnProjectile(Transform projectileToSpawn) {
        Transform projectile = Instantiate(projectileToSpawn, transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), boxCollider);
        projectile.GetComponent<Projectile>().moveX = anim.GetFloat("x");
        projectile.GetComponent<Projectile>().moveY = anim.GetFloat("y");
    }
}
