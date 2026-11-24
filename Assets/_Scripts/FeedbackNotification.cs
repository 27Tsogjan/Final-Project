/*  FeedbackNotification.cs
    Allows player to collect items.

    Audio files:
    collectSFX
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FeedbackNotification : MonoBehaviour
{
    public TMP_Text notificationText;

    private void Start() {
        notificationText.gameObject.SetActive(false);
    }

    public void ShowMessage(string message) {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);

        CancelInvoke("HideMessage");
        Invoke("HideMessage", 2f);
    }

    private void HideMessage() {
        notificationText.gameObject.SetActive(false);
    }

}