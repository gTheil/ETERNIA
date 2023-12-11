using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistenceManager : MonoBehaviour
{
    public List<string> interactableNames = new List<string>();
    public List<bool> interactableStates = new List<bool>();
    public List<string> npcNames = new List<string>();
    public List<int> npcSequences = new List<int>();

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Persistence");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        FindInteractables(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        FindNPCs(SceneManager.GetActiveScene(), LoadSceneMode.Single);
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
                        Debug.Log("State set for " + i.gameObject.name + ": " + i.state);
                        break;
                    }
                }
            }
        }
    }

    public void FindNPCs(Scene scene, LoadSceneMode mode) {
        var npcs = FindObjectsOfType<NPC>();

        foreach (var i in npcs) {
            if (!npcNames.Contains(i.gameObject.name)) {
                npcNames.Add(i.gameObject.name);
                npcSequences.Add(i.dialogueSequence);
            } else {
                int index = npcNames.IndexOf(i.gameObject.name);

                for (int j = 0; j < npcSequences.Count; j++) {
                    if (j == index) {
                        i.dialogueSequence = npcSequences[j];
                        Debug.Log("State sequence for " + i.gameObject.name + ": " + i.dialogueSequence);
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

    public void SetDialogueSequence(string name, int sequence) {
        int index = npcNames.IndexOf(name);

        for (int i = 0; i < npcSequences.Count; i++) {
            if (i == index) {
                npcSequences[i] = sequence;
                break;
            }
        }
    }
}
