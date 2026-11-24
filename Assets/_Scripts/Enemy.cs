/*  Enemy.cs
    Enemy base class.

    @Janice: we'll need monster assets for both idle and in battle.

    Reference:
    Davis, Micha L. "Modular Enemies Using Abstract Classes and Interfaces in Unity." Medium.
    https://micha-l-davis.medium.com/modular-enemies-using-abstract-classes-and-interfaces-in-unity-6fa833057ecc

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class Enemy : MonoBehaviour, IDamageable
{
    public int Health { get { return health; } set { health = value; } }

    // Health, speed, loot
    [SerializeField]
    protected int health;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected int g;

    [Header("Enemy Health Bar")]
    [SerializeField] protected Slider healthBar;

    protected int maxHealth;

    // Patrol waypoint variables
    [SerializeField]
    protected Transform pointA, pointB;
    protected Vector3 movementTarget;

    // Sprite and Animation component handle variables
    protected Animator anim;
    protected SpriteRenderer rend;

    protected bool isHit;
    protected float canTakeDamage;
    protected float damageCooldown = 0.5f;

    private bool isDead;

    // Handle to the player
    [SerializeField]
    protected Transform player;

    protected void Start() {
        Init();

        maxHealth = health;

        if (healthBar != null) {
            healthBar.maxValue = maxHealth;
            healthBar.value = health;
        }
    }

    // Load each enemy with awareness of their components at start of program
    protected virtual void Init() {
        anim = GetComponentInChildren<Animator>();

        if (anim == null) {
            Debug.LogError(transform.name + " Animator is NULL");
        }

        rend = GetComponentInChildren<SpriteRenderer>();
        if (rend == null) {
            Debug.LogError(transform.name + " Sprite Renderer is NULL");
        }
    }

    protected virtual void Update() {
        // Find player spawned at runtime
        if (player == null) {
            GameObject p = GameObject.FindWithTag("Player");

            if (p != null) {
                player = p.transform;
            }
        }

        // Enemy will not move until player is found
        if (player == null) {
            return;
        }

        if (anim == null) {
            return;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle_anim") && !anim.GetBool("InCombat")) {
            return;
        }

        Move();
    }

    protected virtual void Move() {
        if (pointA == null || pointB == null || rend == null || anim == null) {
            return;
        }

        if (movementTarget == Vector3.zero) {
            movementTarget = pointA.position;
        }

        if (movementTarget == pointA.position) {
            rend.flipX = true;
        }

        else if (movementTarget == pointB.position) {
            rend.flipX = false;
        }

        if (transform.position == pointA.position) {
            anim.SetTrigger("Idle");
            movementTarget = pointB.position;
        }

        else if (transform.position == pointB.position) {
            anim.SetTrigger("Idle");
            movementTarget = pointA.position;
        }

        if (!isHit) {
            transform.position = Vector3.MoveTowards(transform.position, movementTarget, speed * Time.deltaTime);
        }

        if (Vector2.Distance(player.position, transform.position) > 2f) {
            isHit = false;
            anim.SetBool("InCombat", false);
        }
    }

    public void Damage(int damageAmount) {
        if (isDead || damageAmount <= 0) {
            return;
        }

        if (Time.time < canTakeDamage) {
            return;
        }

        canTakeDamage = Time.time + damageCooldown;

        if (anim != null) {
            anim.SetTrigger("Hit");
        }
            
        Health = Mathf.Max(0, Health - damageAmount);
        isHit = true;

        if (anim != null) {
            anim.SetBool("InCombat", true);
        }
            
        if (healthBar != null) {
            healthBar.value = Health;
        }

        if (Health <= 0) {
            isDead = true;

            // Midboss event upon defeat
            MidbossCombat midboss = GetComponent<MidbossCombat>();
            if (midboss != null) {
                midboss.OnDefeated();
            }

            // Final boss event upon defeat
            FinalBoss finalBoss = GetComponent<FinalBoss>();
            if (finalBoss != null) {
                finalBoss.DefeatBoss();
            }

            Destroy(gameObject, 0.5f);
        }
    }

    protected virtual void Attack() {
    }
}
    
