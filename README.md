# Vengeance — Game Project

## Overview

**Vengeance** is a Unity-based game developed in C#. It features player combat, enemy encounters, inventory and equipment management, boss battles, and story progression.

Players explore the game world, fight enemies, collect items, upgrade their equipment, complete objectives, and progress through the mid-boss and final-boss encounters.

## Game Features

- **Combat System:** Supports player attacks, blocking, evading, and enemy melee and ranged attacks.
- **Health System:** Tracks player health, applies incoming damage, and allows health restoration through potions.
- **Inventory System:** Allows players to collect, move, and use items.
- **Equipment System:** Supports head, chest, and shield equipment, with bonuses to health, attack, and defense.
- **Shop System:** Allows players to open and close the shop interface.
- **Enemy Encounters:** Includes basic and elite enemies with different combat behaviors.
- **Boss Encounters:** Features a mid-boss and a final boss with unique attacks.
- **Story Progression:** Uses boss encounters, a boss key, and a locked door to advance the game.
- **Objective Tracking:** Displays objectives that update as the player progresses.
- **Audio System:** Uses background music and sound effects for menus, gameplay, inventory interactions, and combat.
- **Menu System:** Includes character selection, pause, restart, main menu, game-over, and victory screens.

## Scene Structure

The project uses five scenes in the following build order:

| Build Index | Scene |
|---|---|
| 0 | Title |
| 1 | Character Select |
| 2 | Main Game |
| 3 | Game Over |
| 4 | Win |

## Main Scripts

### Game Management

- `GameManager.cs` — Manages game state, story progression, game-over conditions, and victory.
- `MenuManager.cs` — Connects menu actions and handles navigation between game screens.
- `PauseMenu.cs` — Handles pausing, continuing, restarting, and returning to the main menu.
- `GameOverScreen.cs` — Handles game-over screen actions and music.
- `WinScreen.cs` — Handles victory screen actions and music.
- `SoundManager.cs` — Provides shared audio playback methods.

### Player Systems

- `PlayerHealth.cs` — Manages player health, damage, healing, and health bonuses from equipment.
- `PlayerMovement.cs` — Controls player movement.
- `Evade.cs` — Handles the player's evade ability and temporary damage avoidance.
- `Block.cs` — Handles damage reduction while blocking.
- `Attack.cs` — Handles player attack hit detection.
- `CharacterSelection.cs` — Handles character selection.

### Inventory and Equipment

- `Inventory.cs` — Manages inventory items, equipment, and equipment stat bonuses.
- `InventoryItem.cs` — Handles individual inventory items and health potion usage.
- `InventorySlot.cs` — Handles inventory slot interactions and item placement.
- `EquipmentPoints.cs` — Supports equipment attachment points.
- `Item.cs` — Defines item data, including item types, equipment, and stat bonuses.

### Enemies and Combat

- `Enemy.cs` — Provides shared enemy health, movement, and damage behavior.
- `EnemyMeleeCombat.cs` — Handles enemy melee combat.
- `EnemyRangedCombat.cs` — Handles enemy ranged attacks.
- `BasicMeleeMonster.cs` — Implements basic melee enemy behavior.
- `BasicRangedMonster.cs` — Implements basic ranged enemy behavior.
- `EliteMeleeMonster.cs` — Implements elite melee enemy behavior.
- `EliteRangedMonster.cs` — Implements elite ranged enemy behavior.
- `EnemyProjectile.cs` — Handles projectiles fired by ranged enemies.
- `AcidEffect.cs` — Handles the mid-boss acid projectile.
- `MonsterGrenade.cs` — Handles the elite enemy grenade attack.
- `MidbossCombat.cs` — Controls the mid-boss's attacks and combat behavior.
- `FinalBossCombat.cs` — Controls the final boss's attacks and combat behavior.
- `FinalBoss.cs` — Handles final-boss behavior and defeat.

### World and Story Progression

- `MidBoss.cs` — Handles mid-boss interaction, defeat, and boss-key rewards.
- `Door.cs` — Handles door interactions and required-key checks.
- `Collectible.cs` — Handles collectible items.
- `OpenShop.cs` — Opens and closes the shop interface when the player enters or exits its trigger.
- `ObjectiveTracker.cs` — Displays and updates the player's current objective.
- `DialogueManager.cs` — Handles dialogue display.
- `FeedbackNotification.cs` — Displays gameplay feedback notifications.

## Story Progression

The main progression sequence is:

1. Find and defeat the mid-boss.
2. Obtain the boss key.
3. Use the key to access the final-boss area.
4. Reach and defeat the final boss.
5. Complete the game and reach the victory screen.

If the player's inventory is full when the mid-boss is defeated, the boss-key reward remains pending. The game attempts to award the key again after the player successfully uses a health potion and frees an inventory slot.

## Inventory and Equipment

The inventory supports different item types, including health potions and equipment.

Equipment can be assigned to three slots:

- Head
- Chest
- Shield

Equipped items contribute bonuses to the player's:

- Health
- Attack
- Defense

Health potions restore player health when used successfully.

## Audio

The game uses background music and sound effects throughout gameplay.

Audio includes:

- Main menu music
- Exploration music
- Game-over music
- Victory music
- Button click sounds
- Inventory equip and unequip sounds
- Item drag and drop sounds
- Shop opening and closing sounds
- Enemy and boss attack sounds

## Development Information

- **Game Engine:** Unity
- **Programming Language:** C#
- **Project Type:** 3D game with UI-based inventory and menu systems

