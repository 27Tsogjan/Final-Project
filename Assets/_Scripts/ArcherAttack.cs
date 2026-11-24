/*  ArcherAttack.cs
    Basic ranged combat.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttack : MonoBehaviour
{
    [Header("Ranged Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackCooldown = 0.5f;

    private PlayerMovement playerMovement;
    private float nextAttackTime;

    private void Start() {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextAttackTime) {
            Attack();
        }
    }

    private void Attack() {
        if (projectilePrefab == null || firePoint == null || playerMovement == null) {
            return;
        }

        nextAttackTime = Time.time + attackCooldown;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null) {
            projectileScript.Launch(playerMovement.FacingDirection, gameObject);
        }
    }
}