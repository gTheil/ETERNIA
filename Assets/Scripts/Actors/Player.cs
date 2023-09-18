using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CombatActor
{
    public string playerState;

    //private Animator anim;
    private Animator meleeAttackAnim;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        playerState = "idle"; // inicializa o jogador no estado "idle"
        //anim = GetComponent<Animator>();
        meleeAttackAnim = transform.GetChild(0).GetComponent<Animator>();
    }

    protected override void Update() {
        base.Update();

        // durante o estado de dash, a velocidade do jogador e a lista de layers com os quais ele colide são alteradas
        if (playerState == "dash") {
            collisionMask = LayerMask.GetMask("Solid");
            speed = 10;
        } else {
            collisionMask = LayerMask.GetMask("Actor", "Solid");
            speed = 5;
        }

        // caso o jogador aperte a tecla de dash enquanto se movimenta e não está já executando um dash, ele entrará no estado de dash
        if (playerState == "idle" ) {
            if (Input.GetKeyDown(KeyCode.LeftShift) && (moveX != 0 || moveY != 0))
                Dash();
            if (Input.GetKeyDown(KeyCode.Z))
                MeleeAttack();
        }
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        // o jogador não pode alterar sua direção de movimento durante a execução do dash
        if (playerState != "dash") {
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");
        }
    
        base.FixedUpdate();

        if (anim != null) {
            if (playerState != "dash" && (moveX != 0 || moveY != 0)) {
                anim.SetFloat("x", moveX);
                anim.SetFloat("y", moveY);
                ActiveAnimatorLayer("Walk");
            } else {
                switch (playerState) {
                    case "idle":
                        ActiveAnimatorLayer("Idle");
                        break;
                    default:
                        break;
                }
            }
        }
        
    }

    // função utilizada para alterar o estado do player
    public void SetPlayerState(string state) {
        playerState = state;
    }

    private void Dash() {
        anim.SetTrigger("dash");
    }

    private void MeleeAttack() {
        meleeAttackAnim.SetTrigger("meleeAttackSide");
        moveX = 0;
        moveY = 0;
    }

    public void ActiveAnimatorLayer(string layerName) {
        for (int i = 0; i < anim.layerCount; i++) {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(anim.GetLayerIndex(layerName), 1);
    }
}
