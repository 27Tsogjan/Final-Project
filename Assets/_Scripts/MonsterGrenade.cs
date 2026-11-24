/*  MonsterGrenade.cs
    Elite enemy grenade skill.
    Grenade is thrown at player.
    Explosion is delayed.

    Reference:
    Davis, Micha L. "Creating Enemy Attack Behavior in Unity." Medium.
    https://micha-l-davis.medium.com/creating-enemy-attack-behavior-in-unity-a73d144ba8ae

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGrenade : MonoBehaviour {
    [Header("Grenade Settings")]
    [SerializeField] private int damage = 3;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float throwDuration = 0.8f;
    [SerializeField] private float arcHeight = 0.75f;
    [SerializeField] private float explosionDelay = 1f;

    [Header("Explosion Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explosionSound;

    private Rigidbody rb;
    private GameObject owner;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private float elapsedTime;
    private bool isLaunched;
    private bool hasExploded;

    private void Awake() {
        rb = GetComponent<Rigidbody>();

        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void Launch(Vector3 target, GameObject grenadeOwner) {
        if (rb == null) {
            return;
        }

        owner = grenadeOwner;
        startPosition = rb.position;

        targetPosition = target;
        targetPosition.z = startPosition.z;

        elapsedTime = 0f;
        isLaunched = true;

        rb.isKinematic = true;
    }

    private void FixedUpdate() {
        if (!isLaunched || hasExploded) {
            return;
        }

        elapsedTime += Time.fixedDeltaTime;

        float progress = Mathf.Clamp01(elapsedTime / Mathf.Max(throwDuration, 0.01f));

        Vector3 position = Vector3.Lerp(startPosition, targetPosition, progress);
        position.y += Mathf.Sin(progress * Mathf.PI) * arcHeight;
        position.z = startPosition.z;

        rb.MovePosition(position);

        if (progress >= 1f) {
            Explode();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!isLaunched || hasExploded) {
            return;
        }

        if (other.gameObject == owner || other.transform.root.gameObject == owner) {
            return;
        }

        PlayerHealth target = other.GetComponentInParent<PlayerHealth>();

        if (target == null) {
            return;
        }

        Explode();
    }

    private void Explode() {
        if (hasExploded) {
            return;
        }

        hasExploded = true;
        Invoke(nameof(Detonate), explosionDelay);
    }

    private void Detonate() {
        if (audioSource != null && explosionSound != null) {
            audioSource.PlayOneShot(explosionSound);
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        HashSet<PlayerHealth> damagedTargets = new HashSet<PlayerHealth>();

        foreach (Collider hit in hits) {
            PlayerHealth target = hit.GetComponentInParent<PlayerHealth>();

            if (target != null && damagedTargets.Add(target)) {
                target.Damage(damage);
            }
        }

        Destroy(gameObject, 0.1f);
    }
}