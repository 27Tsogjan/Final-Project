/*  Interactable.cs
    Base class for objects player can interact with.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactionMessage = "Press E to interact";

    public virtual void Interact() {
        Debug.Log("Object has been interacted with.");
    }
}