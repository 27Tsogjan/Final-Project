/*  IceProjectile.cs
    Final boss ice projectile.
    Damages and freezes player on impact.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : MonoBehaviour {
    [Header("Ice Projectile Settings")]
    [SerializeField] private int damage = 2;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float freezeDuration = 3f;
    [SerializeField] private float lifetime = 4f;

    private Rigidbody rb;
    private GameObject owner;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        Destroy(gameObject, lifetime);
    }

    public void Launch(Vector3 direction, GameObject projectileOwner) {
        owner = projectileOwner;

        direction.z = 0f;
        direction.Normalize();

        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == owner || other.transform.root.gameObject == owner) {
            return;
        }

        PlayerMovement movement = other.GetComponentInParent<PlayerMovement>();

        if (movement != null) {
            IDamageable target = movement.GetComponent<IDamageable>();

            if (target != null) {
                target.Damage(damage);
            }

            movement.Freeze(freezeDuration);
        }

        Destroy(gameObject);
    }
}