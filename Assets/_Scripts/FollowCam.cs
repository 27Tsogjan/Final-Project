/*  FollowCam.cs
    Camera will follow the player at all times.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;
    public GameObject player;

    [Header("Set Dynamically")]
    public float camZ;

    void Awake() {
        camZ = this.transform.position.z;
    }

    void FixedUpdate() {
        if (player == null) {
            return;
        }

        Vector3 destination = player.transform.position;
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;
        this.transform.position = destination;
    }
}