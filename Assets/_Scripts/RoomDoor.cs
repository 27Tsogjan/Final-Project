using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorSide { Left, Right, Top, Bottom }

public class RoomDoor : Interactable
{
    public DoorSide side = DoorSide.Right;
    public AudioClip doorSFX;

    private void Reset() {
        interactionMessage = "Press E to enter";
    }

    public override void Interact() {
        if (RoomManager.Instance == null) {
            Debug.LogWarning("RoomDoor: no RoomManager in the scene.");
            return;
        }

        if (SoundManager.Instance != null && doorSFX != null) {
            SoundManager.Instance.Play(doorSFX);
        }

        RoomManager.Instance.NextRoom(side);
    }
}
