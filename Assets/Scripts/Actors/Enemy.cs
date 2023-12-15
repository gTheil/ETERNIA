using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CombatActor
{
    public float triggerDistance; // a distância que o jogador precisa estar do inimigo para que o inimigo comece a perseguí-lo
    public float chaseDistance; // a distância máxima de sua posição inicial que o inimigo persegue o jogador
    public float chaseCooldown; // após acertar o jogador, quanto tempo deve esperar até voltar a perseguí-lo

    public bool chasing;
    private float lastHit;
    protected Transform playerTransform;
    public Vector3 startingPosition;
    protected SpriteRenderer spr;

    protected override void Start() {
        base.Start();

        playerTransform = GameManager.instance.GetPlayer().transform;
        startingPosition = transform.position;
        spr = GetComponent<SpriteRenderer>();
    }

    protected virtual void FixedUpdate() {
        moveX = Mathf.Round(moveDelta.x);
        moveY = Mathf.Round(moveDelta.y);

        if (!GameManager.instance.IsPlayerDead()) {
            if (actorState != "cooldown") {
                if (Vector3.Distance(playerTransform.position, startingPosition) < chaseDistance) {
                    if (Vector3.Distance(playerTransform.position, transform.position) < triggerDistance)
                        SetActorState("chase");
                } else {
                    if (actorState == "chase") {
                        SetActorState("return");
                    }
                }
            } else {
                UpdateMovement(Vector3.zero);
            }

            switch(actorState) {
                case "idle":
                    UpdateMovement(Vector3.zero);
                    break;
                case "chase":
                    UpdateMovement((playerTransform.position - transform.position).normalized);
                    break;
                case "return":
                    UpdateMovement((startingPosition - transform.position).normalized);
                    if (Vector3.Distance(transform.position, startingPosition) <= 0.1f)
                        SetActorState("idle");
                    break;
                case "cooldown":
                    if (Time.time - lastHit > chaseCooldown)
                        SetActorState("chase");
                    break;
                default:
                UpdateMovement(Vector3.zero);
                    break;
            }
        }

        if (Time.time - lastImmune > immuneTime) {
            anim.SetFloat("x", moveX);
            anim.SetFloat("y", moveY);
        }

    }

    protected override void OnDamageDealt() {
        SetActorState("cooldown");
        lastHit = Time.time;
    }

    protected override void TakeDamage(Damage dmg) {
        base.TakeDamage(dmg);
        if (hitPoint > 0)
            SetActorState("chase");
    }

    protected override void Death() {
        GameManager.instance.EnemyDeath();
        Destroy(gameObject);
    }
}
