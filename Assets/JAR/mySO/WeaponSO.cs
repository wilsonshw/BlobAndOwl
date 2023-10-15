using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "NewWeapon")]
public class WeaponSO : ScriptableObject
{
    public string weaponName = "Name";
    public string weaponType = "melee/ranged"; //melee/ranged

    [TextAreaAttribute(5, 8)] public string description;

    [Header("Melee Parts")]
    public melee_Parts m_pommel;
    public melee_Parts m_grip;
    public melee_Parts m_crossGuard;
    public melee_Parts m_blade;

    [Header("Ranged Parts")]
    public ranged_Parts r_magazine;
    public ranged_Parts r_grip;
    public ranged_Parts r_trigger;
    public ranged_Parts r_barrel;
    public ranged_Parts r_muzzle;
    public ranged_Parts r_underbarrel;
    public ranged_Parts r_sight;


}
