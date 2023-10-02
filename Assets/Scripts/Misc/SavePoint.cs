using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : MonoBehaviour
{
    private void OnTriggerEnter2D (Collider2D col) {
        if (col.gameObject.name == "Player") {
            GameManager.instance.SetNewScene(SceneManager.GetActiveScene().name, transform.position.x, transform.position.y);
            GameManager.instance.SaveGameData();
        }
    }
}
