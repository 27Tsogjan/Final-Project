/*  OpenShop.cs
    Open shop menu.

    Audio file:
        closeShopSFX
        openShopSFX

    Reference:
    Game Dev Guru. "UNITY TUTORIAL - How to make a shop in Unity in 7 minutes [2022 UPDATED]." YouTube.
    https://www.youtube.com/watch?v=dhLea2Wk5fw

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : MonoBehaviour
{
    public GameObject shopPanel;
    public AudioClip openShopSFX;
    public AudioClip closeShopSFX;


    void Start() {
        if (shopPanel != null) {
            shopPanel.SetActive(false);
        }
        
        else {
            Debug.LogError("OpenShop: Shop Panel is not assigned!", this);
        }
    }

    private void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Player") && shopPanel != null) {
            shopPanel.SetActive(true);

            if (SoundManager.Instance != null && openShopSFX != null) {
                SoundManager.Instance.Play(openShopSFX);
            }
        }
    }

    private void OnTriggerExit(Collider col) {
        if (col.CompareTag("Player") && shopPanel != null) {
            shopPanel.SetActive(false);

            if (SoundManager.Instance != null && closeShopSFX != null) {
                SoundManager.Instance.Play(closeShopSFX);
            }
        }
    }
}
