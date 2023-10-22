using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

public class MenuNavigationManager_Melee : MonoBehaviour
{
    //======================== Setup ========================
    [SerializeField] JAR_StatController playerStat;

    [SerializeField] Transform weaponMenuPanel;
    [SerializeField] TextMeshProUGUI weaponPartSelectedText;
    [SerializeField] GameObject defaultMenuButtonA; //this is the same as pommel;
    [SerializeField] GameObject defaultMenuButtonB;

    [SerializeField] GameObject MenuButtonA_pommel;
    [SerializeField] GameObject MenuButtonA_grip;
    [SerializeField] GameObject MenuButtonA_crossguard;
    [SerializeField] GameObject MenuButtonA_blade;

    [SerializeField] TextMeshProUGUI weaponPartHeaderB;

    [SerializeField] List<TextMeshProUGUI> equippedTexts;
    [SerializeField] List<GameObject> partsDisplayButtons;
    [SerializeField] List<TextMeshProUGUI> statDifferenceTexts;

    [SerializeField] AudioSource equipSFX;

    public bool inMenuA = true; //used to check if in the outer menu or the detailed menu;
    public MultiplayerEventSystem multiEventSys;
    public Button[] menuAButtons;

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
        multiEventSys.SetSelectedGameObject(null);
        multiEventSys.SetSelectedGameObject(defaultMenuButtonB); //back button


