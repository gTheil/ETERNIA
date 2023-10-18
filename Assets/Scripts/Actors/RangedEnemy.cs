using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : CombatActor
{
    private float shootingDistance;
    private float runDistance;
    private Transform playerTransform;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        playerTransform = GameManager.instance.GetPlayer();
    }

    private void FixedUpdate() {

    }

}
