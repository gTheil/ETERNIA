using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameObject controlsCanvas;
    public GameObject pauseCanvas;
    public GameObject saveButton;
    public GameObject returnButton;

    void Start() {
        controlsCanvas = GameObject.Find("ControlsCanvas");
        pauseCanvas = GameObject.Find("PauseCanvas");

        if (controlsCanvas != null)
            controlsCanvas.SetActive(false);
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);
    }

    void Update() {
        if (pauseCanvas != null && controlsCanvas != null) {
            if (Input.GetButtonDown("Pause") || (Input.GetButtonDown("Cancel") && pauseCanvas.activeSelf) || (Input.GetButtonDown("Cancel") && controlsCanvas.activeSelf))
                PauseMenu();
        }
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
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(returnButton);
    }

    public void PauseMenu() {
        if (!GameManager.instance.IsUIOpen() && !GameManager.instance.IsDialogueActive()) {
            if (Time.timeScale == 1) {
                pauseCanvas.SetActive(true);
                EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(saveButton);
                Time.timeScale = 0;
            }
            else {
                if (!controlsCanvas.activeSelf) {
                    pauseCanvas.SetActive(false);
                    Time.timeScale = 1;
                } else {
                    controlsCanvas.SetActive(false);
                    pauseCanvas.SetActive(true);
                    EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(saveButton);
                }
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
