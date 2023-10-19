using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigationManager : MonoBehaviour
{
    //======================== Setup ========================
    [SerializeField] JAR_StatController playerStat;

    [SerializeField] Transform weaponMenuPanel;
    [SerializeField] TextMeshProUGUI weaponPartSelectedText;
    [SerializeField] GameObject defaultMenuButtonA; //this is the same as magazine;
    [SerializeField] GameObject defaultMenuButtonB;

    [SerializeField] GameObject MenuButtonA_grip;
    [SerializeField] GameObject MenuButtonA_trigger;
    [SerializeField] GameObject MenuButtonA_barrel;
    [SerializeField] GameObject MenuButtonA_sight;
    [SerializeField] GameObject MenuButtonA_underbarrel;
    [SerializeField] GameObject MenuButtonA_muzzle;

    [SerializeField] TextMeshProUGUI weaponPartHeaderB;

    [SerializeField] List<TextMeshProUGUI> equippedTexts;
    [SerializeField] List<GameObject> partsDisplayButtons;
    [SerializeField] List<TextMeshProUGUI> statDifferenceTexts;

    [SerializeField] AudioSource equipSFX;

    private bool inMenuA = true; //used to check if in the outer menu or the detailed menu;


    private void Update()
    {

        //BANDAID fix for "clicking while in menu" issue;
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (inMenuA) { EventSystem.current.SetSelectedGameObject(defaultMenuButtonA); }
            else { EventSystem.current.SetSelectedGameObject(defaultMenuButtonB); }
        }

    }

    //======================== Button Functions ========================

    public void OnButtonHover_WeaponPart(string weaponPartName)
    {
        weaponPartSelectedText.text = weaponPartName;
    }


    public void OnButtonPress_WeaponPart()
    {
        //standard processing of menu A;
        weaponMenuPanel.DOLocalMoveX(800f, 0.5f, true).SetUpdate(true);
        inMenuA = false;

        //JAR: set up default button placement on menu open;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultMenuButtonB);


        //processing menu B (according to part type);
        ProcessMenuB();

    }


    public void ProcessMenuB()
    {

        //NOTE: later can put a check here to switch between melee and ranged by checking SO;

        RevertButtonColour();
        weaponPartHeaderB.text = weaponPartSelectedText.text; //set the header name accordingly;

        //populate the parts with the respective names and SO;
        List<ranged_Parts> targetParts = new List<ranged_Parts>();

        //finds all existing parts based on part type;
        for (int counter = 0; counter < playerStat.weaponSlot.myParts_r.Count; counter++)
        {
            if (playerStat.weaponSlot.myParts_r[counter].partType == weaponPartHeaderB.text)
            {
                targetParts.Add(playerStat.weaponSlot.myParts_r[counter]);
            }
        }

        //set the button parts accordingly based on the targetParts variable;
        for (int counter = 0; counter < targetParts.Count; counter++)
        {
            partsDisplayButtons[counter].GetComponent<buttonContent>().myParts_r = targetParts[counter]; //set the actual SO;
            partsDisplayButtons[counter].GetComponentInChildren<TextMeshProUGUI>().text = targetParts[counter].partName; //set the name;
            partsDisplayButtons[counter].GetComponent<buttonContent>().myParts_r = targetParts[counter]; //set the content of the parts script;

            //if parts match what is on gun, make button green;


            if (weaponPartHeaderB.text == "magazine")
            {
                if (partsDisplayButtons[counter].GetComponent<buttonContent>().myParts_r.partName == playerStat.weaponSlot.r_magazine.partName)
                {
                    HighlightEquipped(partsDisplayButtons[counter]);
                    ApplyEquipText(equippedTexts[counter]);
                }
            }

            if (weaponPartHeaderB.text == "grip")
            {
                if (partsDisplayButtons[counter].GetComponent<buttonContent>().myParts_r.partName == playerStat.weaponSlot.r_grip.partName)
                {
                    HighlightEquipped(partsDisplayButtons[counter]);
                    ApplyEquipText(equippedTexts[counter]);
                }
            }

            if (weaponPartHeaderB.text == "trigger")
            {
                if (partsDisplayButtons[counter].GetComponent<buttonContent>().myParts_r.partName == playerStat.weaponSlot.r_trigger.partName)
                {
                    HighlightEquipped(partsDisplayButtons[counter]);
                    ApplyEquipText(equippedTexts[counter]);
                }
            }

            if (weaponPartHeaderB.text == "barrel")
            {
                if (partsDisplayButtons[counter].GetComponent<buttonContent>().myParts_r.partName == playerStat.weaponSlot.r_barrel.partName)
                {
                    HighlightEquipped(partsDisplayButtons[counter]);
                    ApplyEquipText(equippedTexts[counter]);
                }
            }

            if (weaponPartHeaderB.text == "sight")
            {
                if (partsDisplayButtons[counter].GetComponent<buttonContent>().myParts_r.partName == playerStat.weaponSlot.r_sight.partName)
                {
                    HighlightEquipped(partsDisplayButtons[counter]);
                    ApplyEquipText(equippedTexts[counter]);
                }
            }

            if (weaponPartHeaderB.text == "underbarrel")
            {
                if (partsDisplayButtons[counter].GetComponent<buttonContent>().myParts_r.partName == playerStat.weaponSlot.r_underbarrel.partName)
                {
                    HighlightEquipped(partsDisplayButtons[counter]);
                    ApplyEquipText(equippedTexts[counter]);
                }
            }

            if (weaponPartHeaderB.text == "muzzle")
            {
                if (partsDisplayButtons[counter].GetComponent<buttonContent>().myParts_r.partName == playerStat.weaponSlot.r_muzzle.partName)
                {
                    HighlightEquipped(partsDisplayButtons[counter]);
                    ApplyEquipText(equippedTexts[counter]);
                }
            }
        }

    }


    public void HighlightEquipped(GameObject button)
    {
        //change button normal and selected colour;
        ColorBlock myButtonColour = button.GetComponent<Button>().colors;
        myButtonColour.normalColor = new Color32(162, 219, 64, 200);
        myButtonColour.selectedColor = new Color32(162, 219, 64, 255);
        button.GetComponent<Button>().colors = myButtonColour;
    }


    public void ApplyEquipText(TextMeshProUGUI equipText)
    {
        //update the equip status;
        equipText.text = "(EQUIPPED)";
    }


    public void OnPartSelect(GameObject button)
    {
        //play equip sound;
        equipSFX.Play();

        //check partname;
        if (weaponPartHeaderB.text == "magazine")
        {
            //swap out current part with selected part;
            playerStat.weaponSlot.r_magazine = button.GetComponent<buttonContent>().myParts_r;
        }

        if (weaponPartHeaderB.text == "grip")
        {
            //swap out current part with selected part;
            playerStat.weaponSlot.r_grip = button.GetComponent<buttonContent>().myParts_r;
        }

        if (weaponPartHeaderB.text == "trigger")
        {
            //swap out current part with selected part;
            playerStat.weaponSlot.r_trigger = button.GetComponent<buttonContent>().myParts_r;
        }

        if (weaponPartHeaderB.text == "barrel")
        {
            //swap out current part with selected part;
            playerStat.weaponSlot.r_barrel = button.GetComponent<buttonContent>().myParts_r;
        }

        if (weaponPartHeaderB.text == "sight")
        {
            //swap out current part with selected part;
            playerStat.weaponSlot.r_sight = button.GetComponent<buttonContent>().myParts_r;
        }

        if (weaponPartHeaderB.text == "underbarrel")
        {
            //swap out current part with selected part;
            playerStat.weaponSlot.r_underbarrel = button.GetComponent<buttonContent>().myParts_r;
        }

        if (weaponPartHeaderB.text == "muzzle")
        {
            //swap out current part with selected part;
            playerStat.weaponSlot.r_muzzle = button.GetComponent<buttonContent>().myParts_r;
        }

    }


    public void RevertButtonColour()
    {
        //tag the currently equipped button;
        GameObject exemptButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        //reset the colours of anything other than the currently equipped button;
        for (int counter = 0; counter < partsDisplayButtons.Count; counter++)
        {
            if (partsDisplayButtons[counter] != exemptButton)
            {
                //revert button normal and selected colour;
                ColorBlock myButtonColour = partsDisplayButtons[counter].GetComponent<Button>().colors;
                myButtonColour.normalColor = new Color32(125, 125, 125, 255);
                myButtonColour.selectedColor = new Color32(245, 245, 245, 255);
                partsDisplayButtons[counter].GetComponent<Button>().colors = myButtonColour;

                //revert any equipped texts;
                equippedTexts[counter].text = "";
            }
        }
    }


    public void OnBackPress_WeaponPart()
    {
        weaponMenuPanel.DOLocalMoveX(0f, 0.5f, true).SetUpdate(true);
        inMenuA = true;

        //memory cursor - sends you back to the correct category button before switching to menu B;
        if (weaponPartHeaderB.text == "magazine")
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultMenuButtonA);
        }

        if (weaponPartHeaderB.text == "grip")
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(MenuButtonA_grip);
        }

        if (weaponPartHeaderB.text == "trigger")
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(MenuButtonA_trigger);
        }

        if (weaponPartHeaderB.text == "barrel")
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(MenuButtonA_barrel);
        }

        if (weaponPartHeaderB.text == "sight")
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(MenuButtonA_sight);
        }

        if (weaponPartHeaderB.text == "underbarrel")
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(MenuButtonA_underbarrel);
        }

        if (weaponPartHeaderB.text == "muzzle")
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(MenuButtonA_muzzle);
        }

    }


    public void PreviewStatChanges()
    {
        //get the item name and description and populate the UI (intentionally left blank for now);

        //remember original part;
        var partMemory = playerStat.weaponSlot.r_magazine;

        //save current stat, index 0 - 6: dmg, impact, fireRate, accuracy, bullets, reloadTime, critRate;
        List<float> currentStats = new List<float>();

        currentStats.Add(playerStat.GetEffectiveDMG());
        currentStats.Add(playerStat.GetEffectiveImpact());
        currentStats.Add(playerStat.GetEffectiveFireRate());
        currentStats.Add(playerStat.GetEffectiveAccuracy());
        currentStats.Add(playerStat.GetEffectiveBullets());
        currentStats.Add(playerStat.GetEffectiveReloadTime());
        currentStats.Add(playerStat.GetEffectiveCritRate());


        //set new parts for preview;
        if (weaponPartHeaderB.text == "magazine")
        {
            partMemory = playerStat.weaponSlot.r_magazine;
            playerStat.weaponSlot.r_magazine = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<buttonContent>().myParts_r;
        }

        if (weaponPartHeaderB.text == "grip")
        {
            partMemory = playerStat.weaponSlot.r_grip;
            playerStat.weaponSlot.r_grip = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<buttonContent>().myParts_r;
        }

        if (weaponPartHeaderB.text == "trigger")
        {
            partMemory = playerStat.weaponSlot.r_trigger;
            playerStat.weaponSlot.r_trigger = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<buttonContent>().myParts_r;
        }

        if (weaponPartHeaderB.text == "barrel")
        {
            partMemory = playerStat.weaponSlot.r_barrel;
            playerStat.weaponSlot.r_barrel = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<buttonContent>().myParts_r;
        }

        if (weaponPartHeaderB.text == "sight")
        {
            partMemory = playerStat.weaponSlot.r_sight;
            playerStat.weaponSlot.r_sight = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<buttonContent>().myParts_r;
        }

        if (weaponPartHeaderB.text == "underbarrel")
        {
            partMemory = playerStat.weaponSlot.r_underbarrel;
            playerStat.weaponSlot.r_underbarrel = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<buttonContent>().myParts_r;
        }

        if (weaponPartHeaderB.text == "muzzle")
        {
            partMemory = playerStat.weaponSlot.r_muzzle;
            playerStat.weaponSlot.r_muzzle = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<buttonContent>().myParts_r;
        }


        //save new stat
        List<float> newStats = new List<float>();

        newStats.Add(playerStat.GetEffectiveDMG());
        newStats.Add(playerStat.GetEffectiveImpact());
        newStats.Add(playerStat.GetEffectiveFireRate());
        newStats.Add(playerStat.GetEffectiveAccuracy());
        newStats.Add(playerStat.GetEffectiveBullets());
        newStats.Add(playerStat.GetEffectiveReloadTime());
        newStats.Add(playerStat.GetEffectiveCritRate());


        //revert part;
        if (weaponPartHeaderB.text == "magazine")
        {
            playerStat.weaponSlot.r_magazine = partMemory;
        }

        if (weaponPartHeaderB.text == "grip")
        {
            playerStat.weaponSlot.r_grip = partMemory;
        }

        if (weaponPartHeaderB.text == "trigger")
        {
            playerStat.weaponSlot.r_trigger = partMemory;
        }

        if (weaponPartHeaderB.text == "barrel")
        {
            playerStat.weaponSlot.r_barrel = partMemory;
        }

        if (weaponPartHeaderB.text == "sight")
        {
            playerStat.weaponSlot.r_sight = partMemory;
        }

        if (weaponPartHeaderB.text == "underbarrel")
        {
            playerStat.weaponSlot.r_underbarrel = partMemory;
        }

        if (weaponPartHeaderB.text == "muzzle")
        {
            playerStat.weaponSlot.r_muzzle = partMemory;
        }

        //compare stats and print positives and negative changes;
        PrintStats(currentStats, newStats);

    }


    public void PrintStats(List<float> currentStats, List<float> newStats)
    {
        for (int counter = 0; counter < newStats.Count; counter++)
        {
            statDifferenceTexts[counter].text = newStats[counter].ToString();

            //adjust colour accordingly;
            if (newStats[counter] > currentStats[counter]) { statDifferenceTexts[counter].color = new Color32(94, 197, 0, 255); }
            else if (newStats[counter] < currentStats[counter]) { statDifferenceTexts[counter].color = new Color32(217, 0, 0, 255); }
            else { statDifferenceTexts[counter].color = new Color32(255, 255, 255, 255); }
        }
    }


    //BANDAID;
    public void PrintCurrentStats()
    {
        //save current stat, index 0 - 6: dmg, impact, fireRate, accuracy, bullets, reloadTime, critRate;
        List<float> currentStats = new List<float>();

        currentStats.Add(playerStat.GetEffectiveDMG());
        currentStats.Add(playerStat.GetEffectiveImpact());
        currentStats.Add(playerStat.GetEffectiveFireRate());
        currentStats.Add(playerStat.GetEffectiveAccuracy());
        currentStats.Add(playerStat.GetEffectiveBullets());
        currentStats.Add(playerStat.GetEffectiveReloadTime());
        currentStats.Add(playerStat.GetEffectiveCritRate());

        for (int counter = 0; counter < currentStats.Count; counter++)
        {
            statDifferenceTexts[counter].text = currentStats[counter].ToString();
            statDifferenceTexts[counter].color = new Color32(255, 255, 255, 255);
        }
    }





    //Debug.Log(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);


}
