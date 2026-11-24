/*  EnemyRangedCombat.cs
    Basic enemy ranged combat.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedCombat : Enemy
{
    [Header("Ranged Combat")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float projectileSpeed = 8f;

    private bool isAttacking;
    private float nextAttackTime;

    protected override void Update() {
        if (isAttacking) {
            return;
        }

        base.Update();

        if (player == null) {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time >= nextAttackTime) {
            Attack();
        }
    }

    protected override void Attack() {
        if (projectilePrefab == null || firePoint == null || player == null) {
            return;
        }

        nextAttackTime = Time.time + attackCooldown;
        isAttacking = true;
        anim.SetTrigger("Attack");
    }

    public void Fire() {
        if (player == null || projectilePrefab == null || firePoint == null) {
            EndAttack();
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();

        if (projectileScript != null) {
            Vector3 direction = player.position - firePoint.position;
            projectileScript.Launch(direction, attackDamage, gameObject, projectileSpeed);
        }
    }

    public void EndAttack() {
        isAttacking = false;
    }
}