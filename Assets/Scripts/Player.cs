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

        playerState = "idle"; // inicializa o jogador no estado "idle"
        anim = GetComponent<Animator>();
    }

    void Update() {
        // caso o jogador aperte a tecla de dash enquanto se movimenta e não está já executando um dash, ele entrará no estado de dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && playerState != "dash" && (moveX != 0 || moveY != 0))
            anim.SetTrigger("dash");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // durante o estado de dash, a velocidade do jogador e a lista de layers com os quais ele colide são alteradas
        // além disso, o jogador não pode alterar sua direção de movimento durante a execução do dash
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

    // função utilizada para alterar o estado do player
    public void SetPlayerState(string state) {
        playerState = state;
    }
}
