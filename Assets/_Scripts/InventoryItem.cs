/*  InventoryItem.cs
    Individual item in inventory.

    Reference:
    Big and Crispy. "Inventory System | Unity Tutorial." YouTube.
    https://www.youtube.com/watch?v=-IPjFSWeyrg

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    Image itemIcon;
    public CanvasGroup canvasGroup { get; private set;}

    public Item myItem { get; set; }
    public InventorySlot activeSlot { get; set; }

    void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        itemIcon = GetComponent<Image>();
    }

    public void Initialize(Item item, InventorySlot parent) {
        activeSlot = parent;
        activeSlot.myItem = this;
        myItem = item;
        itemIcon.sprite = item.sprite;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            Inventory.Singleton.SetCarriedItem(this);
        }

        else if (eventData.button == PointerEventData.InputButton.Right) {
            UseItem();
        }
    }

    private void UseItem() {
        if (myItem == null || activeSlot == null) {
            return;
        }

        if (myItem.itemType == ItemType.HealthPotion) {
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

            if (playerHealth != null && playerHealth.RestoreHealth(myItem.healthRestore)) {
                activeSlot.myItem = null;

                if (Inventory.Singleton != null) {
                    MidBoss midBoss = FindObjectOfType<MidBoss>();

                    if (midBoss != null) {
                        midBoss.TryAwardBossKey();
                    }
                }

                Destroy(gameObject);
            }
        }

    }
}
