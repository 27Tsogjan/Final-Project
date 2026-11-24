/*  TreasureChest.cs
    Allows player to open treasure chests.

    Audio files:
    treasureChestOpenSFX
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : Interactable
{
    [Header("Chest Settings")]
    public AudioClip treasureChestOpenSFX;

    private bool isOpen = false;

    public FeedbackNotification feedbackNotification;

    private void Reset() {
        interactionMessage = "Press E to open";
    }


    public override void Interact() {
        // Prevent opening an already opened chest
        if (isOpen) {
            return;
        }

        // Check if inventory exists
        if (Inventory.Singleton == null) {
        if (feedbackNotification != null) {
            feedbackNotification.ShowMessage("Inventory unavailable.");
        }
            return;
        }

        // Add a random item to inventory
        if (Inventory.Singleton.SpawnInventoryItem()) {
            isOpen = true;

            // Play chest opening sound
            if (SoundManager.Instance != null && treasureChestOpenSFX != null) {
                SoundManager.Instance.Play(treasureChestOpenSFX);
            }
        }

        else {
            if (feedbackNotification != null) {
                feedbackNotification.ShowMessage("Your inventory is full.");
            }
        }
    }
}