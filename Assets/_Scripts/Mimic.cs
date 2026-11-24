/*  Mimic.cs
    Mimic monster.
    Looks like a treasure chest.
    Begins to attack after player interacts with it.
    Has bite skill.

    Reference:
    Davis, Micha L. "Creating Enemy Attack Behavior in Unity." Medium.
    https://micha-l-davis.medium.com/creating-enemy-attack-behavior-in-unity-a73d144ba8ae

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : Enemy {
    [Header("Basic Melee Attack")]
    [SerializeField] private int attackDamage = 2;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Bite Attack")]
    [SerializeField] private int biteDamage = 3;
    [SerializeField] private float biteRange = 0.8f;
    [SerializeField] private float biteCooldown = 4f;

    private bool isActivated;
    private bool isAttacking;
    private float nextAttackTime;
    private float nextBiteTime;

    protected override void Update() {
        if (!isActivated || isAttacking) {
            return;
        }

        base.Update();

        if (player == null) {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        bool canAttack = false;
        if (distance <= attackRange && Time.time >= nextAttackTime) {
            canAttack = true;
        }

        bool canBite = false;
        if (distance <= biteRange && Time.time >= nextBiteTime) {
            canBite = true;
        }

        int availableAttacks = 0;

        if (canAttack) {
            availableAttacks++;
        }

        if (canBite) {
            availableAttacks++;
        }

        if (availableAttacks == 0) {
            return;
        }

        int selectedAttack = Random.Range(0, availableAttacks);
        int currentAttack = 0;

        if (canAttack) {
            if (currentAttack == selectedAttack) {
                BasicMelee();
                return;
            }
            currentAttack++;
        }

        if (canBite) {
            if (currentAttack == selectedAttack) {
                Bite();
                return;
            }
        }
    }

    public void ActivateMimic() {
        if (isActivated) {
            return;
        }

        isActivated = true;
        Debug.Log("The mimic has awakened!");
    }

    private void BasicMelee() {
        nextAttackTime = Time.time + attackCooldown;
        isAttacking = true;
        anim.SetTrigger("Attack");
    }

    private void Bite() {
        nextBiteTime = Time.time + biteCooldown;
        isAttacking = true;
        anim.SetTrigger("Bite");
    }

    public void DealDamage() {
        if (player == null) {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange) {
            IDamageable target = player.GetComponent<IDamageable>();

            if (target != null) {
                target.Damage(attackDamage);
            }
        }
    }

    public void DealBiteDamage() {
        if (player == null) {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= biteRange) {
            IDamageable target = player.GetComponent<IDamageable>();

            if (target != null) {
                target.Damage(biteDamage);
            }
        }
    }

    public void EndAttack() {
        isAttacking = false;
    }
}