        //processing menu B (according to part type);
        ProcessMenuB();

    }


    public void ProcessMenuB()
    {
        PrintCurrentStats();

        //NOTE: later can put a check here to switch between melee and ranged by checking SO;

        RevertButtonColour();
        weaponPartHeaderB.text = weaponPartSelectedText.text; //set the header name accordingly;

        //populate the parts with the respective names and SO;
        List<melee_Parts> targetParts = new List<melee_Parts>();

        //finds all existing parts based on part type;
        for (int counter = 0; counter < playerStat.weaponSlot.myParts_m.Count; counter++)
        {
            if (playerStat.weaponSlot.myParts_m[counter].partType == weaponPartHeaderB.text)
            {
                targetParts.Add(playerStat.weaponSlot.myParts_m[counter]);
            }
        }

        //set the button parts accordingly based on the targetParts variable;
        for (int counter = 0; counter < targetParts.Count; counter++)
        {
            partsDisplayButtons[counter].GetComponent<buttonContent>().myParts_m = targetParts[counter]; //set the actual SO;
            partsDisplayButtons[counter].GetComponentInChildren<TextMeshProUGUI>().text = targetParts[counter].partName; //set the name;
            partsDisplayButtons[counter].GetComponent<buttonContent>().myParts_m = targetParts[counter]; //set the content of the parts script;

            //if parts match what is on gun, make button green;


            if (weaponPartHeaderB.text == "pommel")
            {
                if (partsDisplayButtons[counter].GetComponent<buttonContent>().myParts_m.partName == playerStat.weaponSlot.m_pommel.partName)
                {
                    defaultMenuButtonB = partsDisplayButtons[counter];
                    HighlightEquipped(partsDisplayButtons[counter]);
                    ApplyEquipText(equippedTexts[counter]);
                }
            }

            if (weaponPartHeaderB.text == "grip")
            {
                if (partsDisplayButtons[counter].GetComponent<buttonContent>().myParts_m.partName == playerStat.weaponSlot.m_grip.partName)
                {
                    defaultMenuButtonB = partsDisplayButtons[counter];
                    HighlightEquipped(partsDisplayButtons[counter]);
                    ApplyEquipText(equippedTexts[counter]);
                }
            }

            if (weaponPartHeaderB.text == "crossguard")
            {
                if (partsDisplayButtons[counter].GetComponent<buttonContent>().myParts_m.partName == playerStat.weaponSlot.m_crossGuard.partName)
                {
                    defaultMenuButtonB = partsDisplayButtons[counter];
                    HighlightEquipped(partsDisplayButtons[counter]);
                    ApplyEquipText(equippedTexts[counter]);
                }
            }

            if (weaponPartHeaderB.text == "blade")
            {
                if (partsDisplayButtons[counter].GetComponent<buttonContent>().myParts_m.partName == playerStat.weaponSlot.m_blade.partName)
                {
                    defaultMenuButtonB = partsDisplayButtons[counter];
                    HighlightEquipped(partsDisplayButtons[counter]);
                    ApplyEquipText(equippedTexts[counter]);
                }
            }
            multiEventSys.SetSelectedGameObject(null);
            multiEventSys.SetSelectedGameObject(partsDisplayButtons[counter]);
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
        //check partname;
        if (weaponPartHeaderB.text == "pommel")
        {
            //swap out current part with selected part;
            playerStat.weaponSlot.m_pommel = button.GetComponent<buttonContent>().myParts_m;
        }

        if (weaponPartHeaderB.text == "grip")
        {
            //swap out current part with selected part;
            playerStat.weaponSlot.m_grip = button.GetComponent<buttonContent>().myParts_m;
        }

        if (weaponPartHeaderB.text == "crossguard")
        {
            //swap out current part with selected part;
            playerStat.weaponSlot.m_crossGuard = button.GetComponent<buttonContent>().myParts_m;
        }

        if (weaponPartHeaderB.text == "blade")
        {
            //swap out current part with selected part;
            playerStat.weaponSlot.m_blade = button.GetComponent<buttonContent>().myParts_m;
        }

        //play equip sound;
        equipSFX.Play();

    }

    public void RevertButtonColour()
    {
        //tag the currently equipped button;
        GameObject exemptButton = multiEventSys.currentSelectedGameObject;

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
        TweenBackHome();
        EnableButtons();
        //memory cursor - sends you back to the correct category button before switching to menu B;
        if (weaponPartHeaderB.text == "pommel")
        {
            multiEventSys.SetSelectedGameObject(null);
            multiEventSys.SetSelectedGameObject(defaultMenuButtonA);
        }

        if (weaponPartHeaderB.text == "grip")
        {
            multiEventSys.SetSelectedGameObject(null);
            multiEventSys.SetSelectedGameObject(MenuButtonA_grip);
        }

        if (weaponPartHeaderB.text == "crossguard")
        {
            multiEventSys.SetSelectedGameObject(null);
            multiEventSys.SetSelectedGameObject(MenuButtonA_crossguard);
        }

        if (weaponPartHeaderB.text == "blade")
        {
            multiEventSys.SetSelectedGameObject(null);
            multiEventSys.SetSelectedGameObject(MenuButtonA_blade);
        }
        
    }

    public void TweenBackHome()
    {
        inMenuA = true;
        weaponMenuPanel.DOLocalMoveX(0f, 0.5f, true).SetUpdate(true);
    }

    public void EnableButtons()
    {
        for (int i = 0; i < menuAButtons.Length; i++)
            menuAButtons[i].interactable = true;
    }


    public void PreviewStatChanges_Hover(GameObject hovered)
    {
        //get the item name and description and populate the UI (intentionally left blank for now);

        //remember original part;
        var partMemory = playerStat.weaponSlot.m_pommel;

        //save current stat, index 0 - 4: dmg, def, aoe, cooldown, critRate;
        List<float> currentStats = new List<float>();

        currentStats.Add(playerStat.GetEffectiveDMG());
        currentStats.Add(playerStat.GetEffectiveDEF());
        currentStats.Add(playerStat.GetEffectiveAoE());
        currentStats.Add(playerStat.GetEffectiveCooldown());
        currentStats.Add(playerStat.GetEffectiveCritRate());

        //set new parts for preview;
        if (weaponPartHeaderB.text == "pommel")
        {
            partMemory = playerStat.weaponSlot.m_pommel;
            playerStat.weaponSlot.m_pommel = hovered.GetComponent<buttonContent>().myParts_m;
        }

        if (weaponPartHeaderB.text == "grip")
        {
            partMemory = playerStat.weaponSlot.m_grip;
            playerStat.weaponSlot.m_grip = hovered.GetComponent<buttonContent>().myParts_m;
        }

        if (weaponPartHeaderB.text == "crossguard")
        {
            partMemory = playerStat.weaponSlot.m_crossGuard;
            playerStat.weaponSlot.m_crossGuard = hovered.GetComponent<buttonContent>().myParts_m;
        }

        if (weaponPartHeaderB.text == "blade")
        {
            partMemory = playerStat.weaponSlot.m_blade;
            playerStat.weaponSlot.m_blade = hovered.GetComponent<buttonContent>().myParts_m;
        }

        //save new stat;
        List<float> newStats = new List<float>();

        newStats.Add(playerStat.GetEffectiveDMG());
        newStats.Add(playerStat.GetEffectiveDEF());
        newStats.Add(playerStat.GetEffectiveAoE());
        newStats.Add(playerStat.GetEffectiveCooldown());
        newStats.Add(playerStat.GetEffectiveCritRate());


        //revert part;
        if (weaponPartHeaderB.text == "pommel")
        {
            playerStat.weaponSlot.m_pommel = partMemory;
        }

        if (weaponPartHeaderB.text == "grip")
        {
            playerStat.weaponSlot.m_grip = partMemory;
        }

        if (weaponPartHeaderB.text == "crossguard")
        {
            playerStat.weaponSlot.m_crossGuard = partMemory;
        }

        if (weaponPartHeaderB.text == "blade")
        {
            playerStat.weaponSlot.m_blade = partMemory;
        }

        //compare stats and print positives and negative changes;
        PrintStats(currentStats, newStats);

    }


    public void PreviewStatChanges()
    {
        //get the item name and description and populate the UI (intentionally left blank for now);

        //remember original part;
        var partMemory = playerStat.weaponSlot.m_pommel;

        //save current stat, index 0 - 4: dmg, def, aoe, cooldown, critRate;
        List<float> currentStats = new List<float>();

        currentStats.Add(playerStat.GetEffectiveDMG());
        currentStats.Add(playerStat.GetEffectiveDEF());
        currentStats.Add(playerStat.GetEffectiveAoE());
        currentStats.Add(playerStat.GetEffectiveCooldown());
        currentStats.Add(playerStat.GetEffectiveCritRate());


        //set new parts for preview;
        if (weaponPartHeaderB.text == "pommel")
        {
            partMemory = playerStat.weaponSlot.m_pommel;
            playerStat.weaponSlot.m_pommel = multiEventSys.currentSelectedGameObject.GetComponent<buttonContent>().myParts_m;
        }

        if (weaponPartHeaderB.text == "grip")
        {
            partMemory = playerStat.weaponSlot.m_grip;
            playerStat.weaponSlot.m_grip = multiEventSys.currentSelectedGameObject.GetComponent<buttonContent>().myParts_m;
        }

        if (weaponPartHeaderB.text == "crossguard")
        {
            partMemory = playerStat.weaponSlot.m_crossGuard;
            playerStat.weaponSlot.m_crossGuard = multiEventSys.currentSelectedGameObject.GetComponent<buttonContent>().myParts_m;
        }

        if (weaponPartHeaderB.text == "blade")
        {
            partMemory = playerStat.weaponSlot.m_blade;
            playerStat.weaponSlot.m_blade = multiEventSys.currentSelectedGameObject.GetComponent<buttonContent>().myParts_m;
        }

        //save new stat
        List<float> newStats = new List<float>();

        newStats.Add(playerStat.GetEffectiveDMG());
        newStats.Add(playerStat.GetEffectiveDEF());
        newStats.Add(playerStat.GetEffectiveAoE());
        newStats.Add(playerStat.GetEffectiveCooldown());
        newStats.Add(playerStat.GetEffectiveCritRate());


        //revert part;
        if (weaponPartHeaderB.text == "pommel")
        {
            playerStat.weaponSlot.m_pommel = partMemory;
        }

        if (weaponPartHeaderB.text == "grip")
        {
            playerStat.weaponSlot.m_grip = partMemory;
        }

        if (weaponPartHeaderB.text == "crossguard")
        {
            playerStat.weaponSlot.m_crossGuard = partMemory;
        }

        if (weaponPartHeaderB.text == "blade")
        {
            playerStat.weaponSlot.m_blade = partMemory;
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
        //save current stat, index 0 - 4: dmg, def, aoe, cooldown, critRate;
        List<float> currentStats = new List<float>();

        currentStats.Add(playerStat.GetEffectiveDMG());
        currentStats.Add(playerStat.GetEffectiveDEF());
        currentStats.Add(playerStat.GetEffectiveAoE());
        currentStats.Add(playerStat.GetEffectiveCooldown());
        currentStats.Add(playerStat.GetEffectiveCritRate());

        for (int counter = 0; counter < currentStats.Count; counter++)
        {
            statDifferenceTexts[counter].text = currentStats[counter].ToString();
            statDifferenceTexts[counter].color = new Color32(255, 255, 255, 255);
        }
    }





    //Debug.Log(UnityEngine.EventSystems.multiEventSys.currentSelectedGameObject.name);


}
