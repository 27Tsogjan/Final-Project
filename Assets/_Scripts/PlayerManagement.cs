/*  PlayerManagement.cs
    Player management interaction with shop.

    Audio files:
        purchaseSFX
        notEnoughMoneySFX

    Reference:
    Game Dev Guru. "UNITY TUTORIAL - How to make a shop in Unity in 7 minutes [2022 UPDATED]." YouTube.
    https://www.youtube.com/watch?v=dhLea2Wk5fw

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManagement : MonoBehaviour
{
    [Header("Player Currency")]
    public int g = 1000;

    [Header("Shop UI")]
    public TMP_Text gText;
    public FeedbackNotification feedbackNotification;
    public Item potionItem;

    [Header("Shop Audio")]
    public AudioClip purchaseSFX;
    public AudioClip notEnoughMoneySFX;

    private void Start() {
        UpdateGDisplay();
    }
    
    public void BuyItem(Item item) {
        if (!TryBuyItem(item)) {
            return;
        }
    }

    public void BuyPotion() {
        BuyItem(potionItem);
    }

    private bool TryBuyItem(Item item) {
        if (item == null)
        {
            Debug.LogWarning("No item assigned.");
            return false;
        }

        if (g < item.buyPrice)
        {
            if (feedbackNotification != null)
                feedbackNotification.ShowMessage("Hey! You don't have enough G for that!");

            if (SoundManager.Instance != null && notEnoughMoneySFX != null)
                SoundManager.Instance.Play(notEnoughMoneySFX);

            return false;
        }

        if (Inventory.Singleton == null ||
            !Inventory.Singleton.SpawnInventoryItem(item))
        {
            if (feedbackNotification != null)
                feedbackNotification.ShowMessage("Hey! Your inventory is full!");

            return false;
        }

        g -= item.buyPrice;

        if (SoundManager.Instance != null && purchaseSFX != null)
            SoundManager.Instance.Play(purchaseSFX);

        UpdateGDisplay();

        Debug.Log("Purchased " + item.name + "!");
        return true;
    }

    public void UpdateGDisplay() {
        if (gText != null) {
            gText.text = "G: " + g;
        }
    }
}
