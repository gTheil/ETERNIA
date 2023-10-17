using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public float meleeDistance; // a distância que o jogador precisa estar do inimigo para que o inimigo tente atacá-lo

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (actorState != "meleeAttack")
            base.FixedUpdate();

        if (actorState == "chase" && Vector3.Distance(playerTransform.position, transform.position) < meleeDistance)
            MeleeAttack();
    }

    protected override void MeleeAttack() {
        lastState = actorState;
        base.MeleeAttack();
    }

}
