/*  MidBoss.cs
    Allows player to interact with the mid boss.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBoss : NPCInteraction
{
    [Header("Mid-Boss Settings")]
    public Item bossKey;
    private bool isDefeated = false;
    private bool keyAwarded = false;
    public ObjectiveTracker objectiveTracker;


    public override void Interact() {
        base.Interact();

        if (isDefeated) {
            Debug.Log(npcName + ": You have already defeated me.");
            return;
        }

        Debug.Log(npcName + ": Prepare yourself!");
    }
    
    public void DefeatMidBoss() {
        if (isDefeated) {
            return;
        }

        isDefeated = true;

        if (Inventory.Singleton != null && bossKey != null) {
            keyAwarded = Inventory.Singleton.SpawnInventoryItem(bossKey);
        }

        if (GameManager.Instance != null) {
            GameManager.Instance.currentProgress = GameManager.StoryProgress.FindFinalBossDoor;
        }
        
        if (objectiveTracker != null) {
            if (keyAwarded) {
                objectiveTracker.SetObjective("Use the boss key to reach the final boss.");
                Debug.Log("Mid-boss defeated! You received the key.");
            }
            
            else {
                objectiveTracker.SetObjective("Make room in your inventory to receive the boss key.");
                Debug.LogWarning("Mid-boss defeated, but the key has not been awarded yet.");
            }
        }
    }

    public void TryAwardBossKey() {
        if (!isDefeated || keyAwarded || bossKey == null || Inventory.Singleton == null) {
            return;
        }

        if (Inventory.Singleton.SpawnInventoryItem(bossKey)) {
            keyAwarded = true;

            if (objectiveTracker != null) {
                objectiveTracker.SetObjective("Use the boss key to reach the final boss.");
            }

            Debug.Log("You received the boss key!");
        }
    }
}