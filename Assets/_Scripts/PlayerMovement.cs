/*  PlayerMovement.cs
    Controls player movement.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private float moveX;
    private float moveY;
    private bool isFrozen;
    private Coroutine freezeCoroutine;

    public Vector3 FacingDirection { get; private set; } = Vector3.right;

    public bool IsFrozen => isFrozen;

    private void Start() {
        rb = GetComponent<Rigidbody>();

        if (rb == null) {
            Debug.LogError("PlayerMovement requires a Rigidbody!", this);
            enabled = false;
        }
    }

    private void Update() {
        if (isFrozen) {
            moveX = 0;
            moveY = 0;
            return;
        }

        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0 || moveY != 0) {
            FacingDirection = new Vector3(moveX, moveY, 0).normalized;
        }
    }

    // Character movement physics
    private void FixedUpdate() {
        if (isFrozen) {
            rb.velocity = Vector3.zero;
            return;
        }

        Evade evade = GetComponent<Evade>();

        if (evade != null && evade.isEvading()) {
            return;
        }

        rb.velocity = new Vector3(moveX, moveY, 0).normalized * moveSpeed;
    }

    public void Freeze(float duration) {
        if (freezeCoroutine != null) {
            StopCoroutine(freezeCoroutine);
        }

        freezeCoroutine = StartCoroutine(FreezePlayer(duration));
    }

    private IEnumerator FreezePlayer(float duration) {
        isFrozen = true;
        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(duration);

        isFrozen = false;
        freezeCoroutine = null;
    }
}