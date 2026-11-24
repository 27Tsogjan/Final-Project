# Unity Setup & Implementation Guide
## Requirements for the Game Project

This guide outlines the Unity objects, components, Inspector references, animations, audio clips, and UI connections needed to support the current C# scripts.

The goal is to make sure all required Unity elements exist and are connected correctly so the game's systems work as intended.

---

# 1. Scene Setup

The project uses five scenes. Their build indices must remain consistent with the scene-loading code.

| Build Index | Scene |
|---|---|
| 0 | Title |
| 1 | Character Select |
| 2 | Main Game |
| 3 | Game Over |
| 4 | Win |

**Important:** Do not change the scene order without updating the corresponding `SceneManager.LoadScene()` calls.

# 2. Required Game Managers

## GameManager

The main game scene needs a GameManager object with:

- `GameManager.cs`
- A reference to `MenuManager`
- A reference to `ObjectiveTracker`
- An exploration music AudioClip

The GameManager controls:

- Story progression
- Game-over state
- Victory state
- Objective initialization

## SoundManager

The project needs a SoundManager that provides:

- A static `SoundManager.Instance`
- `Play(AudioClip clip)`
- `PlayMusic(AudioClip clip)`

These methods are called by several scripts. The SoundManager must be available when those scripts attempt to play audio.

# 3. Player Setup

The player requires the components used by the player scripts.

## Required scripts and components

- `PlayerHealth.cs`
- `PlayerMovement.cs`
- `Evade.cs`
- `Block.cs`
- The player's attack script
- A Rigidbody
- The appropriate Collider components

The movement and evade scripts use the player's Rigidbody to control movement.

## Health UI

The PlayerHealth script uses a Unity UI Slider.

The health Slider should:

- Be assigned to the `healthBar` field.
- Display the current health.
- Use the player's maximum health as its maximum value.

The health system also supports bonuses from equipped equipment. When equipment changes the player's maximum health, the current health is adjusted and clamped to the new maximum. If the resulting health reaches zero, the player's defeat logic should run.

# 4. Inventory Setup

The inventory requires the following scripts and references.

## Inventory object

Attach `Inventory.cs` to the inventory manager object.

Its references include:

- Inventory Slots array
- Draggables Transform
- Item Prefab
- Item List
- Give Item Button, if used
- Head Equipment Point
- Chest Equipment Point
- Shield Equipment Point

## Inventory slots

Each inventory slot requires:

- `InventorySlot.cs`
- A UI object that can receive pointer clicks
- The appropriate `SlotTag`

Supported slot tags include:

- `None`
- `Head`
- `Chest`
- `Shield`

The Inventory array should contain the normal inventory slots used for item storage. If equipment slots are included in this array, the item-spawning code must exclude them so ordinary items are not placed in equipment slots.

## Inventory item prefab

The item prefab requires:

- `InventoryItem.cs`
- A UI Image
- A CanvasGroup
- An item icon Image

The prefab must be assigned to the Inventory's `itemPrefab` field.

## Item data

Each Item asset should have the appropriate data assigned:

- Item name
- Sprite
- Item type
- Equipment slot tag, if applicable
- Equipment prefab, if applicable
- Health restoration amount, if it is a potion
- Health bonus
- Attack bonus
- Defense bonus

## Equipment attachment points

The player needs separate attachment points for:

- Head equipment
- Chest equipment
- Shield equipment

Equipment prefabs should be positioned and oriented appropriately for their attachment points.

# 5. Combat and Animations

The combat system needs animation clips and Animator Controllers that match the animation calls made by the combat scripts.

## Player animations

The relevant animations should include:

| Animation | Purpose |
|---|---|
| Idle | Default standing animation |
| Walk/Run | Movement |
| Attack | Player attack |
| Block | Defensive action |
| Evade/Dodge | Avoiding incoming damage |
| Death | Player defeat |

The Animator Controller should have transitions between these states based on the parameters used by the movement and combat scripts.

**Important:** Animator parameter names and types must match the exact names used in the scripts. Do not assume parameter names without checking the Animator calls.

## Mid-boss and final boss animations

The boss Animator Controllers should support the animations required by their combat scripts.

Recommended animation states:

- Idle
- Movement
- Attack
- Hit reaction
- Death

If a boss attack animation uses an Animation Event to apply damage, the event must call the correct method at the correct frame.

## Animation Events

The combat scripts use Animation Events to trigger attack actions, damage, sounds, and attack completion.

Examples of event methods include:

**MidbossCombat.cs**

- `Fire`
- `DealMeleeDamage`
- `DealGroundSlamDamage`
- `PlayAcidSound`
- `PlayMeleeSound`
- `PlayGroundSlamSound`
- `EndAttack`

**EliteRangedMonster.cs**

- `FireRanged`
- `Throw`
- `PlayGrenadeSound`
- `PlayRangedSound`
- `EndAttack`

**FinalBossCombat.cs**

- `FireAcid`
- `FireIce`
- `Throw`
- `DealMeleeDamage`
- `DealBiteDamage`
- `DealGroundSlamDamage`
- `PlayAcidSound`
- `PlayIceSound`
- `PlayMeleeSound`
- `PlayBiteSound`
- `PlayGroundSlamSound`
- `PlayGrenadeSound`
- `EndAttack`

Each Animation Event must use the exact method name available in the receiving script. The event must be placed on the GameObject that has the script containing that method.

Attack animations must call `EndAttack()` at the appropriate point so the enemy can resume attacking.

