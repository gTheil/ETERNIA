using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Actor
{
    protected override void Start() {
        base.Start();
        currentSpeed = baseSpeed;
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
    }

    protected override void Movement() {
        transform.Translate(moveX * currentSpeed * Time.deltaTime, moveY * currentSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Actor")) {
            Debug.Log("Hit " + col.gameObject.name);
            Destroy(gameObject);
        } else if (col.gameObject.layer == LayerMask.NameToLayer("Solid")) {
            Debug.Log("Hit wall");
            Destroy(gameObject);
        }
    }
}
