using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warp : MonoBehaviour
{
    public string sceneToLoad;
    public float warpPositionX;
    public float warpPositionY;

    private void OnTriggerEnter2D (Collider2D col) {
        if (col.gameObject.name == "Player") {
            GameManager.instance.SetNewScene(sceneToLoad, warpPositionX, warpPositionY);
            GameManager.instance.SaveGameState();
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        }
    }
}
