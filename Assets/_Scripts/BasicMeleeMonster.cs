/*  BasicMeleeMonster.cs
    Battle logic for basic melee monster.
    Only auto attacks, no special ability.

    Reference:
    Davis, Micha L. "Creating Enemy Attack Behavior in Unity." Medium.
    https://micha-l-davis.medium.com/creating-enemy-attack-behavior-in-unity-a73d144ba8ae

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeMonster : Enemy {
    [Header("Basic Melee")]
    [SerializeField] private int meleeDamage = 2;
    [SerializeField] private float meleeRange = 0.8f;
    [SerializeField] private float meleeCooldown = 1.5f;

    [Header("Attack Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip meleeSound;

    private bool isAttacking;
    private float nextMeleeTime;

    protected override void Init() {
        base.Init();

        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
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

        bool canMelee = false;
        if (distance <= meleeRange && Time.time >= nextMeleeTime) {
            canMelee = true;
        }

        int availableAttacks = 0;

        if (canMelee) {
            availableAttacks++;
        }

        if (availableAttacks == 0) {
            return;
        }

        int selectedAttack = Random.Range(0, availableAttacks);
        int currentAttack = 0;

        if (canMelee) {
            if (currentAttack == selectedAttack) {
                BasicMelee();
                return;
            }
            currentAttack++;
        }
    }

    private void BasicMelee() {
        nextMeleeTime = Time.time + meleeCooldown;
        isAttacking = true;
        anim.SetTrigger("Melee");
    }

    public void DealMeleeDamage() {
        if (player == null) {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= meleeRange) {
            IDamageable target = player.GetComponent<IDamageable>();

            if (target != null) {
                target.Damage(meleeDamage);
            }
        }
    }

    // Sound effects called through animation events.
    public void PlayMeleeSound() {
        if (audioSource != null && meleeSound != null) {
            audioSource.PlayOneShot(meleeSound);
        }
    }

    public void EndAttack() {
        isAttacking = false;
    }
}