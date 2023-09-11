using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
    
        base.FixedUpdate();
    }
}
