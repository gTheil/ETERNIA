using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Actor
{
    public Transform originActor;

    protected override void Start() {
        base.Start();
    }

    private void FixedUpdate() {
        UpdateMovement(new Vector3(moveX, moveY, 0));
        anim.SetFloat("x", moveX);
        anim.SetFloat("y", moveY);
    }

    protected override void UpdateMovement(Vector3 input) {
        moveDelta = new Vector3(input.x, input.y, 0).normalized;
        transform.Translate(moveDelta.x * currentSpeed * Time.deltaTime, moveDelta.y * currentSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Hurtbox")) {
            if (col.transform.parent.tag != originActor.gameObject.tag) {
                Debug.Log("Hit " + col.gameObject.name);
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                Destroy(gameObject, 1f);
            }
        } else if (col.gameObject.layer == LayerMask.NameToLayer("Solid")) {
            Debug.Log("Hit wall");
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
