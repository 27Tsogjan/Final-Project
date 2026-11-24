/*  EnemyMeleeCombat.cs
    Basic melee combat for enemies.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeCombat : Enemy
{
    [Header("Melee Combat")]
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1.5f;
    

    private bool isAttacking;
    private float nextAttackTime;

    protected override void Attack() {
        if (player == null || isAttacking || Time.time < nextAttackTime) {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange) {
            nextAttackTime = Time.time + attackCooldown;
            isAttacking = true;
            anim.SetTrigger("Attack");
        }
    }

    protected override void Update() {
        if (isAttacking) {
            return;
        }

        base.Update();

        if (player == null) {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange) {
            Attack();
        }
    }

    public void DealDamage() {
        if (player == null) {
            return;
        }

        IDamageable target = player.GetComponent<IDamageable>();

        if (target != null) {
            target.Damage(attackDamage);
        }
    }

    public void EndAttack() {
        isAttacking = false;
    }
}