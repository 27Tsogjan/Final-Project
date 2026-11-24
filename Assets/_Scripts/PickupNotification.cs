/*  PickupNotification.cs
    Notification that displays when an item is picked up.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickupNotification : MonoBehaviour {
    public TMP_Text notificationText;

    private void Start() {
        notificationText.gameObject.SetActive(false);
    }

    public void ShowNotification(string message) {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);
        CancelInvoke("HideNotification");
        Invoke("HideNotification", 2f);
    }

    private void HideNotification() {
        notificationText.gameObject.SetActive(false);
    }
}