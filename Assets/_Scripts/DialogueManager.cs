/*  DialogueManager.cs
    Displays dialogue and controls the dialogue UI.

    Reference:
    Redstoneinvente Game Studio. "Unity Dialogue System Tutorial: Linear Story Telling (C#)" YouTube.
    https://www.youtube.com/watch?v=GqzinfLEg9U

    Adapted for this game project.
*/

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public TMP_Text title;
    public TMP_Text content;

    public float characterDelay = 0.1f;
    public float punctuationDelay = 0.5f;

    public int maxWords = 20;                   // placeholder

    public bool moveNext;
    public GameObject moveNextButton;

    public Canvas dialogueCanvas;

    private bool isShowing = false;

    private void Awake() {
        instance = this;
    }

    public void MoveNext() {
        moveNext = true;
    }

    public void ShowDialogue(NPCManager nPCManager) {
        if (isShowing || nPCManager == null || nPCManager.CurrentDialogue == null) {
            return;
        }

        isShowing = true;
        StartCoroutine(ShowDialogueC(nPCManager));
    }

    IEnumerator ShowDialogueC(NPCManager nPCManager) {
        moveNext = false;

        title.text = nPCManager.CurrentDialogue.title;
        content.text = "";

        dialogueCanvas.enabled = true;

        foreach (char character in nPCManager.CurrentDialogue.lines) {
            content.text += character;
            yield return new WaitForSeconds(IsPunctuation(character) ? punctuationDelay : characterDelay);

            if (content.textInfo.wordCount >= maxWords && (character == ' ' || IsPunctuation(character) || character == ',')) {
                moveNextButton.SetActive(true);
                yield return new WaitUntil(() => moveNext);

                content.text = "";
                moveNextButton.SetActive(false);
                moveNext = false;
            }
        }

        moveNextButton.SetActive(true);
        yield return new WaitUntil(() => moveNext);

        content.text = "";
        moveNextButton.SetActive(false);
        moveNext = false;

        dialogueCanvas.enabled = false;

        isShowing = false;
        nPCManager.MoveNext();
    }

    bool IsPunctuation(char character) {
        return character == '.' || character == '?' || character == '!';
    }
}
