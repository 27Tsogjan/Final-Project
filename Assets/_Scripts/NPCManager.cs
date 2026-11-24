/*  NPCManager.cs
    Manages overall dialogue system.
    Dialogue advances by pressing E.

    Reference:
    Redstoneinvente Game Studio. "Unity Dialogue System Tutorial: Linear Story Telling (C#)" YouTube.
    https://www.youtube.com/watch?v=GqzinfLEg9U

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NPCManager : MonoBehaviour
{
    public List<NPCDialogue> dialogues;
    public int currentDialogueIndex = 0;

    private List<NPCDialogue> availableDialogues = new List<NPCDialogue>();

    public NPCDialogue CurrentDialogue {
        get {
            if (availableDialogues.Count == 0) {
                return null;
            }

            return availableDialogues[currentDialogueIndex];
        }
    }

    public void StartDialogue() {
        availableDialogues.Clear();

        if (GameManager.Instance == null) {
            return;
        }

        foreach (NPCDialogue dialogue in dialogues) {
            if (dialogue != null && dialogue.requiredProgress == GameManager.Instance.currentProgress) {
                availableDialogues.Add(dialogue); 
            }
        }

        if (availableDialogues.Count == 0) {
            return;
        }

        currentDialogueIndex = 0;
        DialogueManager.instance.ShowDialogue(this);
    }

    public void MoveNext() {
        currentDialogueIndex++;

        if (currentDialogueIndex >= availableDialogues.Count) {
            currentDialogueIndex = 0;
        }
    }
}