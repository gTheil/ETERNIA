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
    private Transform playerTransform;
    public Vector3 startingPosition;

    protected override void Start() {
        base.Start();

        playerTransform = GameManager.instance.GetPlayer();
        startingPosition = transform.position;
    }

    protected virtual void FixedUpdate() {
        if (actorState != "cooldown") {
            if (Vector3.Distance(playerTransform.position, startingPosition) < chaseDistance) {
                if (Vector3.Distance(playerTransform.position, transform.position) < triggerDistance)
                    SetActorState("chase");
            } else {
                if (Vector3.Distance(transform.position, startingPosition) <= 1f)
                    SetActorState("idle");
                else
                    SetActorState("return");
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
                break;
            case "cooldown":
                if (Time.time - lastHit > chaseCooldown)
                    SetActorState("chase");
                break;
            default:
                break;
        }
    }

    protected override void OnDamageDealt() {
        SetActorState("cooldown");
        lastHit = Time.time;
    }

    protected override void Death() {
        Destroy(gameObject);
    }
}
