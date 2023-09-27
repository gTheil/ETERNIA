using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CombatActor
{
    public string playerState;
    public GameObject meleeAttack;

    private Collider2D meleeAttackCollider;
    private List<ActionItem> inputBuffer = new List<ActionItem>(); //The input buffer
    private bool actionAllowed; //set to true whenever we want to process actions from the input buffer, set to false when an action has to wait in the buffer

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        playerState = "idle"; // inicializa o jogador no estado "idle"
        meleeAttackCollider = meleeAttack.GetComponent<BoxCollider2D>();
    }

    private void Update() {
        switch (playerState) {
            case "dash":
                currentSpeed = baseSpeed * 2;
                break;
            case "meleeAttack":
                currentSpeed = baseSpeed / 2;
                break;
            case "rangedAttack":
                currentSpeed = 0;
                break;
            default:
                currentSpeed = baseSpeed;
                break;
        }

        if (playerState == "dash") {
            collisionMask = LayerMask.GetMask("Solid");
        } else {
            collisionMask = LayerMask.GetMask("Actor", "Solid");
        }

        CheckInput();

        if (playerState == "idle")
            actionAllowed = true;

        if (actionAllowed)
            TryBufferedAction();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        // o jogador não pode alterar sua direção de movimento durante a execução do dash
        if (playerState == "idle") {
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

    private void CheckInput() {
        if (Input.GetKeyDown(KeyCode.LeftShift) && (moveX != 0 || moveY != 0))
            inputBuffer.Add(new ActionItem(ActionItem.InputAction.Dash, Time.time));
        if (Input.GetKeyDown(KeyCode.Z))
            inputBuffer.Add(new ActionItem(ActionItem.InputAction.MeleeAttack, Time.time));
        if (Input.GetKeyDown(KeyCode.X))
            inputBuffer.Add(new ActionItem(ActionItem.InputAction.RangedAttack, Time.time));    
    }

    private void TryBufferedAction() {
        if (inputBuffer.Count > 0) {
            foreach (ActionItem ai in inputBuffer.ToArray()) {  //Using ToArray so we iterate a copy of the list rather than the actual list, since we will be modifying the list in the loop
                inputBuffer.Remove(ai);  //Remove it from the buffer
                if (ai.CheckIfValid()) {
                    //Means the action is still within the allowed time, so we do the action and then break from processing more of the buffer
                    DoAction(ai);
                    break;  //We probably only want to do 1 action at a time, so we just break here and don't process the rest of the inputBuffer
                }
            }
        }
    }

    private void DoAction(ActionItem ai) {
        switch (ai.action) {
            case ActionItem.InputAction.Dash:
                Dash();
                break;
            case ActionItem.InputAction.MeleeAttack:
                MeleeAttack();
                break;
            case ActionItem.InputAction.RangedAttack:
                RangedAttack();
                break;
            default:
                break;
        }

        actionAllowed = false;
    }

    // função utilizada para alterar o estado do player
    public void SetPlayerState(string state) {
        playerState = state;
    }

    private void Dash() {
        anim.SetTrigger("dash");
    }

    private void MeleeAttack() {
        anim.SetTrigger("meleeAttack");
    }

    private void RangedAttack() {
        anim.SetTrigger("rangedAttack");
        SpawnProjectile(projectilePrefab);
    }

    public void ActiveAnimatorLayer(string layerName) {
        for (int i = 0; i < anim.layerCount; i++) {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(anim.GetLayerIndex(layerName), 1);
    }
}
