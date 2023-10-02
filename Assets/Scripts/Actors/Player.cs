using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : CombatActor
{
    public float blockPoint;
    public float blockPointMax;
    public int blockFactor;
    public int blockPushResistance;
    public float blockRecoveryCooldown;
    public float lastBlock;
    public float blockRecoverySpeed;
    public float blockRecoverySpeedBreakOffset;
    public int gold;

    public GameObject meleeAttack;
    public AudioSource meleeAttackSound;
    public AudioSource rangedAttackSound;
    public AudioSource dashSound;

    private Collider2D meleeAttackCollider;
    private List<ActionItem> inputBuffer = new List<ActionItem>(); //The input buffer
    private bool actionAllowed; //set to true whenever we want to process actions from the input buffer, set to false when an action has to wait in the buffer
    private float blockBreakOffset;
    private bool blockBroken;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        meleeAttackCollider = meleeAttack.GetComponent<BoxCollider2D>();

        GameManager.instance.UpdateHealthBar(hitPoint, hitPointMax);
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        // o jogador não pode alterar sua direção de movimento durante a execução do dash
        if (actorState == "idle" || actorState == "block") {
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");
        }
    
        base.FixedUpdate();

        if (anim != null) {
            if (actorState != "dash" && (moveX != 0 || moveY != 0)) {
                anim.SetFloat("x", moveX);
                anim.SetFloat("y", moveY);
                ActiveAnimatorLayer("Walk");
            } else {
                switch (actorState) {
                    case "idle":
                        ActiveAnimatorLayer("Idle");
                        break;
                    case "block":
                        ActiveAnimatorLayer("Block");
                        break;
                    default:
                        break;
                }
            }
        }
        
    }

    private void Update() {
        switch (actorState) {
            case "dash":
                currentSpeed = baseSpeed * 2;
                break;
            case "meleeAttack":
                currentSpeed = baseSpeed / 2;
                break;
            case "rangedAttack":
                currentSpeed = baseSpeed / 2;
                break;
            default:
                currentSpeed = baseSpeed;
                break;
        }

        if (actorState == "dash") {
            collisionMask = LayerMask.GetMask("Solid");
        } else {
            collisionMask = LayerMask.GetMask("Actor", "Solid");
        }

        CheckInput();

        if (actorState == "idle" || actorState == "block")
            actionAllowed = true;

        if (actionAllowed)
            TryBufferedAction();

        GameManager.instance.UpdateBlockBar(blockPoint, blockPointMax);

        if (blockPoint < blockPointMax && Time.time - lastBlock > blockRecoveryCooldown) {
            blockPoint += (blockRecoverySpeed - blockBreakOffset) * Time.deltaTime;
            if (blockPoint >= blockPointMax) {
                blockPoint = blockPointMax;
                blockBreakOffset = 0f;
                blockBroken = false;
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
        if (Input.GetKeyDown(KeyCode.C) && !blockBroken)
            inputBuffer.Add(new ActionItem(ActionItem.InputAction.Block, Time.time));

        // Debug
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        if (Input.GetKeyDown(KeyCode.L)) {
            GameManager.instance.LoadGameData();
        }    
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
            case ActionItem.InputAction.Block:
                ToggleBlock();
                break;
            default:
                break;
        }

        actionAllowed = false;
    }

    private void Dash() {
        anim.SetTrigger("dash");
        dashSound.Play();
    }

    private void MeleeAttack() {
        anim.SetTrigger("meleeAttack");
        meleeAttackSound.Play();
    }

    private void RangedAttack() {
        anim.SetTrigger("rangedAttack");
        SpawnProjectile(projectilePrefab);
        rangedAttackSound.Play();
    }

    private void ToggleBlock() {
        if (actorState == "idle") {
            actorState = "block";
            lastState = "block";
        } else {
            actorState = "idle";
            lastState = "idle";
        }
    }

    protected override void TakeDamage(Damage dmg) {
        switch (actorState) {
            case "dash":
                GameManager.instance.UpdateDebugUI("dodge");
                break;
            case "block":
                lastBlock = Time.time;
                GameManager.instance.UpdateDebugUI("block");
                int blockToHitPoint = dmg.damageAmount - blockFactor;
                if (blockToHitPoint < 1)
                    blockToHitPoint = 1;
                blockPoint -= blockToHitPoint;
                pushDirection = (transform.position - dmg.origin).normalized * (dmg.pushForce - blockPushResistance);

                if (blockPoint <= 0) {
                    blockPoint = 0;
                    BlockBreak();
                }

                break;
            default:
                if (Time.time - lastImmune > immuneTime) {
                    lastImmune = Time.time;
                    hitPoint -= dmg.damageAmount;
                    pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

                    damageSound.Play();
                    GameManager.instance.ShowText(dmg.damageAmount.ToString(), 25, Color.red, transform.position, Vector3.up * 50f, 0.5f);
                    GameManager.instance.UpdateDebugUI("damage");
                    GameManager.instance.UpdateHealthBar(hitPoint, hitPointMax);

                    if (hitPoint <= 0) {
                        hitPoint = 0;
                        Death();
                    }
                
                }
                break;
        } 
    }

    protected virtual void BlockBreak() {
        blockBroken = true;
        blockBreakOffset = blockRecoverySpeedBreakOffset;
        ToggleBlock();
    }

    public void ActiveAnimatorLayer(string layerName) {
        for (int i = 0; i < anim.layerCount; i++) {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(anim.GetLayerIndex(layerName), 1);
    }
}
