/*  MeleeHitbox.cs
    Range for melee attacks.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    private int damage;
    private GameObject owner;

    public void Initialize(int damageAmount, GameObject attacker) {
        damage = damageAmount;
        owner = attacker;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.root.gameObject == owner) {
            return;
        }

        IDamageable target = other.GetComponentInParent<IDamageable>();

        if (target != null) {
            target.Damage(damage);
        }
    }
}