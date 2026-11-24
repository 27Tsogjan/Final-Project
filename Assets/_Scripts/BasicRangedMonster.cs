/*  BasicRangedMonster.cs
    Battle logic for elite ranged monster.
    Has .

    Reference:
    Davis, Micha L. "Creating Enemy Attack Behavior in Unity." Medium.
    https://micha-l-davis.medium.com/creating-enemy-attack-behavior-in-unity-a73d144ba8ae

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedMonster : Enemy {
    [Header("Basic Ranged Attack")]
    [SerializeField] private float rangedCooldown = 2f;
    [SerializeField] private float rangedRange = 5f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int rangedDamage = 1;
    [SerializeField] private float projectileSpeed = 8f;

    [Header("Attack Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip rangedSound;


    private bool isAttacking;
    private float nextRangedTime;

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

        bool canRanged = false;
        if (distance <= rangedRange && Time.time >= nextRangedTime) {
            canRanged = true;
        }

        int availableAttacks = 0;

        if (canRanged) {
            availableAttacks++;
        }

        if (availableAttacks == 0) {
            return;
        }

        int selectedAttack = Random.Range(0, availableAttacks);
        int currentAttack = 0;

        if (canRanged) {
            if (currentAttack == selectedAttack) {
                BasicRanged();
                return;
            }
            currentAttack++;
        }
    }

    public void FireRanged() {
        if (projectilePrefab == null || firePoint == null || player == null) {
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();

        if (projectileScript != null) {
            Vector3 direction = player.position - firePoint.position;
            projectileScript.Launch(direction, rangedDamage, gameObject, projectileSpeed);
        }
    }

    private void BasicRanged() {
        nextRangedTime = Time.time + rangedCooldown;
        isAttacking = true;
        anim.SetTrigger("Ranged");
    }
    
    public void PlayRangedSound() {
        if (audioSource != null && rangedSound != null) {
            audioSource.PlayOneShot(rangedSound);
        }
    }

    public void EndAttack() {
        isAttacking = false;
    }
}