Do not add Animation Events for methods that do not exist.

# 6. Sound Effects and Music

Audio clips are referenced directly by several scripts. The corresponding AudioClip fields should be assigned.

## Background music

| Audio Clip | Script |
|---|---|
| Main Menu Music | `MenuManager.cs` |
| Exploration Music | `GameManager.cs` |
| Game Over Music | `GameOverScreen.cs` |
| Victory Music | `WinScreen.cs` |

## UI sound effects

| Sound Effect | Script |
|---|---|
| Button Click | `MenuManager.cs` |
| Button Click | `PauseMenu.cs` |
| Button Click | `GameOverScreen.cs` |
| Button Click | `WinScreen.cs` |

## Inventory sound effects

| Sound Effect | Script |
|---|---|
| Equip | `Inventory.cs` |
| Unequip | `Inventory.cs` |
| Drag | `Inventory.cs` |
| Drop | `Inventory.cs` |

## Shop sound effects

| Sound Effect | Script |
|---|---|
| Open Shop | `OpenShop.cs` |
| Close Shop | `OpenShop.cs` |

## Combat sound effects

The combat scripts include sound-playing methods for enemy and boss attacks. Assign the corresponding AudioClips to the relevant script fields.

The grenade explosion sound is handled by `MonsterGrenade.cs`, which plays its assigned explosion sound when the grenade detonates.

Other sounds, such as player attack, blocking, taking damage, and evading, are optional unless the corresponding scripts contain AudioClip fields or sound-playing calls.

# 7. Shop Setup

The shop object uses `OpenShop.cs`.

It requires:

- A Collider configured as a trigger
- A reference to the shop panel
- An opening sound effect
- A closing sound effect

The player must use the `Player` tag because the script checks `CompareTag("Player")`.

The shop panel is initially hidden by the script and becomes visible when the player enters the trigger.

# 8. Objective Tracker

The project requires an `ObjectiveTracker` with a working `SetObjective(string)` method.

It is used to display objectives such as:

- Find and defeat the mid-boss.
- Use the boss key to reach the final boss.
- Make room in your inventory to receive the boss key.

The objective text should update when the relevant progression events occur.

# 9. Menus and UI Connections

## Pause Menu

The pause menu needs buttons connected to:

- Pause
- Continue
- Restart
- Main Menu

## Game Over Screen

The Game Over screen needs:

- Restart button
- Main Menu button
- Game Over music

## Win Screen

The Win screen needs:

- Restart button
- Main Menu button
- Victory music

## Menu Manager

The MenuManager methods should be connected to their corresponding UI buttons.

The following actions are supported:

- Character Select
- Pause menu actions
- Win screen actions
- Game Over screen actions

Button callbacks should point to the correct public methods.

# 10. Important Gameplay Checks

Before considering the Unity setup complete, verify the following:

- [ ] All five scenes are included and have the correct build indices.
- [ ] SoundManager exposes the methods used by the scripts.
- [ ] GameManager references are connected.
- [ ] Player health Slider is assigned.
- [ ] Inventory slots are populated in the Inventory array.
- [ ] Equipment slots are excluded from ordinary item spawning if they are included in the Inventory array.
- [ ] Inventory item prefab has the required components.
- [ ] Equipment attachment points are assigned.
- [ ] Item assets have their correct data.
- [ ] Boss key Item asset is assigned to MidBoss.
- [ ] ObjectiveTracker is assigned.
- [ ] Shop panel and audio clips are assigned.
- [ ] UI buttons call the correct public methods.
- [ ] Animator parameters match the scripts.
- [ ] Required Animation Events call existing methods.
- [ ] Required AudioClips are assigned.
- [ ] Player and relevant enemies use the tags checked by the scripts.
- [ ] Evading temporarily takes control of player movement.
- [ ] Freezing the player prevents movement and evading.
- [ ] Projectiles ignore their owner and unrelated trigger colliders.
- [ ] The grenade explodes at its intended target and plays its explosion sound.
- [ ] Removing health bonuses updates player health correctly and triggers defeat if health reaches zero.
- [ ] Closing the final-boss door does not change story progression.

# 11. Final Testing Checklist

Test the following gameplay sequences:

1. Start the game and verify that the correct music plays.
2. Select a character and enter the main game.
3. Move, attack, block, and evade.
4. Take damage and verify that the health bar updates.
5. Use a health potion and verify that health is restored.
6. Equip and unequip items and verify that stat bonuses update.
7. Open and close the shop.
8. Defeat the mid-boss and verify that the boss key is awarded.
9. Fill the inventory before defeating the mid-boss and verify that the key can be awarded after successfully consuming a health potion.
10. Reach and defeat the final boss.
11. Verify the Win screen and Game Over screen.
12. Test all restart and main menu buttons.
13. Verify all corresponding sound effects and music.
14. Test evading while moving in different directions.
15. Freeze the player and verify that movement and evading are prevented.
16. Test projectiles against their owner, unrelated objects, and valid damageable targets.
17. Test grenade explosion timing, damage radius, and sound.
18. Equip and remove health-bonus equipment and verify the health behavior.
19. Open and close the final-boss door and verify that story progression updates only when it is opened.

---

## Important Note

This guide distinguishes between requirements directly supported by the reviewed scripts and recommended additions.

Animation names, Animator parameters, Animation Events, and additional combat sounds must be verified against the actual combat scripts before implementation. Do not create arbitrary parameter names or event methods that the code does not use.

The gameplay checks should be tested in the running project to confirm that the scripts work together as intended.