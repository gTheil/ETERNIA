using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Actor
{
    public string originActor;

    protected override void Start() {
        base.Start();
        currentSpeed = baseSpeed;
    }

    private void FixedUpdate() {
        UpdateMovement(new Vector3(moveX, moveY, 0));
    }

    protected override void UpdateMovement(Vector3 direction) {
        transform.Translate(direction.x * currentSpeed * Time.deltaTime, direction.y * currentSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Hurtbox")) {
            if (col.transform.parent.tag != originActor) {
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
}
