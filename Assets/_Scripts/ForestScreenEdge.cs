using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestScreenEdge : MonoBehaviour
{
    public GameObject[] sections;
    public NPCManager[] sectionDialogues;
    public float leftX = -8f;

    private int current = 0;

    private void Start() {
        ShowSection(0);
    }

    private void ShowSection(int index) {
        for (int i = 0; i < sections.Length; i++) {
            sections[i].SetActive(i == index);
        }

        current = index;

        if (index < sectionDialogues.Length && sectionDialogues[index] != null) {
            StartCoroutine(RunDialogue(sectionDialogues[index]));
        }
    }

    private IEnumerator RunDialogue(NPCManager manager) {
        // Wait a frame so the hero has been spawned by LoadCharacters
        yield return null;

        GameObject p = GameObject.FindWithTag("Player");
        PlayerMovement pm = null;
        if (p != null) {
            pm = p.GetComponent<PlayerMovement>();
        }

        manager.StartDialogue();

        if (DialogueManager.instance == null) {
            yield break;
        }

        Canvas dialogueCanvas = DialogueManager.instance.dialogueCanvas;

        // If no dialogue matched, there is nothing to freeze for
        if (pm == null || dialogueCanvas == null || !dialogueCanvas.enabled) {
            yield break;
        }

        pm.Freeze(1000f);
        yield return new WaitUntil(() => !dialogueCanvas.enabled);
        pm.Freeze(0f);   // Frozen for 0 seconds, so this releases the hero
    }

    private void OnTriggerEnter(Collider other) {
        PlayerMovement pm = other.GetComponentInParent<PlayerMovement>();

        if (pm == null || current >= sections.Length - 1) {
            return;
        }

        // Back to the far left, keeping the current Y
        Vector3 pos = new Vector3(leftX, pm.transform.position.y, pm.transform.position.z);
        pm.transform.position = pos;

        Rigidbody rb = pm.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.position = pos;
        }

        ShowSection(current + 1);
    }
}