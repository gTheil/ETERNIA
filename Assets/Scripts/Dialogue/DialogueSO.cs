using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueSO : ScriptableObject
{
    public DialogueActor[] actors;
    public bool advanceSequence;
    public bool returnFromBranch;
    public bool openShop;

    [Tooltip("Only fill if Minor is selected as Actor Name")]
    [Header("Minor NPC Info")]
    public string minorActorName;
    public Sprite minorActorPortrait;

    [Header("Dialogue")]
    [TextArea]
    public string[] dialogue;

    [Tooltip("Dialogue Choices")]
    public string[] optionText;

    public DialogueSO option0;
    public DialogueSO option1;
    public DialogueSO option2;
    public DialogueSO option3;
}
