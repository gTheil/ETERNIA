using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player")
            GameManager.instance.RestoreHealth(GameManager.instance.GetHitPointMax());
    }
}
