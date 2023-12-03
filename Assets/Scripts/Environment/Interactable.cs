using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour
{
    public float interactionDistance;
    public bool state;

    protected Collider2D col;

    protected virtual void Start() {
        col = GetComponent<Collider2D>();
    }

    void Update() {
        if (Input.GetButtonDown("Submit") && interactionDistance > 0f) {
            if (Vector3.Distance(GameManager.instance.GetPlayer().transform.position, transform.position) <= interactionDistance) {
                Interact();
            }
        }
    }

    public virtual void Interact() {
        Debug.Log(gameObject.name);
        GameManager.instance.SetInteractableState(gameObject.name, state);
    }
}
