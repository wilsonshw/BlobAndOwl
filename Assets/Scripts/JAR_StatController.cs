using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAR_StatController : MonoBehaviour
{
    [SerializeField] public WeaponSO weaponSlot;


    //insert code to determine effective weapon output here;

    //melee parts (4 total) - m_pommel, m_grip, m_crossGuard, m_blade;
    //melee part variables - dmg, def, aoe, cooldown, critRate;

    //ranged parts (7 total) - r_magazine, r_barrel, r_grip, r_ muzzle, r_sight, r_trigger, r_underbarrel;    
    //ranged part variables - dmg, impact, fireRate, accuracy, bullets, reloadTime, critRate;



    //////////////////////
    /// Common Stats    //
    //////////////////////

    //this returns the effective DMG of character's weapon;
    public float GetEffectiveDMG() 
    {
        float effectiveDMG = 0;

        if (weaponSlot.weaponType == "melee") //if melee weapon;
        {
            effectiveDMG = weaponSlot.m_pommel.dmg + weaponSlot.m_grip.dmg + weaponSlot.m_crossGuard.dmg + weaponSlot.m_blade.dmg;
        }

        if (weaponSlot.weaponType == "ranged") //if ranged weapon;
        {
            effectiveDMG = weaponSlot.r_magazine.dmg + weaponSlot.r_barrel.dmg + weaponSlot.r_grip.dmg +
                weaponSlot.r_muzzle.dmg + weaponSlot.r_sight.dmg + weaponSlot.r_trigger.dmg + weaponSlot.r_underbarrel.dmg;
        }

        return effectiveDMG;
    }


    //this returns the effective CritRate of character's weapon;
    public float GetEffectiveCritRate()
    {
        float effectiveCrit = 0;

        if (weaponSlot.weaponType == "melee") //if melee weapon;
        {
            effectiveCrit = weaponSlot.m_pommel.critRate + weaponSlot.m_grip.critRate + weaponSlot.m_crossGuard.critRate + weaponSlot.m_blade.critRate;
        }

        if (weaponSlot.weaponType == "ranged") //if ranged weapon;
        {
            effectiveCrit = weaponSlot.r_magazine.critRate + weaponSlot.r_barrel.critRate + weaponSlot.r_grip.critRate +
                weaponSlot.r_muzzle.critRate + weaponSlot.r_sight.critRate + weaponSlot.r_trigger.critRate + weaponSlot.r_underbarrel.critRate;
        }

        return effectiveCrit;
    }



    ///////////////////////////
    /// Melee Only Stats     //
    //////////////////////////

    //this returns the effective DEF of melee weapon;
    public float GetEffectiveDEF()
    {
        return weaponSlot.m_pommel.def + weaponSlot.m_grip.def + weaponSlot.m_crossGuard.def + weaponSlot.m_blade.def;
    }


    //this returns the effective AoE of melee weapon;
    public float GetEffectiveAoE()
    {
        return weaponSlot.m_pommel.aoe + weaponSlot.m_grip.aoe + weaponSlot.m_crossGuard.aoe + weaponSlot.m_blade.aoe;
    }


    //this returns the effective cooldown of melee weapon;
    public float GetEffectiveCooldown()
    {
        return weaponSlot.m_pommel.cooldown + weaponSlot.m_grip.cooldown + weaponSlot.m_crossGuard.cooldown + weaponSlot.m_blade.cooldown;
    }





    ///////////////////////////
    /// Ranged Only Stats    //
    //////////////////////////

    //this returns the effective impact of ranged weapon;
    public float GetEffectiveImpact()
    {
        return 
                weaponSlot.r_magazine.impact + weaponSlot.r_barrel.impact +
                weaponSlot.r_grip.impact + weaponSlot.r_muzzle.impact +
                weaponSlot.r_sight.impact + weaponSlot.r_trigger.impact +
                weaponSlot.r_underbarrel.impact
        ;
    }


    //this returns the effective fireRate of ranged weapon;
    public float GetEffectiveFireRate()
    {
        return 
                weaponSlot.r_magazine.fireRate + weaponSlot.r_barrel.fireRate +
                weaponSlot.r_grip.fireRate + weaponSlot.r_muzzle.fireRate +
                weaponSlot.r_sight.fireRate + weaponSlot.r_trigger.fireRate +
                weaponSlot.r_underbarrel.fireRate
        ;
    }


    //this returns the effective accuracy of ranged weapon;
    public float GetEffectiveAccuracy()
    {
        return
                weaponSlot.r_magazine.accuracy + weaponSlot.r_barrel.accuracy +
                weaponSlot.r_grip.accuracy + weaponSlot.r_muzzle.accuracy +
                weaponSlot.r_sight.accuracy + weaponSlot.r_trigger.accuracy +
                weaponSlot.r_underbarrel.accuracy
        ;
    }


    //this returns the effective bullets of ranged weapon;
    public float GetEffectiveBullets()
    {
        return 
                weaponSlot.r_magazine.bullets + weaponSlot.r_barrel.bullets +
                weaponSlot.r_grip.bullets + weaponSlot.r_muzzle.bullets +
                weaponSlot.r_sight.bullets + weaponSlot.r_trigger.bullets +
                weaponSlot.r_underbarrel.bullets
        ;
    }


    //this returns the effective reload time of ranged weapon;
    public float GetEffectiveReloadTime()
    {
        return 
                weaponSlot.r_magazine.reloadTime + weaponSlot.r_barrel.reloadTime +
                weaponSlot.r_grip.reloadTime + weaponSlot.r_muzzle.reloadTime +
                weaponSlot.r_sight.reloadTime + weaponSlot.r_trigger.reloadTime +
                weaponSlot.r_underbarrel.reloadTime
        ;
    }







}
