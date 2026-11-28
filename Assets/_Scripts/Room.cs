using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform entryPoint;

    [Header("Random contents (generic rooms only)")]
    public Transform[] spawnPoints;
    public GameObject[] possibleSpawns;
    [Range(0f, 1f)] public float emptyChance = 0.2f;

    private void Start() {
        if (possibleSpawns == null || possibleSpawns.Length == 0) {
            return;
        }

        foreach (Transform point in spawnPoints) {
            if (Random.value < emptyChance) {
                continue;
            }

            GameObject prefab = possibleSpawns[Random.Range(0, possibleSpawns.Length)];

            if (prefab != null) {
                Instantiate(prefab, point.position, Quaternion.identity, transform);
            }
        }
    }
}