/*  NPCDialogue.cs
    Stores NPC's dialogue text in ScriptableObject.

    Reference:
    Redstoneinvente Game Studio. "Unity Dialogue System Tutorial: Linear Story Telling (C#)" YouTube.
    https://www.youtube.com/watch?v=GqzinfLEg9U

    Adapted for this game project.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "NPCDialogue", menuName = "Simple Dialogue System/NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string title;
    [TextArea] public string lines;

    public GameManager.StoryProgress requiredProgress;
}