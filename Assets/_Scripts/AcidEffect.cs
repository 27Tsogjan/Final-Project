/*  AcidEffect.cs
    Midboss acid skill.

    Reference:
    Davis, Micha L. "Creating Enemy Attack Behavior in Unity." Medium.
    https://micha-l-davis.medium.com/creating-enemy-attack-behavior-in-unity-a73d144ba8ae

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidEffect : MonoBehaviour {
    [Header("Acid Projectile")]
    [SerializeField] private float lifetime = 3f;

    private Rigidbody rb;
    private GameObject owner;
    private int damage;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        Destroy(gameObject, lifetime);
    }

    public void Launch(Vector3 direction, int damageAmount, GameObject projectileOwner, float speed) {
        owner = projectileOwner;
        damage = damageAmount;

        direction.z = 0f;
        direction.Normalize();

        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == owner || other.transform.root.gameObject == owner) {
            return;
        }
        IDamageable target = other.GetComponentInParent<IDamageable>();

        if (target != null) {
            target.Damage(damage);
        }

        target.Damage(damage);
        Destroy(gameObject);
    }
}