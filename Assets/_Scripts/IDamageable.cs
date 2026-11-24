/*  IDamageable.cs
    Damage interfact.

    Reference:
    Davis, Micha L. "Modular Enemies Using Abstract Classes and Interfaces in Unity." Medium.
    https://micha-l-davis.medium.com/modular-enemies-using-abstract-classes-and-interfaces-in-unity-6fa833057ecc

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    int Health { get; set; }
    void Damage(int damageAmount);
}