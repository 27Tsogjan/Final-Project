/*  FinalBoss.cs
    Allows player to interact with the final boss.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : NPCInteraction
{
    [Header("Final Boss Settings")]
    public Item requiredKey;
    public ObjectiveTracker objectiveTracker;

    private bool encounterStarted = false;
    private bool isDefeated = false;

    public override void Interact() {
        base.Interact();

        if (encounterStarted) {
            return;
        }

        encounterStarted = true;

        if (objectiveTracker != null) {
            objectiveTracker.SetObjective("Vanquish the final boss.");
        }

        Debug.Log(npcName + ": So, you've made it this far. Prepare yourself!");
    }

    public void DefeatBoss() {
        if (isDefeated) {
            return;
        }

        isDefeated = true;

        if (GameManager.Instance != null) {
            GameManager.Instance.currentProgress = GameManager.StoryProgress.DefeatFinalBoss;
            GameManager.Instance.WinGame();
        }

        Debug.Log("Final boss defeated!");
    }

}