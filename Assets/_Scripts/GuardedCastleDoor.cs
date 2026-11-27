using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardedCastleDoor : Interactable
{
    [Header("Guards")]
    public Enemy[] guards;
    public string lockedMessage = "Defeat the guards first.";

    [Header("Next Scene")]
    public int castleSceneIndex = 5;
    public AudioClip doorOpenSFX;
    public FeedbackNotification feedbackNotification;

    private void Reset() {
        interactionMessage = "Press E to enter";
    }

    public override void Interact() {
        Debug.Log("Door interacted. Guards remaining: " + GuardsRemaining());

        if (GuardsRemaining()) {
            if (feedbackNotification != null) {
                feedbackNotification.ShowMessage(lockedMessage);
            }
            return;
        }

        if (SoundManager.Instance != null && doorOpenSFX != null) {
            SoundManager.Instance.Play(doorOpenSFX);
        }

        SceneManager.LoadScene(castleSceneIndex);
    }

    private bool GuardsRemaining() {
        foreach (Enemy guard in guards) {
            if (guard != null && guard.Health > 0) {
                return true;
            }
        }
        return false;
    }
}