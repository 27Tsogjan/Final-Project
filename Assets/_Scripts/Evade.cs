/*  Evade.cs
    Allows players to evade.
    A successful evade with block all damage.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evade : MonoBehaviour
{
    [Header("Evading")]
    [SerializeField] private KeyCode evadeKey = KeyCode.Q;
    [SerializeField] private float evadeSpeed = 8f;
    [SerializeField] private float evadeDuration = 0.25f;
    [SerializeField] private float evadeCooldown = 1f;

    private Rigidbody rb;
    private PlayerMovement playerMovement;

    private float nextEvadeTime;
    private float evadeEndTime;
    private bool evading;

    public bool isEvading() {
        return evading;
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update() {
        if (Input.GetKeyDown(evadeKey) && Time.time >= nextEvadeTime && !evading && !playerMovement.IsFrozen) {
            StartEvade();
        }

        if (evading && Time.time >= evadeEndTime) {
            evading = false;
        }
    }

    private void FixedUpdate() {
        if (evading && !playerMovement.IsFrozen) {
            Vector3 direction = playerMovement.FacingDirection;
            direction.z = 0f;
            direction.Normalize();

            rb.velocity = new Vector3(direction.x * evadeSpeed, direction.y * evadeSpeed, rb.velocity.z);
        }
    }

    private void StartEvade() {
        evading = true;
        nextEvadeTime = Time.time + evadeCooldown;
        evadeEndTime = Time.time + evadeDuration;
    }
}