/*  FinalBossCombat.cs
    Battle logic for final boss.
    Final boss can use every special ability from other monsters.
    Also has a freeze skill.

    Reference:
    Davis, Micha L. "Creating Enemy Attack Behavior in Unity." Medium.
    https://micha-l-davis.medium.com/creating-enemy-attack-behavior-in-unity-a73d144ba8ae

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossCombat : Enemy {
    [Header("Acid Attack")]
    [SerializeField] private int acidDamage = 2;
    [SerializeField] private float acidRange = 5f;
    [SerializeField] private float acidCooldown = 2f;
    [SerializeField] private float acidProjectileSpeed = 3f;
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

    [Header("Bite Attack")]
    [SerializeField] private int biteDamage = 3;
    [SerializeField] private float biteRange = 0.8f;
    [SerializeField] private float biteCooldown = 4f;

    [Header("Grenade Attack")]
    [SerializeField] private GameObject _grenade;
    [SerializeField] private float grenadeRange = 5f;
    [SerializeField] private float grenadeCooldown = 6f;

    [Header("Ice Spell")]
    [SerializeField] private float iceRange = 5f;
    [SerializeField] private float iceCooldown = 8f;
    [SerializeField] private GameObject iceProjectilePrefab;
    [SerializeField] private Transform iceFirePoint;

    [Header("Attack Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip acidSound;
    [SerializeField] private AudioClip meleeSound;
    [SerializeField] private AudioClip biteSound;
    [SerializeField] private AudioClip groundSlamSound;
    [SerializeField] private AudioClip grenadeSound;
    [SerializeField] private AudioClip iceSound;

    private bool isAttacking;

    private float nextAcidTime;
    private float nextSlamTime;
    private float nextMeleeTime;
    private float nextBiteTime;
    private float nextGrenadeTime;
    private float nextIceTime;

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

        bool canBite = false;
        if (distance <= biteRange && Time.time >= nextBiteTime) {
            canBite = true;
        }

        bool canSlam = false;
        if (distance <= slamRange && Time.time >= nextSlamTime) {
            canSlam = true;
        }

        bool canAcid = false;
        if (distance <= acidRange && Time.time >= nextAcidTime) {
            canAcid = true;
        }

        bool canGrenade = false;
        if (distance <= grenadeRange && Time.time >= nextGrenadeTime) {
            canGrenade = true;
        }

        bool canIce = false;
        if (distance <= iceRange && Time.time >= nextIceTime) {
            canIce = true;
        }

        int availableAttacks = 0;

        if (canMelee) availableAttacks++;
        if (canBite) availableAttacks++;
        if (canSlam) availableAttacks++;
        if (canAcid) availableAttacks++;
        if (canGrenade) availableAttacks++;
        if (canIce) availableAttacks++;

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

        if (canBite) {
            if (currentAttack == selectedAttack) {
                Bite();
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

        if (canAcid) {
            if (currentAttack == selectedAttack) {
                Attack();
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

        if (canIce && currentAttack == selectedAttack) {
            IceSpell();
        }
    }

    protected override void Attack() {
        if (player == null || isAttacking || Time.time < nextAcidTime) {
            return;
        }

        nextAcidTime = Time.time + acidCooldown;
        isAttacking = true;
        anim.SetTrigger("Attack");
    }

    private void BasicMelee() {
        nextMeleeTime = Time.time + meleeCooldown;
        isAttacking = true;
        anim.SetTrigger("Melee");
    }

    private void Bite() {
        nextBiteTime = Time.time + biteCooldown;
        isAttacking = true;
        anim.SetTrigger("Bite");
    }

    private void GroundSlam() {
        nextSlamTime = Time.time + slamCooldown;
        isAttacking = true;
        anim.SetTrigger("GroundSlam");
    }

    private void ThrowGrenade() {
        nextGrenadeTime = Time.time + grenadeCooldown;
        isAttacking = true;
        anim.SetTrigger("ThrowGrenade");
    }

    private void IceSpell() {
        nextIceTime = Time.time + iceCooldown;
        isAttacking = true;
        anim.SetTrigger("IceSpell");
    }

    public void FireAcid() {
        if (_acidBlob == null || player == null) {
            return;
        }

        Vector3 spawnPosition = new Vector3(
            transform.position.x - 0.45f,
            transform.position.y - 0.03f,
            transform.position.z
        );

        GameObject blob = Instantiate(_acidBlob, spawnPosition, Quaternion.identity);
        AcidEffect acid = blob.GetComponent<AcidEffect>();

        if (acid != null) {
            Vector3 direction = player.position - spawnPosition;
            acid.Launch(direction, acidDamage, gameObject, acidProjectileSpeed);
        }
    }

    public void Throw() {
        if (_grenade == null || player == null) {
            return;
        }

        GameObject grenadeObject = Instantiate(
            _grenade,
            transform.position,
            Quaternion.identity
        );

        MonsterGrenade grenade = grenadeObject.GetComponent<MonsterGrenade>();

        if (grenade != null) {
            grenade.Launch(player.position, gameObject);
        }
    }

    public void DealMeleeDamage() {
        DealDirectDamage(meleeDamage, meleeRange);
    }

    public void DealBiteDamage() {
        DealDirectDamage(biteDamage, biteRange);
    }

    public void DealGroundSlamDamage() {
        DealDirectDamage(slamDamage, slamRadius);
    }

    private void DealDirectDamage(int damage, float range) {
        if (player == null) {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= range) {
            IDamageable target = player.GetComponent<IDamageable>();

            if (target != null) {
                target.Damage(damage);
            }
        }
    }

    public void FireIce() {
        if (player == null || iceProjectilePrefab == null || iceFirePoint == null) {
            return;
        }

        GameObject projectile = Instantiate(
            iceProjectilePrefab,
            iceFirePoint.position,
            Quaternion.identity
        );

        IceProjectile ice = projectile.GetComponent<IceProjectile>();

        if (ice != null) {
            Vector3 direction = player.position - iceFirePoint.position;
            ice.Launch(direction, gameObject);
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

    public void PlayBiteSound() {
        if (audioSource != null && biteSound != null) {
            audioSource.PlayOneShot(biteSound);
        }
    }

    public void PlayGroundSlamSound() {
        if (audioSource != null && groundSlamSound != null) {
            audioSource.PlayOneShot(groundSlamSound);
        }
    }

    public void PlayGrenadeSound() {
        if (audioSource != null && grenadeSound != null) {
            audioSource.PlayOneShot(grenadeSound);
        }
    }

    public void PlayIceSound() {
        if (audioSource != null && iceSound != null) {
            audioSource.PlayOneShot(iceSound);
        }
    }

    public void EndAttack() {
        isAttacking = false;
    }
}