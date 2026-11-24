/*  KnightAttack.cs
    Basic melee combat.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttack : MonoBehaviour
{
    [Header("Melee Attack")]
    [SerializeField] private GameObject hitboxPrefab;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float hitboxLifetime = 0.2f;
    [SerializeField] private float attackDistance = 0.7f;
    

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
        if (hitboxPrefab == null || playerMovement == null) {
            return;
        }

        nextAttackTime = Time.time + attackCooldown;
        
        Vector3 direction = playerMovement.FacingDirection;
        Vector3 spawnPosition = transform.position + direction * attackDistance;
        spawnPosition.z = transform.position.z;

        GameObject hitbox = Instantiate(hitboxPrefab, spawnPosition, Quaternion.identity);
        int damage = 1;

        if (Inventory.Singleton != null) {
            damage += Inventory.Singleton.totalAttack;
        }

        MeleeHitbox hitboxScript = hitbox.GetComponent<MeleeHitbox>();

        if (hitboxScript != null) {
            hitboxScript.Initialize(damage, gameObject);
        }

        Destroy(hitbox, hitboxLifetime);
    }
}