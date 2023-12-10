using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;

    // Start is called before the first frame update
    void Start()
    {
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void ShowMenu() {
        mainMenuPanel.SetActive(true);
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void ShowControls() {
        controlsPanel.SetActive(true);

        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void ShowCredits() {
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }
}
