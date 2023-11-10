using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : CombatActor
{
    public int baseDamage;
    public int defense;
    public float blockPoint;
    public float blockPointMax;
    public int blockFactor;
    public float blockPushResistance;
    public float blockRecoveryCooldown;
    public float lastBlock;
    public float blockRecoverySpeed;
    public float blockRecoverySpeedBreakOffset;
    public int gold;
    public int attackBonus;
    public int defenseBonus;

    public GameObject meleeAttack;
    public AudioSource dashSound;
    public EquipmentSO equippedSword, equippedBow, equippedShield;

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
        blockPoint = equippedShield.shieldBlockPoint;
        UpdateStats();
        GameManager.instance.UpdateHealthBar(hitPoint, hitPointMax);
        GameManager.instance.UpdateStatsUI(hitPoint, hitPointMax, blockPoint, blockPointMax, meleeDamage, rangedDamage, blockFactor);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        // o jogador não pode alterar sua direção de movimento durante a execução do dash
        if (actorState == "idle" || actorState == "block") {
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");
        }

        if (Time.time - lastImmune > immuneTime)
            UpdateMovement(new Vector3(moveX, moveY, 0));
        else
            UpdateMovement(Vector3.zero);

        if (anim != null) {
            if (actorState != "dash" && (moveX != 0 || moveY != 0) && Time.time - lastImmune > immuneTime) {
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

        UpdateStats();

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

        if (blockPoint < blockPointMax && Time.time - lastBlock > blockRecoveryCooldown) {
            blockPoint += (blockRecoverySpeed - blockBreakOffset) * Time.deltaTime;
            GameManager.instance.UpdateBlockBar(blockPoint, blockPointMax);
            GameManager.instance.UpdateSingleStat("block", blockPoint, blockPointMax);
            if (blockPoint >= blockPointMax) {
                blockPoint = blockPointMax;
                blockBreakOffset = 0f;
                blockBroken = false;
            }
        }
    }

    private void CheckInput() {
        if (Input.GetButtonDown("Dash") && (moveX != 0 || moveY != 0))
            inputBuffer.Add(new ActionItem(ActionItem.InputAction.Dash, Time.time));
        if (Input.GetButtonDown("Melee"))
            inputBuffer.Add(new ActionItem(ActionItem.InputAction.MeleeAttack, Time.time));
        if (Input.GetButtonDown("Ranged"))
            inputBuffer.Add(new ActionItem(ActionItem.InputAction.RangedAttack, Time.time));
        if (Input.GetButtonDown("Block") && !blockBroken)
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

    private void ToggleBlock() {
        if (actorState == "idle") {
            actorState = "block";
            lastState = "block";
        } else {
            actorState = "idle";
            lastState = "idle";
        }
    }

    protected override void MeleeAttack() {
        base.MeleeAttack();
        meleeAttackSound.Play();
    }

    protected override void RangedAttack() {
        anim.SetTrigger("rangedAttack");
        SpawnProjectile(projectilePrefab, anim.GetFloat("x"), anim.GetFloat("y"), transform, projectileSpeed);
        rangedAttackSound.Play();
    }

    protected override void TakeDamage(Damage dmg) {
        switch (actorState) {
            case "dash":
                GameManager.instance.UpdateDebugUI("dodge");
                break;
            case "block":
                lastBlock = Time.time;
                int damageToBlock = (dmg.damageAmount - defense) - blockFactor;
                if (damageToBlock < 0)
                    damageToBlock = 0;
                else if (damageToBlock > 0) {
                    blockPoint -= damageToBlock;

                    GameManager.instance.ShowText(damageToBlock.ToString(), 52, Color.black, transform.position, Vector3.up * 50f, 0.5f);
                    GameManager.instance.UpdateDebugUI("block");
                    GameManager.instance.UpdateBlockBar(blockPoint, blockPointMax);
                    GameManager.instance.UpdateStatsUI(hitPoint, hitPointMax, blockPoint, blockPointMax, meleeDamage, rangedDamage, blockFactor);
                }

                if (dmg.pushForce - blockPushResistance > 0f)
                        pushDirection = (transform.position - dmg.origin).normalized * (dmg.pushForce - blockPushResistance);
                    
                if (blockPoint <= 0) {
                    blockPoint = 0;
                    BlockBreak();
                }
                break;
            default:
                if (dmg.damageAmount - defense > 0) {
                    if (Time.time - lastImmune > immuneTime) {
                        lastImmune = Time.time;
                        int damageDealt = dmg.damageAmount - defense;
                        hitPoint -= damageDealt;
                        if (!pushImmune)
                            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

                        damageSound.Play();
                        GameManager.instance.ShowText(damageDealt.ToString(), 52, Color.red, transform.position, Vector3.up * 50f, 0.5f);
                        GameManager.instance.UpdateDebugUI("damage");
                        GameManager.instance.UpdateHealthBar(hitPoint, hitPointMax);
                        GameManager.instance.UpdateStatsUI(hitPoint, hitPointMax, blockPoint, blockPointMax, meleeDamage, rangedDamage, blockFactor);

                        if (hitPoint <= 0) {
                            hitPoint = 0;
                            Death();
                        }
                    }
                }
                break;
        } 
    }

    public void RestoreHealth(int amount) {
        hitPoint += amount;
        if (hitPoint > hitPointMax)
            hitPoint = hitPointMax;
        GameManager.instance.UpdateStatsUI(hitPoint, hitPointMax, blockPoint, blockPointMax, meleeDamage, rangedDamage, blockFactor);
    }

    public void IncreaseStat(string stat, int amount, float duration) {
        switch (stat) {
            case "attack":
                attackBonus = amount;
                break;
            case "defense":
                defenseBonus = amount;
                break;
            case "pushImmune":
                pushImmune = true;
                break;
            default:
                break;
        }
        StartCoroutine(OnBuffActive(stat, duration));
        GameManager.instance.UpdateStatsUI(hitPoint, hitPointMax, blockPoint, blockPointMax, meleeDamage, rangedDamage, blockFactor);
    }

    public EquipmentSO GetEquipment(EquipType equipType) {
        EquipmentSO equipment = ScriptableObject.CreateInstance<EquipmentSO>();
        switch (equipType) {
            case EquipType.sword:
                if (equippedSword != null)
                    equipment = equippedSword;
                else
                    equipment = null;
                break;
            case EquipType.bow:
                if (equippedBow != null)
                    equipment = equippedBow;
                else
                    equipment = null;
                break;
            case EquipType.shield:
                if (equippedShield != null)
                    equipment = equippedShield;
                else
                    equipment = null;
                break;
            default:
                break;
        }
        return equipment;
    }

    public void SetEquipment(EquipmentSO equip) {
        switch (equip.equipType) {
            case EquipType.sword:
                equippedSword = equip;
                break;
            case EquipType.bow:
                equippedBow = equip;
                break;
            case EquipType.shield:
                equippedShield = equip;
                break;
            default:
                break;
        }
        UpdateStats();
        if (blockPoint > blockPointMax)
            blockPoint = blockPointMax;
        GameManager.instance.UpdateStatsUI(hitPoint, hitPointMax, blockPoint, blockPointMax, meleeDamage, rangedDamage, blockFactor);
    }

    protected override void Death() {
        GetComponent<SpriteRenderer>().enabled = false;
        hitbox.enabled = false;
        StartCoroutine(ReloadScene());
    }

    protected virtual void BlockBreak() {
        blockBroken = true;
        blockBreakOffset = blockRecoverySpeedBreakOffset;
        ToggleBlock();
    }

    IEnumerator OnBuffActive(string stat, float duration) {
        yield return new WaitForSeconds(duration);
        if (stat == "attack")
            DeactivateAttackBuff();
        else if (stat == "defense")
            DeactivateDefenseBuff();
        else if (stat == "pushImmune")
            DeactivatePushImmunity();
    }

    private void DeactivateAttackBuff() {
        attackBonus = 0; 
        GameManager.instance.UpdateStatsUI(hitPoint, hitPointMax, blockPoint, blockPointMax, meleeDamage, rangedDamage, blockFactor);
    }

    private void DeactivateDefenseBuff() {
        defenseBonus = 0;
        GameManager.instance.UpdateStatsUI(hitPoint, hitPointMax, blockPoint, blockPointMax, meleeDamage, rangedDamage, blockFactor);
    }

    private void DeactivatePushImmunity() {
        pushImmune = false;
        GameManager.instance.UpdateStatsUI(hitPoint, hitPointMax, blockPoint, blockPointMax, meleeDamage, rangedDamage, blockFactor);
    }

    private void UpdateStats() {
        meleeDamage = baseDamage + attackBonus + equippedSword.swordAtk;
        meleePush = equippedSword.atkPush;

        rangedDamage = baseDamage + attackBonus + equippedBow.bowAtk;
        rangedPush = equippedBow.atkPush;
        projectileSpeed = equippedBow.projectileSpeed;

        defense = baseDefense + defenseBonus;
        blockFactor = equippedShield.shieldDef;
        blockPointMax = equippedShield.shieldBlockPoint;
        blockRecoverySpeed = equippedShield.shieldRecoverySpeed;
        blockRecoveryCooldown = equippedShield.shieldRecoveryCooldown;
        blockPushResistance = equippedShield.shieldPushResistance;
        blockRecoverySpeedBreakOffset = equippedShield.shieldBreakOffset;
    }

    IEnumerator ReloadScene() {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    // Getters

    public int GetHitPoint() {
        return hitPoint;
    }

    public int GetHitPointMax() {
        return hitPointMax;
    }

    public float GetBlockPoint() {
        return blockPoint;
    }

    public float GetBlockPointMax() {
        return blockPointMax;
    }

    public int GetMeleeDamage() {
        return meleeDamage;
    }

    public int GetRangedDamage() {
        return rangedDamage;
    }

    public int GetBlockFactor() {
        return blockFactor;
    }
}
