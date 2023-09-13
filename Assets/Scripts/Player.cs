using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    public string playerState;

    private Animator anim;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        playerState = "idle";
        anim = GetComponent<Animator>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift) && playerState != "dash" && (moveX != 0 || moveY != 0))
            anim.SetTrigger("dash");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerState == "dash") {
            speed = 10;
            collisionMask = LayerMask.GetMask("Solid");
        } else {
            speed = 5;
            collisionMask = LayerMask.GetMask("Actor", "Solid");
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");
        }
    
        base.FixedUpdate();
    }

    public void SetPlayerState(string state) {
        playerState = state;
    }
}
