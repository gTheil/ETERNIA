using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatActor : Actor
{
    public int hitPoint;
    public int hitPointMax;
    public float pushRecoverySpeed = 0.2f;
    public bool pushImmune;

    public Transform projectilePrefab;
    public AudioSource damageSound;
    public Collider2D hurtbox;

    protected float immuneTime = 0.5f;
    protected float lastImmune;
    public string actorState;
    public string lastState;

    protected Vector3 pushDirection;

    protected override void Start() {
        base.Start();

        actorState = "idle"; // inicializa o jogador no estado "idle"
        lastState = actorState;
    }

    // função utilizada para alterar o estado do player
    public void SetActorState(string state) {
        actorState = state;
    }

    public void SpawnProjectile(Transform projectileToSpawn, string firedBy) {
        Transform projectile = Instantiate(projectileToSpawn, transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), hitbox);
        projectile.GetComponent<Projectile>().moveX = anim.GetFloat("x");
        projectile.GetComponent<Projectile>().moveY = anim.GetFloat("y");
        projectile.GetComponent<Projectile>().originActor = firedBy;
    }

    protected virtual void TakeDamage(Damage dmg) {
        if (Time.time - lastImmune > immuneTime) {
            lastImmune = Time.time;
            hitPoint -= dmg.damageAmount;
            if (!pushImmune)
                pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            damageSound.Play();
            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 25, Color.red, transform.position, Vector3.up * 50f, 0.5f);
            GameManager.instance.UpdateDebugUI("hit");

            if (hitPoint <= 0) {
                hitPoint = 0;
                Death();
            }
                
        }
    }

    protected virtual void OnDamageDealt() {

    }

    protected virtual void Death() {

    }
}
