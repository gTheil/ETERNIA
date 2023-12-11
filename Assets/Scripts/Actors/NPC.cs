using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Actor
{
    public DialogueSO[] conversation;
    public float interactionDistance;
    public int dialogueSequence;
    public Vector3 startingPosition;
    public float lastMoveStart;
    public float lastMoveEnd;

    public float roamingDistance;
    public float moveDuration;
    public float moveCooldown;

    private Transform playerTransform;
    public bool dialogueInitiated;
    public NPCType npcType;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetActorState("cooldown");
        startingPosition = transform.position;
        playerTransform = GameManager.instance.GetPlayer().transform;
    }

    void Update() {
        if (Vector3.Distance(GameManager.instance.GetPlayer().transform.position, transform.position) <= interactionDistance) {
            if (Input.GetButtonDown("Submit") && !dialogueInitiated && Time.timeScale == 1) {
                GameManager.instance.InitiateDialogue(conversation[dialogueSequence], this);
                dialogueInitiated = true;
                if (anim != null) {
                    anim.SetFloat("x", (playerTransform.position - transform.position).normalized.x);
                    anim.SetFloat("y", (playerTransform.position - transform.position).normalized.y);
                } 
            }
        }
    }

    private void FixedUpdate() {
        if (baseSpeed > 0) {
            if (actorState != "cooldown") {
                anim.SetFloat("x", moveX);
                anim.SetFloat("y", moveY);
                
                if (Vector3.Distance(transform.position, startingPosition) < roamingDistance) {
                    if (actorState != "roaming" && Time.time - lastMoveStart > moveDuration) {
                        SetActorState("roaming");
                        lastMoveStart = Time.time;
                        RandomizeMovement();
                    }
                } else {
                    if (actorState != "return" && Time.time - lastMoveStart > moveDuration) {
                        SetActorState("return");
                        lastMoveStart = Time.time;
                        moveX = Mathf.Round((startingPosition - transform.position).normalized.x);
                        moveY = Mathf.Round((startingPosition - transform.position).normalized.y);
                    }
                }
                HandleMovement();
            } else {
                if (Time.time - lastMoveEnd < moveCooldown)
                    UpdateMovement(Vector3.zero);
                else
                    SetActorState("idle");
            }
        }
        
    }

    private void RandomizeMovement() {
        moveX = Mathf.Round(Random.Range(-1, 1));
        moveY = Mathf.Round(Random.Range(-1, 1));
        while (moveX == 0 && moveY == 0) {
            moveX = Mathf.Round(Random.Range(-1, 1));
            moveY = Mathf.Round(Random.Range(-1, 1));
        }  
    }

    private void HandleMovement() {
        if (Time.time - lastMoveStart < moveDuration) {
            ActiveAnimatorLayer("Walk");
            UpdateMovement(new Vector3(moveX, moveY, 0));
        }
        else {
            ActiveAnimatorLayer("Idle");
            lastMoveEnd = Time.time;
            SetActorState("cooldown");
        }
    }

}

public enum NPCType {
    Blacksmith,
    Alchemist,
    Minor,
};
