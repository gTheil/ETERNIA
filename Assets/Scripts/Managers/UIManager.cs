using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject controlsCanvas;
    public GameObject pauseCanvas;

    void Start() {
        controlsCanvas = GameObject.Find("ControlsCanvas");
        pauseCanvas = GameObject.Find("PauseCanvas");

        if (controlsCanvas != null)
            controlsCanvas.SetActive(false);
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);
    }

    public void NewGame() {
        GameManager.instance.SetNewGame(true);
        Time.timeScale = 1;
        SceneManager.LoadScene("PlayerTestScene", LoadSceneMode.Single);
    }

    public void SaveGame() {
        GameManager.instance.SetNewScene(SceneManager.GetActiveScene().name, GameManager.instance.GetPlayer().transform.position.x, GameManager.instance.GetPlayer().transform.position.y);
        GameManager.instance.SaveGameData();
    }

    public void LoadGame() {
        GameManager.instance.SetNewGame(false);
        Time.timeScale = 1;
        GameManager.instance.LoadGameData();
    }

    public void ShowControls() {
        controlsCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
    }

    public void PauseMenu() {
        if (Time.timeScale == 1 && !GameManager.instance.IsUIOpen()) {
            pauseCanvas.SetActive(true);
            Time.timeScale = 0;
        }
        else {
            if (!controlsCanvas.activeSelf) {
                pauseCanvas.SetActive(false);
                Time.timeScale = 1;
            } else {
                controlsCanvas.SetActive(false);
                pauseCanvas.SetActive(true);
            }
        }
    }

    public void MainMenu() {
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
