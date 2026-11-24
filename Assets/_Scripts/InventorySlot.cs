/*  InventorySlot.cs
    Item slots in inventory.

    Audio file:
        dropSFX

    Reference:
    Big and Crispy. "Inventory System | Unity Tutorial." YouTube.
    https://www.youtube.com/watch?v=-IPjFSWeyrg

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem myItem { get; set; }

    public SlotTag myTag;

    public void OnPointerClick(PointerEventData eventData) {
        if (Inventory.carriedItem == null) {
            return;
        }

        // Slot already contains something
        if (myItem != null) {
            return;
        }

        if (myTag != SlotTag.None && Inventory.carriedItem.myItem.itemTag != myTag) {
            return;
        }

        SetItem(Inventory.carriedItem); 
    }

    public void SetItem(InventoryItem item) {
        Inventory.carriedItem = null;

        // Reset old slot
        if (item.activeSlot != null) {
            item.activeSlot.myItem = null;
        }

        // Set current slot
        myItem = item;
        myItem.activeSlot = this;
        myItem.transform.SetParent(transform);
        myItem.canvasGroup.blocksRaycasts = true;

        if (myTag != SlotTag.None) {
            Inventory.Singleton.EquipEquipment(myTag, myItem);
        }

        // Unsure if you'll add this SFX
        if (SoundManager.Instance != null && Inventory.Singleton.dropSFX != null) {
            SoundManager.Instance.Play(Inventory.Singleton.dropSFX);
        }
    }
}