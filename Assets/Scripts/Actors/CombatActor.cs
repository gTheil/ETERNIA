using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatActor : Actor
{
    public int hitPoint;
    public int hitPointMax;
    public bool pushImmune;
    public float pushRecoverySpeed = 0.2f;

    public Transform projectilePrefab;
    public AudioSource damageSound;
    public AudioSource meleeAttackSound;
    public AudioSource rangedAttackSound;
    public Collider2D hurtbox;

    protected float immuneTime = 0.5f;
    protected float lastImmune;
    protected Vector3 pushDirection;

    protected override void Start() {
        base.Start();
    }

    public void SpawnProjectile(Transform projectileToSpawn, float x, float y, string firedBy) {
        Transform projectile = Instantiate(projectileToSpawn, transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), hitbox);
        projectile.GetComponent<Projectile>().moveX = x;
        projectile.GetComponent<Projectile>().moveY = y;
        projectile.GetComponent<Projectile>().originActor = firedBy;
    }

    public void PlaySound(string sound) {
        if (sound == "meleeAttack")
            meleeAttackSound.Play();
    }

    protected virtual void MeleeAttack() {
        anim.SetTrigger("meleeAttack");
    }

    protected virtual void RangedAttack() {
        anim.SetTrigger("rangedAttack");
        SpawnProjectile(projectilePrefab, moveX, moveY, gameObject.tag);
        rangedAttackSound.Play();
    }

    protected override void UpdateMovement(Vector3 input) {
        moveDelta = new Vector3(input.x, input.y, 0).normalized;

        moveDelta += pushDirection;
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        // detecta se há algo com o qual o ator deve colidir na direção (no eixo X) que ele está tentando se mover
        // se houver, o ator não irá se mover nessa eixo
        hit = Physics2D.BoxCast(hitbox.bounds.center, hitbox.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * currentSpeed * Time.deltaTime), collisionMask);
        if (hit.collider == null) {
            transform.Translate(moveDelta.x * currentSpeed * Time.deltaTime, 0, 0);
        }

        // detecta se há algo com o qual o ator deve colidir na direção (no eixo Y) que ele está tentando se mover
        // se houver, o ator não irá se mover nessa eixo
        hit = Physics2D.BoxCast(hitbox.bounds.center, hitbox.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * currentSpeed * Time.deltaTime), collisionMask);
        if (hit.collider == null) {
            transform.Translate(0, moveDelta.y * currentSpeed * Time.deltaTime, 0);
        }
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
        //pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
    }

    protected virtual void Death() {

    }
}
