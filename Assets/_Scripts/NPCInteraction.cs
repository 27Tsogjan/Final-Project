/*  NPCInteraction.cs
    Handles basic NPC interactions (by pressing E).
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : Interactable
{
    [Header("NPC Settings")]
    public string npcName;

    [TextArea]
    public string dialogue;

    public NPCManager npcManager;

    private void Reset() {
        interactionMessage = "Press E to talk";
    }

    public override void Interact() {
        if (npcManager != null) {
            npcManager.StartDialogue();
        }
    }
}