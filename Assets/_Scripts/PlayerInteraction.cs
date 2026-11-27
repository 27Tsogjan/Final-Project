/*  PlayerInteraction.cs
    Allows player to interact with objects by pressing E.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private readonly Dictionary<Interactable, int> nearbyInteractables = new Dictionary<Interactable, int>();
    public InteractionPrompt interactionPrompt;

    private void Update() {
        RemoveInvalidInteractables();

        if (Input.GetKeyDown(KeyCode.E))
{
            Debug.Log("E pressed. Nearby interactables: " + nearbyInteractables.Count);
            Interactable closest = GetClosestInteractable();

            if (closest != null) {
                Debug.Log("Closest interactable: " + closest.name);
                closest.Interact();
            }
        }

        UpdatePrompt();
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Hero touched trigger: " + other.name);
        Interactable interactable = other.GetComponentInParent<Interactable>();

        if (interactable == null) {
            return;
        }

        if (nearbyInteractables.ContainsKey(interactable)) {
            nearbyInteractables[interactable]++;
        }

        else {
            nearbyInteractables.Add(interactable, 1);
        }

        UpdatePrompt();
    }

    private void OnTriggerExit(Collider other) {
        Interactable interactable = other.GetComponentInParent<Interactable>();

        if (interactable == null || !nearbyInteractables.ContainsKey(interactable)) {
            return;
        }

        nearbyInteractables[interactable]--;

        if (nearbyInteractables[interactable] <= 0) {
            nearbyInteractables.Remove(interactable);
        }

        UpdatePrompt();
    }

    private Interactable GetClosestInteractable() {
        Interactable closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Interactable interactable in nearbyInteractables.Keys) {
            if (interactable == null) {
                continue;
            }

            float distance = Vector3.Distance(transform.position, interactable.transform.position);

            if (distance < closestDistance) {
                closestDistance = distance;
                closest = interactable;
            }
        }

        return closest;
    }

    private void RemoveInvalidInteractables() {
        List<Interactable> invalid = new List<Interactable>();

        foreach (Interactable interactable in nearbyInteractables.Keys) {
            if (interactable == null) {
                invalid.Add(interactable);
            }
        }

        foreach (Interactable interactable in invalid) {
            nearbyInteractables.Remove(interactable);
        }
    }

    private void UpdatePrompt() {
        if (interactionPrompt == null) {
            return;
        }

        Interactable closest = GetClosestInteractable();

        if (closest != null) {
            interactionPrompt.ShowPrompt(closest.interactionMessage);
        }

        else {
            interactionPrompt.HidePrompt();
        }
    }
}