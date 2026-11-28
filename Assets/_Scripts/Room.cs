using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Entry points (where the hero appears)")]
    public Transform entryPoint;   // fallback if a side is left empty
    public Transform entryLeft;
    public Transform entryRight;
    public Transform entryTop;
    public Transform entryBottom;

    [Header("Random contents (generic rooms only)")]
    public GameObject[] possibleSpawns;
    public int minSpawns = 3;
    public int maxSpawns = 5;

    [Header("Spawn area (relative to the room, shown as a green box)")]
    public Vector2 areaCenter = Vector2.zero;
    public Vector2 areaSize = new Vector2(14f, 7f);

    [Header("Spacing")]
    public float minDistanceFromEntries = 3f;
    public float minDistanceBetweenSpawns = 1.5f;
    public float obstacleCheckSize = 0.6f;

    public Transform GetEntry(DoorSide side) {
        Transform t = null;

        switch (side) {
            case DoorSide.Left: t = entryLeft; break;
            case DoorSide.Right: t = entryRight; break;
            case DoorSide.Top: t = entryTop; break;
            case DoorSide.Bottom: t = entryBottom; break;
        }

        return t != null ? t : entryPoint;
    }

    private void Start() {
        if (possibleSpawns == null || possibleSpawns.Length == 0) {
            return;
        }

        Physics.SyncTransforms();

        int count = Random.Range(minSpawns, maxSpawns + 1);
        List<Vector3> used = new List<Vector3>();

        for (int i = 0; i < count; i++) {
            Vector3 pos;

            if (!TryFindSpot(used, out pos)) {
                continue;   // no free spot found, skip this one
            }

            GameObject prefab = possibleSpawns[Random.Range(0, possibleSpawns.Length)];

            if (prefab != null) {
                Instantiate(prefab, pos, Quaternion.identity, transform);
                used.Add(pos);
            }
        }
    }

    private bool TryFindSpot(List<Vector3> used, out Vector3 result) {
        for (int attempt = 0; attempt < 30; attempt++) {
            float x = Random.Range(-areaSize.x / 2f, areaSize.x / 2f) + areaCenter.x;
            float y = Random.Range(-areaSize.y / 2f, areaSize.y / 2f) + areaCenter.y;
            Vector3 pos = transform.position + new Vector3(x, y, 0f);

            if (TooCloseToEntry(pos) || TooCloseToOthers(pos, used) || HitsObstacle(pos)) {
                continue;
            }

            result = pos;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private bool TooCloseToEntry(Vector3 pos) {
        Transform[] entries = { entryPoint, entryLeft, entryRight, entryTop, entryBottom };

        foreach (Transform e in entries) {
            if (e != null && Vector2.Distance(pos, e.position) < minDistanceFromEntries) {
                return true;
            }
        }

        return false;
    }

    private bool TooCloseToOthers(Vector3 pos, List<Vector3> used) {
        foreach (Vector3 u in used) {
            if (Vector2.Distance(pos, u) < minDistanceBetweenSpawns) {
                return true;
            }
        }

        return false;
    }

    private bool HitsObstacle(Vector3 pos) {
        Vector3 half = new Vector3(obstacleCheckSize, obstacleCheckSize, 2f);
        return Physics.CheckBox(pos, half, Quaternion.identity, ~0, QueryTriggerInteraction.Ignore);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + (Vector3)areaCenter, new Vector3(areaSize.x, areaSize.y, 0.1f));
    }

    public static DoorSide Opposite(DoorSide side) {
        switch (side) {
            case DoorSide.Left: return DoorSide.Right;
            case DoorSide.Right: return DoorSide.Left;
            case DoorSide.Top: return DoorSide.Bottom;
            default: return DoorSide.Top;
        }
    }
}