/*  Attack.cs
    Detects damageable objects and applies damage.

    Reference:
    Davis, Micha L. "Creating Enemy Attack Behavior in Unity." Medium.
    https://micha-l-davis.medium.com/creating-enemy-attack-behavior-in-unity-a73d144ba8ae

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    [SerializeField] private int damage = 1;

    void OnTriggerEnter(Collider other) {
        IDamageable hit = other.GetComponent<IDamageable>();
        if (hit != null) {
            hit.Damage(damage);
        }
    }
}