/*  Block.cs
    Player block mechanic.
    Reduces damage by 50%.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Blocking")]
    [SerializeField] private KeyCode blockKey = KeyCode.R;
    [SerializeField] private float damageReduction = 0.5f;

    public bool IsBlocking { get; private set; }

    private Animator anim;

    private void Start() {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update() {
        IsBlocking = Input.GetKey(blockKey);
        if (anim != null) {
            anim.SetBool("IsBlocking", IsBlocking);
        }
    }

    public int ReduceDamage(int incomingDamage) {
        if (!IsBlocking) {
            return incomingDamage;
        }
        return Mathf.Max(1, Mathf.CeilToInt(incomingDamage * (1f - damageReduction)));
    }
}