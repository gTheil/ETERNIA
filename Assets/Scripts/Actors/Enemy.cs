using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CombatActor
{
    public float triggerDistance; // a distância que o jogador precisa estar do inimigo para que o inimigo comece a perseguí-lo
    public float chaseDistance; // a distância máxima de sua posição inicial que o inimigo persegue o jogador
    public float chaseCooldown; // após acertar o jogador, quanto tempo deve esperar até voltar a perseguí-lo

    private bool chasing;
    private float lastCollision;
    private Transform playerTransform;
    private Vector3 startingPosition;

    protected override void Start() {
        base.Start();

        playerTransform = GameManager.instance.GetPlayer();
        startingPosition = transform.position;
    }

    private void FixedUpdate() {
        UpdateMovement(Vector3.zero);
    }
}
