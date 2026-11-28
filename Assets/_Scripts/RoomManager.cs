using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [Header("Room Prefabs")]
    public GameObject startRoom;
    public GameObject[] genericRooms;
    public GameObject midbossRoom;
    public GameObject gateRoom;
    public GameObject finalBossRoom;

    [Header("Generic rooms before the midboss")]
    public int minRooms = 6;
    public int maxRooms = 8;

    [Header("Generic rooms between the midboss and the gate")]
    public int minRoomsAfter = 2;
    public int maxRoomsAfter = 3;

    private GameObject currentRoom;
    private int lastIndex = -1;
    private int roomsVisited = 0;
    private int roomsNeeded;
    private int roomsNeededAfter;
    private bool midbossReached = false;

    private void Awake() {
        Instance = this;
    }

    private IEnumerator Start() {
        roomsNeeded = Random.Range(minRooms, maxRooms + 1);
        roomsNeededAfter = Random.Range(minRoomsAfter, maxRoomsAfter + 1);
        Debug.Log("Rooms before midboss: " + roomsNeeded + ", rooms after: " + roomsNeededAfter);

        // Wait a frame so LoadCharacters has spawned the hero
        yield return null;
        LoadRoom(startRoom, DoorSide.Bottom);
    }

    // exitedThrough = the side of the room the hero just left by
    public void NextRoom(DoorSide exitedThrough) {
        DoorSide entrySide = Room.Opposite(exitedThrough);

        if (!midbossReached) {
            if (roomsVisited >= roomsNeeded) {
                midbossReached = true;
                roomsVisited = 0;
                LoadRoom(midbossRoom, entrySide);
                return;
            }
        }
        else if (roomsVisited >= roomsNeededAfter) {
            LoadRoom(gateRoom, entrySide);
            return;
        }

        roomsVisited++;

        int index = Random.Range(0, genericRooms.Length);

        // Avoid the same layout twice in a row
        if (genericRooms.Length > 1) {
            while (index == lastIndex) {
                index = Random.Range(0, genericRooms.Length);
            }
        }

        lastIndex = index;
        LoadRoom(genericRooms[index], entrySide);
    }

    public void LoadFinalBossRoom() {
        LoadRoom(finalBossRoom, DoorSide.Left);
    }

    private void LoadRoom(GameObject prefab, DoorSide entrySide) {
        if (prefab == null) {
            Debug.LogError("RoomManager: a room prefab slot is empty.");
            return;
        }

        if (currentRoom != null) {
            Destroy(currentRoom);
        }

        currentRoom = Instantiate(prefab, Vector3.zero, Quaternion.identity);

        Room room = currentRoom.GetComponent<Room>();
        GameObject hero = GameObject.FindWithTag("Player");

        if (hero == null || room == null) {
            return;
        }

        Transform entry = room.GetEntry(entrySide);

        if (entry == null) {
            Debug.LogWarning("Room has no entry point for " + entrySide);
            return;
        }

        Vector3 pos = entry.position;
        pos.z = hero.transform.position.z;
        hero.transform.position = pos;

        Rigidbody rb = hero.GetComponent<Rigidbody>();

        if (rb != null) {
            rb.position = pos;
            rb.velocity = Vector3.zero;
        }
    }
}