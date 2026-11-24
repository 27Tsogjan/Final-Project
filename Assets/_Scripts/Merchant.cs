/*  Merchant.cs
    Allows player to interact with a merchant.

    Audio file:
        merchantVoiceSFX
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : NPCInteraction
{
    public GameObject shopPanel;
    public AudioClip merchantVoiceSFX;

    private void Reset() {
        interactionMessage = "Press E to shop";
    }

    public override void Interact() {
        base.Interact();

        shopPanel.SetActive(true);

        // Unsure if you'll add SFX
        if (SoundManager.Instance != null && merchantVoiceSFX != null) {
            SoundManager.Instance.Play(merchantVoiceSFX);
        }

        // Could you set the merchant line to this in game?
        Debug.Log(npcName + ": " + "Not many people are here anymore. Take a look at my shop.");
    }

    public void CloseShop() {
        shopPanel.SetActive(false);
    }
}