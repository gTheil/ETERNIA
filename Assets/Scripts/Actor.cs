using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private RaycastHit2D hit;
    
    protected float moveX;
    protected float moveY;

    public float speed;
    
    public virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public virtual void FixedUpdate()
    {
        SetMoveDelta();
        Movement();
    }
    
    protected void SetMoveDelta()
    {
        moveDelta = new Vector3(moveX, moveY, 0);
    }

    protected void Movement() {
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * speed * Time.deltaTime), LayerMask.GetMask("Actor", "Solid"));
        if (hit.collider == null) {
            transform.Translate(moveDelta.x * speed * Time.deltaTime, 0, 0);
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * speed * Time.deltaTime), LayerMask.GetMask("Actor", "Solid"));
        if (hit.collider == null) {
            transform.Translate(0, moveDelta.y * speed * Time.deltaTime, 0);
        }
    }
}
