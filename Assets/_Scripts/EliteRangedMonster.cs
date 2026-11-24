/*  EliteRangedMonster.cs
    Battle logic for elite ranged monster.
    Has a skill to throw a grenade at player.

    Reference:
    Davis, Micha L. "Creating Enemy Attack Behavior in Unity." Medium.
    https://micha-l-davis.medium.com/creating-enemy-attack-behavior-in-unity-a73d144ba8ae

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteRangedMonster : Enemy {
    [Header("Grenade Attack")]
    [SerializeField] private float grenadeRange = 5f;
    [SerializeField] private float grenadeCooldown = 6f;
    [SerializeField] private GameObject _grenade;

    [Header("Basic Ranged Attack")]
    [SerializeField] private float rangedCooldown = 2f;
    [SerializeField] private float rangedRange = 5f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int rangedDamage = 1;
    [SerializeField] private float projectileSpeed = 3f;


    [Header("Attack Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip grenadeSound;
    [SerializeField] private AudioClip rangedSound;


    private bool isAttacking;
    private float nextGrenadeTime;
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

        bool canGrenade = false;
        if (distance <= grenadeRange && Time.time >= nextGrenadeTime) {
            canGrenade = true;
        }

        int availableAttacks = 0;

        if (canRanged) {
            availableAttacks++;
        }

        if (canGrenade) {
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

        if (canGrenade) {
            if (currentAttack == selectedAttack) {
                ThrowGrenade();
                return;
            }
            currentAttack++;
        }
    }


    private void BasicRanged() {
        nextRangedTime = Time.time + rangedCooldown;
        isAttacking = true;
        anim.SetTrigger("Ranged");
    }

    private void ThrowGrenade() {
        nextGrenadeTime = Time.time + grenadeCooldown;
        isAttacking = true;
        anim.SetTrigger("ThrowGrenade");
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

    public void Throw() {
        if (_grenade == null || player == null || firePoint == null) {
            return;
        }

        GameObject grenadeObject = Instantiate(_grenade, firePoint.position, Quaternion.identity);
        MonsterGrenade grenade = grenadeObject.GetComponent<MonsterGrenade>();

        if (grenade != null) {
            grenade.Launch(player.position, gameObject);
        }
    }

    // Sound effects called through animation events.
    public void PlayGrenadeSound() {
        if (audioSource != null && grenadeSound != null) {
            audioSource.PlayOneShot(grenadeSound);
        }
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