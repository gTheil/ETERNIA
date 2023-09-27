using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    protected BoxCollider2D boxCollider;
    private Vector3 moveDelta; // determina a direção que o ator está tentando se mover
    private RaycastHit2D hit;
    
    public float moveX; // a direção no eixo X que o ator está tentando se mover (-1 = esquerda, 1 = direita)
    public float moveY; // a direção no eixo Y que o ator está tentando se mover (-1 = baixo, 1 = cima)
    protected LayerMask collisionMask; // a lista de layers com os quais o ator deve colidir
    protected Animator anim;

    public float baseSpeed; // a velocidade na qual o ator se move
    public float currentSpeed;
    
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        collisionMask = LayerMask.GetMask("Actor", "Solid"); // determina que o ator irá colidir com outros atores e com elementos sólidos do cenário
        anim = GetComponent<Animator>();
    }

    protected virtual void FixedUpdate()
    {
        SetMoveDelta();

        // caso haja intenção de movimento, chama a função que move o ator
        if (moveDelta.x != 0 || moveDelta.y != 0) {
            Movement();
        } 
    }
    
    protected void SetMoveDelta()
    {
        moveDelta = new Vector3(moveX, moveY, 0);
    }

    protected virtual void Movement() {
        // detecta se há algo com o qual o ator deve colidir na direção (no eixo X) que ele está tentando se mover
        // se houver, o ator não irá se mover nessa eixo
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * currentSpeed * Time.deltaTime), collisionMask);
        if (hit.collider == null) {
            transform.Translate(moveDelta.x * currentSpeed * Time.deltaTime, 0, 0);
        }

        // detecta se há algo com o qual o ator deve colidir na direção (no eixo Y) que ele está tentando se mover
        // se houver, o ator não irá se mover nessa eixo
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * currentSpeed * Time.deltaTime), collisionMask);
        if (hit.collider == null) {
            transform.Translate(0, moveDelta.y * currentSpeed * Time.deltaTime, 0);
        }
    }
}
