using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : CombatActor
{
    public float shootDistance; // distância que o jogador deve estar do inimigo para que o inimigo comece a disparar
    public float runTriggerDistance; // distância que o jogador deve estar do inimigo para que o inimigo comece a recuar
    public float maxRunTime; // quanto tempo o inimigo recua antes de atacar novamente após o jogador se aproximar

    public float lookX;
    public float lookY;
    private Transform playerTransform;
    public float runTimer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        playerTransform = GameManager.instance.GetPlayer().transform;
    }

    private void FixedUpdate() {
        moveX = Mathf.Round(moveDelta.x);
        moveY = Mathf.Round(moveDelta.y);

        if (actorState == "run") {
            anim.SetFloat("x", moveX);
            anim.SetFloat("y", moveY);
        } else {
            anim.SetFloat("x", lookX);
            anim.SetFloat("y", lookY);
        }

        if (Vector3.Distance(playerTransform.position, transform.position) < shootDistance) {
            lookX = (playerTransform.position - transform.position).normalized.x;
            lookY = (playerTransform.position - transform.position).normalized.y;

            if (actorState != "rangedAttack" && Time.time - runTimer > maxRunTime) {
                if (Vector3.Distance(playerTransform.position, transform.position) < runTriggerDistance) {
                    SetActorState("run");
                    runTimer = Time.time;
                } else {
                    RangedAttack();
                }
            }
        } else {
            SetActorState("idle");
        }

        switch (actorState) {
            case "run":
                UpdateMovement(((playerTransform.position - transform.position).normalized) * -1);
                break;
            default:
                UpdateMovement(Vector3.zero);
                break;
        }

    }

    protected override void RangedAttack() {
        anim.SetTrigger("rangedAttack");
        SpawnProjectile(projectilePrefab, lookX, lookY, transform);
        rangedAttackSound.Play();
    }

    protected override void Death() {
        Destroy(gameObject);
    }

}
