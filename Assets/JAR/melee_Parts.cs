using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Parts (Melee)", menuName = "NewPart(M)")]
public class melee_Parts : ScriptableObject
{
    public string partName = "Name";
    public string partType = "blade/pommel/grip_m/crossguard/blade"; //pommel, grip_m, crossguard, blade

    [Header("Stats")]
    public float dmg;
    public float def;
    public float aoe;
    public float cooldown;
    public float critRate;

}
