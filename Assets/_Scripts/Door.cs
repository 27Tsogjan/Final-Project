/*  Door.cs
    Allows player to open and close doors.

    Audio files:
    doorOpenSFX
    doorCloseSFX
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [Header("Door Settings")]
    public Transform door;
    public float openAngle = 90f;
    public Item requiredKey;


    [Header("Audio")]
    public AudioClip doorOpenSFX;
    public AudioClip doorCloseSFX;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    public FeedbackNotification feedbackNotification;

    private void Start() {
        if (door == null) {
            door = transform;
        }

        closedRotation = door.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
    }

    private void Reset() {
        interactionMessage = "Press E to open";
    }

    public override void Interact() {
        // Needed for final boss door
        if (!isOpen && requiredKey != null && (Inventory.Singleton == null || !Inventory.Singleton.HasItem(requiredKey))) {
            if (feedbackNotification != null) {
                feedbackNotification.ShowMessage("You need a key to open this door.");
            }
            return;
        }

        isOpen = !isOpen;

        if (isOpen) {
            door.localRotation = openRotation;

            if (requiredKey != null && GameManager.Instance != null) {
                GameManager.Instance.currentProgress = GameManager.StoryProgress.ReachFinalBoss;
            }

            if (SoundManager.Instance != null && doorOpenSFX != null) {
                SoundManager.Instance.Play(doorOpenSFX);
            }
        }

        else {
            door.localRotation = closedRotation;

            // Unsure if you will find a sound for door open/close
            if (SoundManager.Instance != null && doorCloseSFX != null) {
                SoundManager.Instance.Play(doorCloseSFX);
            }
        }
    }
}