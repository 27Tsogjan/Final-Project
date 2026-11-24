/*  MimicInteraction.cs
    Connects the mimic to player's interaction system.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicInteraction : Interactable {
    private Mimic mimic;

    private void Start() {
        mimic = GetComponent<Mimic>();
        interactionMessage = "Press E to open";

        if (mimic == null) {
            Debug.LogError("MimicInteraction requires a Mimic component!", this);
        }
    }

    public override void Interact() {
        if (mimic != null) {
            mimic.ActivateMimic();
        }
    }
}