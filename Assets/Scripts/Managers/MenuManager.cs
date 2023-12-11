using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;
    public GameObject newGameButton;
    public GameObject returnButton;

    // Start is called before the first frame update
    void Start()
    {
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        returnButton.SetActive(false);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(newGameButton);
    }

    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            if (controlsPanel.activeSelf)
                ShowMenu();
            if (creditsPanel.activeSelf)
                ShowMenu();
        }
    }

    public void ShowMenu() {
        mainMenuPanel.SetActive(true);
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        returnButton.SetActive(false);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(newGameButton);
    }

    public void ShowControls() {
        controlsPanel.SetActive(true);
        returnButton.SetActive(true);
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(false);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(returnButton);
    }

    public void ShowCredits() {
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(true);
        returnButton.SetActive(true);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(returnButton);
    }
}
