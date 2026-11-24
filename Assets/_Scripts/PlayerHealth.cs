/*  PlayerHealth.cs
    System that manages player health.
    If HP = 0, game over.

    Reference:
    Davis, Micha L. "Modular Enemies Using Abstract Classes and Interfaces in Unity." Medium.
    https://micha-l-davis.medium.com/modular-enemies-using-abstract-classes-and-interfaces-in-unity-6fa833057ecc

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Player Health")]
    [SerializeField] private int health = 10;
    [SerializeField] private float damageCooldown = 0.5f;

    [SerializeField] private Slider healthBar;
    [SerializeField] private int maxHealth = 10;

    private int baseMaxHealth;
    private float canTakeDamage;
    public int Health { get { return health; } set { health = value; } }

    // Health bar
    private void Start() {
        baseMaxHealth = maxHealth;

        // Character has no equipment with health bonus at start of game
        int healthBonus = 0;

        if (Inventory.Singleton != null) {
            healthBonus = Inventory.Singleton.totalHealth;
        }

        maxHealth = baseMaxHealth + healthBonus;
        health = maxHealth;
        UpdateHealthBar();
    }

    public void UpdateEquipmentBonuses(int healthBonus) {
        // Add HP bonus
        int previousMaxHealth = maxHealth;
        maxHealth = baseMaxHealth + healthBonus;
        health += maxHealth - previousMaxHealth;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthBar();

        if (health == 0) {
            Die();
        }
    }

    public void Damage(int damageAmount) {
        if (damageAmount <= 0) {
            return;
        }
        Evade evade = GetComponent<Evade>();

        if (evade != null && evade.isEvading()) {
            return;
        }

        if (Time.time <= canTakeDamage || health <= 0) {
            return;
        }

        int defense = 0;

        if (Inventory.Singleton != null) {
            defense = Inventory.Singleton.totalDefense;
        }

        int finalDamage = damageAmount;
        Block block = GetComponent<Block>();
        if (block != null) {
            finalDamage = block.ReduceDamage(finalDamage);
        }

        finalDamage = Mathf.Max(1, finalDamage - defense);

        canTakeDamage = Time.time + damageCooldown;
        Health = Mathf.Max(0, Health - finalDamage);

        UpdateHealthBar();

        Debug.Log("Player Health: " + Health);
        Debug.Log("Damage received: " + finalDamage);

        if (health <= 0) {
            Die();
        }
    }

    private void UpdateHealthBar() {
        if (healthBar != null) {
            healthBar.maxValue = maxHealth;
            healthBar.value = health;
        }
    }

    public bool RestoreHealth(int amount) {
        if (amount <= 0 || health <= 0 || health >= maxHealth) {
            return false;
        }

        health = Mathf.Min(health + amount, maxHealth);
        UpdateHealthBar();

        Debug.Log("Potion used! Player Health: " + Health);
        return true;
    }

    private void Die() {
        Debug.Log("Player defeated!");

        if (GameManager.Instance != null) {
            GameManager.Instance.GameOver();
        }
    }
}