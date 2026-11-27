/*  EnemyProjectile.cs
    Projectile behavior for ranged combat enemies.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour 
{
    private Rigidbody rb;
    private GameObject owner;
    private int damage;
    [SerializeField] private float lifetime = 3f;

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
        // Projectile will not damage owner
        if (other.gameObject == owner || other.transform.root.gameObject == owner) {
            return;
        }

        IDamageable target = other.GetComponentInParent<IDamageable>();

        if (target == null) {
            return;
        }
        target.Damage(damage);
        Destroy(gameObject);
    }
}