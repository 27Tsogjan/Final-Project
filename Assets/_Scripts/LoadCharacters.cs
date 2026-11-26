/*  LoadCharacters.cs
    Allows player to select the character they want to play as.

    Reference:
    RumpledCode. "Unity Tutorial - Simple Character Selection System." YouTube.
    https://www.youtube.com/watch?v=3qlRgICRoeA

    Adapted for this game project.
*/
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class LoadCharacters : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform spawnPoint;
    public TMP_Text label;

    void Start() {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter", 0);

        GameObject prefab = characterPrefabs[selectedCharacter];
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        
        if (label != null) {
            label.text = prefab.name;
        }
        
        FollowCam cam = FindObjectOfType<FollowCam>();
        if (cam != null) {
            cam.player = clone;
        }

        EquipmentPoints points = clone.GetComponent<EquipmentPoints>();

        if (points != null && Inventory.Singleton != null) {
            Inventory.Singleton.headEquipmentPoint = points.headEquipmentPoint;
            Inventory.Singleton.chestEquipmentPoint = points.chestEquipmentPoint;
            Inventory.Singleton.shieldEquipmentPoint = points.shieldEquipmentPoint;
        }

        PlayerInteraction pi = clone.GetComponent<PlayerInteraction>();
        if (pi != null) {
            pi.interactionPrompt = FindObjectOfType<InteractionPrompt>();
        }
    }
}