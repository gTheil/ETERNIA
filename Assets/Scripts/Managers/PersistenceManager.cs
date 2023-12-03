using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistenceManager : MonoBehaviour
{
    public List<string> interactableNames = new List<string>();
    public List<bool> interactableStates = new List<bool>();

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Persistence");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void FindInteractables(Scene scene, LoadSceneMode mode) {
        var interactables = FindObjectsOfType<Interactable>();

        foreach (var i in interactables) {
            if (!interactableNames.Contains(i.gameObject.name)) {
                interactableNames.Add(i.gameObject.name);
                interactableStates.Add(i.state);
            } else {
                int index = interactableNames.IndexOf(i.gameObject.name);

                for (int j = 0; j < interactableStates.Count; j++) {
                    if (j == index) {
                        i.state = interactableStates[j];
                        break;
                    }
                }
            }
        }
    }

    public void SetInteractableState(string name, bool state) {
        int index = interactableNames.IndexOf(name);

        for (int i = 0; i < interactableStates.Count; i++) {
            if (i == index) {
                interactableStates[i] = state;
                break;
            }
        }
    }
}
