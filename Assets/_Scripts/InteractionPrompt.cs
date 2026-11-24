/*  InteractionPrompt.cs
    Shows prompt while near interactable objects.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionPrompt : MonoBehaviour
{
    public TMP_Text promptText;

    private void Start() {
        promptText.gameObject.SetActive(false);
    }

    public void ShowPrompt(string message) {
        promptText.text = message;
        promptText.gameObject.SetActive(true);
    }

    public void HidePrompt() {
        promptText.gameObject.SetActive(false);
    }
}