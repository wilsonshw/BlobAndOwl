using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MenuNavigationManager : MonoBehaviour
{
    //======================== Setup ========================
    [SerializeField] Transform weaponMenuPanel;
    [SerializeField] TextMeshProUGUI weaponPartSelectedText;

    private void Start()
    {
        
    }

    //======================== Button Functions ========================

    public void OnButtonPress_WeaponPart()
    {
        weaponMenuPanel.DOLocalMoveX(800f, 0.5f, true);
    }

    public void OnButtonHover_WeaponPart(string weaponPartName)
    {
        weaponPartSelectedText.text = weaponPartName;
    }
}
