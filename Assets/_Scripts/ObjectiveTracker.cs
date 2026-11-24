/*  ObjectiveTracker.cs
    Track player objectives.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveTracker : MonoBehaviour
{
    public TMP_Text objectiveText;

    public void SetObjective(string objective) {
        if (objectiveText != null) {
            objectiveText.text = "Objective: " + objective;
        }
    }

    public void CompleteObjective() {
        if (objectiveText != null) {
            objectiveText.text = "Objective Complete!";
        }
    }
}