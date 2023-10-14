using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatController : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSlot;


    //insert code to determine effective weapon output here;
    //melee part variables - dmg, def, aoe, cooldown, critRate;
    //ranged part variables - dmg, impact, fireRate, accuracy, bullets, reloadTime, critRate;

    public int GetEffectiveDMG()
    {
        int effectiveDMG = 0;

        if (weaponSlot.weaponType == "melee") //if melee weapon;
        {
            effectiveDMG = Mathf.RoundToInt(weaponSlot.m_pommel.dmg + weaponSlot.m_grip.dmg + weaponSlot.m_crossGuard.dmg + weaponSlot.m_blade.dmg);
        }

        if (weaponSlot.weaponType == "ranged") //if ranged weapon;
        {
            //effectiveDMG = Mathf.RoundToInt(weaponSlot.m_pommel.dmg + weaponSlot.m_grip.dmg + weaponSlot.m_crossGuard.dmg + weaponSlot.m_blade.dmg);
        }



        return effectiveDMG;

    }









}
