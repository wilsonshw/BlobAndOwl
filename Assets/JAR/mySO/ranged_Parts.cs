using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Parts (Ranged)", menuName = "NewPart(R)")]
public class ranged_Parts : ScriptableObject
{

    public string partName = "Name";
    public string partType = "barrel/grip_r/magazine/muzzle/sight/trigger/underbarrel"; // barrel, grip_r, magazine, muzzle, sight, trigger, underbarrel;

    [Header("Stats")]
    public float dmg;
    public float impact;
    public float fireRate;
    public float accuracy;
    public float bullets;
    public float reloadTime;
    public float critRate;

}
