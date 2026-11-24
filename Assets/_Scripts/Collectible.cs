/*  Collectible.cs
    Allows player to collect items.

    Audio files:
    collectSFX
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public AudioClip collectSFX;
    public Item item;
    public PickupNotification pickupNotification;

    private void OnTriggerEnter(Collider other) {
        if (Inventory.Singleton == null || item == null)  {
            return;
        }
        
        if (!other.CompareTag("Player")) {
            return;
        }

        // Inventory is full, cannot pick up item
        if (!Inventory.Singleton.SpawnInventoryItem(item)) {
            return;
        }

        if (pickupNotification != null && item != null) {
            pickupNotification.ShowNotification(item.name + " collected!");
        }

        if (collectSFX != null && SoundManager.Instance != null) {
            SoundManager.Instance.Play(collectSFX);
        }

        // Object is picked up
        Destroy(gameObject);
    }
}