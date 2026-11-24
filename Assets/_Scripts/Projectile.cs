/*  Projectile.cs
    Projectile behavior for ranged combat.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [Header("Projectile Settings")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float lifetime = 3f;

    private Rigidbody rb;
    private GameObject owner;
    private int finalDamage;
    private bool hasHit;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void Start() {
        Destroy(gameObject, lifetime);
    }

    public void Launch(Vector3 direction, GameObject projectileOwner) {
        owner = projectileOwner;

        // Damage with equipment bonuses applied
        finalDamage = damage;

        if (Inventory.Singleton != null) {
            finalDamage += Inventory.Singleton.totalAttack;
        }
        direction.z = 0f;
        direction.Normalize();

        rb.velocity = direction * speed;
    }

    void OnTriggerEnter(Collider other) {
        if (hasHit) {
            return;
        }

        // Projectile will not damage owner
        if (other.gameObject == owner || other.transform.root.gameObject == owner) {
            return;
        }

        IDamageable target = other.GetComponentInParent<IDamageable>();

        if (target != null) {
            hasHit = true;
            target.Damage(finalDamage);
            Destroy(gameObject);
        }
    }
}