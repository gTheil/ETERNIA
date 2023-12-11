using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public List<ActorSO> scriptableActors = new List<ActorSO>();

    private DialogueSO currentConversation;
    private int stepNum;
    public bool dialogueActive;

    private GameObject dialogueCanvas;
    private TMP_Text actorName;
    private Image actorPortrait;
    private TMP_Text dialogueText;

    private string currentSpeaker;
    private Sprite currentPortrait;

    public GameObject[] optionButtons;
    private TMP_Text[] optionButtonText;
    private GameObject optionsPanel;

    private NPC initiatedBy;

    [SerializeField]
    private float typeSpeed = 0.02f;
    private Coroutine typewriterRoutine;
    private bool canContinueText = true;

    private DialogueSO startingConversation;
    private int stepToReturn;

    // Start is called before the first frame update
    void Start()
    {
        optionsPanel = GameObject.Find("OptionsPanel");
        //optionButtons = GameObject.FindGameObjectsWithTag("OptionButton");
        optionButtonText = new TMP_Text[optionButtons.Length];

        for (int i = 0; i < optionButtons.Length; i++) {
            optionButtonText[i] = optionButtons[i].GetComponentInChildren<TMP_Text>();
            optionButtons[i].SetActive(false);
        }

        dialogueCanvas = GameObject.Find("DialogueCanvas");
        actorName = GameObject.Find("ActorText").GetComponent<TMP_Text>();
        actorPortrait = GameObject.Find("Portrait").GetComponent<Image>();
        dialogueText = GameObject.Find("DialogueText").GetComponent<TMP_Text>();

        dialogueCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueActive && Input.GetButtonUp("Submit") && canContinueText) {
            if (stepNum >= currentConversation.actors.Length)
                EndDialogue();
            else
                PlayDialogue();
        }
    }

    void PlayDialogue() {
        if (currentConversation.actors[stepNum] == DialogueActor.Minor)
            SetActorInfo(false);
        else
            SetActorInfo(true);

        actorName.text = currentSpeaker;
        actorPortrait.sprite = currentPortrait;

        if (currentConversation.actors[stepNum] == DialogueActor.Branch) {
            for (int i = 0; i < currentConversation.optionText.Length; i++) {
                if (currentConversation.optionText[i] == null)
                    optionButtons[i].SetActive(false);
                else {
                    optionButtonText[i].text = currentConversation.optionText[i];
                    optionButtons[i].SetActive(true);
                }
            }
            optionsPanel.SetActive(true);
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(optionButtons[0]);
        }

        if (typewriterRoutine != null)
            StopCoroutine(typewriterRoutine);

        if (stepNum < currentConversation.dialogue.Length && currentConversation.actors[stepNum] != DialogueActor.Branch)
            typewriterRoutine = StartCoroutine(TypewriterEffect(dialogueText.text = currentConversation.dialogue[stepNum]));
        else {
            optionsPanel.SetActive(true);
        }

        dialogueCanvas.SetActive(true);
        stepNum += 1;
    }

    void SetActorInfo(bool majorCharacter) {
        if (majorCharacter) {
            for (int i = 0; i < scriptableActors.Count; i++) {
                if (scriptableActors[i].actorName == currentConversation.actors[stepNum].ToString()) {
                    currentSpeaker = scriptableActors[i].actorName;
                    currentPortrait = scriptableActors[i].actorPortrait;
                }
            }
        } else {
            currentSpeaker = currentConversation.minorActorName;
            currentPortrait = currentConversation.minorActorPortrait;
        }
    }

    public void Option(int optionNum) {
        if (canContinueText) {
            foreach (GameObject button in optionButtons)
                button.SetActive(false);

            switch (optionNum) {
                case 0:
                    if (currentConversation.option0.returnFromBranch) {
                        startingConversation = currentConversation;
                        stepToReturn = stepNum;
                    }    
                    currentConversation = currentConversation.option0;
                    break;
                case 1:
                    if (currentConversation.option1.returnFromBranch) {
                        startingConversation = currentConversation;
                        stepToReturn = stepNum;
                    }
                    currentConversation = currentConversation.option1;
                    break;
                case 2:
                    if (currentConversation.option2.returnFromBranch) {
                        startingConversation = currentConversation;
                        stepToReturn = stepNum;
                    }
                    currentConversation = currentConversation.option2;
                    break;
                case 3:
                    if (currentConversation.option3.returnFromBranch) {
                        startingConversation = currentConversation;
                        stepToReturn = stepNum;
                    }
                    currentConversation = currentConversation.option3;
                    break;
                default:
                    break;
            }
            stepNum = 0;
        }
    }

    private IEnumerator TypewriterEffect(string line) {
        dialogueText.text = "";
        canContinueText = false;
        bool richTextTag = false;
        yield return new WaitForSecondsRealtime(.2f);

        foreach (char letter in line.ToCharArray()) {
            if (Input.GetButtonDown("Submit")) {
                dialogueText.text = line;
                break;
            }
            if (letter == '<' || richTextTag) {
                richTextTag = true;
                dialogueText.text += letter;
                if (letter == '>')
                    richTextTag = false;
            } else {
                dialogueText.text += letter;
                yield return new WaitForSecondsRealtime(typeSpeed);
            }
        }
        canContinueText = true;
    }

    public void InitiateDialogue(DialogueSO conversation, NPC npc) {
        currentConversation = conversation;
        initiatedBy = npc;

        dialogueActive = true;
        Time.timeScale = 0;
    }

    public void EndDialogue() {
        if (startingConversation == null) {
            stepNum = 0;
            initiatedBy.dialogueInitiated = false;

            dialogueActive = false;
            optionsPanel.SetActive(false);
            dialogueCanvas.SetActive(false);
            Time.timeScale = 1;

            if (currentConversation.openShop) {
                if (initiatedBy.npcType == NPCType.Blacksmith)
                    GameManager.instance.Shop("equipment");
                else if (initiatedBy.npcType == NPCType.Alchemist)
                    GameManager.instance.Shop("consumable");
            }

        } else {
            currentConversation = startingConversation;
            stepNum = stepToReturn;
            startingConversation = null;
            stepToReturn = 0;
            PlayDialogue();
        }
        if (currentConversation.advanceSequence) {
            initiatedBy.dialogueSequence += 1;
            GameManager.instance.SetDialogueSequence(initiatedBy.gameObject.name, initiatedBy.dialogueSequence);
        }
    }
}

public enum DialogueActor {
    Player,
    Elder,
    Blacksmith,
    Alchemist,
    Minor,
    Branch
};