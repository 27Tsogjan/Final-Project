/*  MidbossCombat.cs
    Battle logic for Midboss.
    Has an acid attack & ground slam.

    Reference:
    Davis, Micha L. "Creating Enemy Attack Behavior in Unity." Medium.
    https://micha-l-davis.medium.com/creating-enemy-attack-behavior-in-unity-a73d144ba8ae

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidbossCombat : Enemy {
    [Header("Acid Attack")]
    [SerializeField] private int attackDamage = 2;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float projectileSpeed = 3f;
    [SerializeField] private GameObject _acidBlob;

    [Header("Ground Slam")]
    [SerializeField] private int slamDamage = 3;
    [SerializeField] private float slamRange = 1.25f;
    [SerializeField] private float slamRadius = 1.5f;
    [SerializeField] private float slamCooldown = 5f;

    [Header("Basic Melee")]
    [SerializeField] private int meleeDamage = 2;
    [SerializeField] private float meleeRange = 0.8f;
    [SerializeField] private float meleeCooldown = 1.5f;

    [Header("Attack Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip acidSound;
    [SerializeField] private AudioClip meleeSound;
    [SerializeField] private AudioClip groundSlamSound;

    [SerializeField] private MidBoss midBoss;

    private bool isAttacking;
    private float nextAttackTime;
    private float nextSlamTime;
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

        bool canSlam = false;
        if (distance <= slamRange && Time.time >= nextSlamTime) {
            canSlam = true;
        }

        bool canAcid = false;
        if (distance <= attackRange && Time.time >= nextAttackTime) {
            canAcid = true;
        }

        int availableAttacks = 0;

        if (canMelee) {
            availableAttacks++;
        }

        if (canSlam) {
            availableAttacks++;
        }

        if (canAcid) {
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

        if (canSlam) {
            if (currentAttack == selectedAttack) {
                GroundSlam();
                return;
            }
            currentAttack++;
        }

        if (canAcid && currentAttack == selectedAttack) {
            Attack();
        }
    }

    protected override void Attack() {
        if (player == null || isAttacking || Time.time < nextAttackTime) {
            return;
        }

        nextAttackTime = Time.time + attackCooldown;
        isAttacking = true;
        anim.SetTrigger("Attack");
    }

    private void BasicMelee() {
        nextMeleeTime = Time.time + meleeCooldown;
        isAttacking = true;
        anim.SetTrigger("Melee");
    }

    private void GroundSlam() {
        nextSlamTime = Time.time + slamCooldown;
        isAttacking = true;
        anim.SetTrigger("GroundSlam");
    }

    public void Fire() {
        if (_acidBlob == null || player == null) {
            return;
        }

        Vector3 spawnPosition = new Vector3(transform.position.x - .45f, transform.position.y - .03f, transform.position.z);

        GameObject blob = Instantiate(_acidBlob, spawnPosition, Quaternion.identity);
        AcidEffect acid = blob.GetComponent<AcidEffect>();

        if (acid != null) {
            Vector3 direction = player.position - spawnPosition;
            acid.Launch(direction, attackDamage, gameObject, projectileSpeed);
        }
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

    public void DealGroundSlamDamage() {
        if (player == null) {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= slamRadius) {
            IDamageable target = player.GetComponent<IDamageable>();

            if (target != null) {
                target.Damage(slamDamage);
            }
        }
    }

    // Sound effects called through animation events.
    public void PlayAcidSound() {
        if (audioSource != null && acidSound != null) {
            audioSource.PlayOneShot(acidSound);
        }
    }

    public void PlayMeleeSound() {
        if (audioSource != null && meleeSound != null) {
            audioSource.PlayOneShot(meleeSound);
        }
    }

    public void PlayGroundSlamSound() {
        if (audioSource != null && groundSlamSound != null) {
            audioSource.PlayOneShot(groundSlamSound);
        }
    }

    public void EndAttack() {
        isAttacking = false;
    }

    // Player gets the key needed to face the final boss upon Midboss's defeat.
    public void OnDefeated() {
        if (midBoss != null) {
            midBoss.DefeatMidBoss();
        }
    }
}