using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Actor
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // NPC movement test
        moveX = 1;
        moveY = -1;

        base.FixedUpdate();
    }
}
