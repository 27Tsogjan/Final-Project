/*  Inventory.cs
    Item inventory management.

    Audio files:
        dragSFX
        dropSFX
        equipSFX
        unequipSFX

    Reference:
    Big and Crispy. "Inventory System | Unity Tutorial." YouTube.
    https://www.youtube.com/watch?v=-IPjFSWeyrg

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;

    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;

    [Header("Item List")]
    [SerializeField] Item [] items;

    [Header("Debug")]
    [SerializeField] Button giveItemBtn;

    [Header("Inventory Audio")]
    public AudioClip equipSFX;
    public AudioClip unequipSFX;
    public AudioClip dragSFX;
    public AudioClip dropSFX; 

    [Header("Equipment")]
    public Transform headEquipmentPoint;
    public Transform chestEquipmentPoint;
    public Transform shieldEquipmentPoint;

    private GameObject equippedHead;
    private GameObject equippedChest;
    private GameObject equippedShield;

    private Transform equipmentPoint;
    private GameObject equippedObject;

    public int totalDefense;
    public int totalAttack;
    public int totalHealth;

    private Item equippedHeadItem;
    private Item equippedChestItem;
    private Item equippedShieldItem;

    void Awake() {
        Singleton = this;
        
        if (giveItemBtn != null) {
            giveItemBtn.onClick.AddListener(delegate { SpawnInventoryItem(); });
        }
    }

    public bool SpawnInventoryItem(Item item = null) {
        Item _item = item;
        if (_item == null) {
            if (items == null || items.Length == 0) {
                Debug.LogWarning("No items assigned to the inventory item list.");
                return false;
            }
            
            int random = Random.Range(0, items.Length);
            _item = items[random];
        }

        for (int i = 0; i < inventorySlots.Length; i++) {
            // Check if the slot is empty
            if (inventorySlots[i].myItem == null && inventorySlots[i].myTag == SlotTag.None) {
                Instantiate(itemPrefab, inventorySlots[i].transform).Initialize(_item, inventorySlots[i]);
                return true;
            }
        }

        return false;
    }

    void Update() {
        if (carriedItem != null) {
            carriedItem.transform.position = Input.mousePosition;
        }
    }

    public void SetCarriedItem(InventoryItem item) {
        InventorySlot oldSlot = item.activeSlot;
        InventoryItem swapItem = carriedItem;

        // Carried item must be allowed in the slot we're swapping with
        if (swapItem != null && oldSlot.myTag != SlotTag.None &&
            oldSlot.myTag != swapItem.myItem.itemTag) {
            return;
        }

        // Picking up from an equipment slot unequips it
        if (oldSlot.myTag != SlotTag.None) {
            EquipEquipment(oldSlot.myTag, null);
        }

        // Detach the clicked item from its slot
        oldSlot.myItem = null;
        item.activeSlot = null;

        // Drop the previously carried item into the vacated slot (equips it if needed)
        if (swapItem != null) {
            oldSlot.SetItem(swapItem);   // sets carriedItem to null
        }

        if (SoundManager.Instance != null && dragSFX != null) {
            SoundManager.Instance.Play(dragSFX);
        }

        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform);
    }

    public void EquipEquipment(SlotTag tag, InventoryItem item = null) {
        if (tag == SlotTag.Head) {
            if (item != null) {
                equippedHeadItem = item.myItem;
            }
            else {
                equippedHeadItem = null;
            }
        }

        if (tag == SlotTag.Chest) {
            if (item != null) {
                equippedChestItem = item.myItem;
            }
            else {
                equippedChestItem = null;
            }
        }

        if (tag == SlotTag.Shield) {
            if (item != null) {
                equippedShieldItem = item.myItem;
            }
            else {
                equippedShieldItem = null;
            }
        }

        switch (tag) {
            case SlotTag.Head:
                equipmentPoint = headEquipmentPoint;
                equippedObject = equippedHead;
                break;

            case SlotTag.Chest:
                equipmentPoint = chestEquipmentPoint;
                equippedObject = equippedChest;
                break;

            case SlotTag.Shield:
                equipmentPoint = shieldEquipmentPoint;
                equippedObject = equippedShield;
                break;

            default:
                return;
        }

        if (equipmentPoint == null) {
            Debug.LogWarning("No equipment attachment point assigned for " + tag);
            return;
        }

        if (equippedObject != null) {
            Destroy(equippedObject);
        }

        if (item == null || item.myItem.equipmentPrefab == null) {
            if (tag == SlotTag.Head) {
                equippedHead = null;
            }

            if (tag == SlotTag.Chest) {
                equippedChest = null;
            }

            if (tag == SlotTag.Shield) {
                equippedShield = null;
            }

            UpdateEquipmentStats();

            // Unsure if you will add these SFX
            if (item == null && SoundManager.Instance != null && unequipSFX != null) {
                SoundManager.Instance.Play(unequipSFX);
            }

            return;
        }

        GameObject newEquipment = Instantiate(item.myItem.equipmentPrefab, equipmentPoint.position, equipmentPoint.rotation, equipmentPoint);
        if (tag == SlotTag.Head) {
            equippedHead = newEquipment;
        }

        if (tag == SlotTag.Chest) {
            equippedChest = newEquipment;
        }

        if (tag == SlotTag.Shield) {
            equippedShield = newEquipment;
        }

        UpdateEquipmentStats();

        if (SoundManager.Instance != null && equipSFX != null) {
            SoundManager.Instance.Play(equipSFX);
        }
        
        Debug.Log("Equipped " + item.myItem.name + " on " + tag);
    }

    // Check if player has a specific item
    public bool HasItem(Item item) {
        if (carriedItem != null && carriedItem.myItem == item) {
            return true;
        }

        foreach (InventorySlot slot in inventorySlots) {
            if (slot.myItem != null && slot.myItem.myItem == item) {
                return true;
            }
        }

        return false;
    }

    public void UpdateEquipmentStats() {
        totalDefense = 0;
        totalAttack = 0;
        totalHealth = 0;

        AddStats(equippedHeadItem);
        AddStats(equippedChestItem);
        AddStats(equippedShieldItem);

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth != null) {
            playerHealth.UpdateEquipmentBonuses(totalHealth);
        }
    }

private void AddStats(Item item) {
    if (item == null) {
        return;
    }

    totalDefense += item.defenseBonus;
    totalAttack += item.attackBonus;
    totalHealth += item.healthBonus;
    }
}