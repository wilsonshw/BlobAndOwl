using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.InputSystem;

public class WeaponMenuManager : MonoBehaviour
{
    public enum WeaponPage { Overview, Detailview }

    [SerializeField] private WeaponPage currentPage;

    [Header("Editor Visual References")]
    [SerializeField] private GameObject overviewPage;
    [SerializeField] private GameObject detailviewPage;
    [SerializeField] private GameObject[] allWeaponParts;
    public bool isOverview;
    public bool isDetailedView;


    [Header("Overview Visual References")]
    [SerializeField] private TextMeshProUGUI selectedPartOverviewText;

    [Header("Detailview Visual References")]
    [SerializeField] private TextMeshProUGUI selectedPartDetailviewText;

    private void Start()
    {
        isDetailedView = false;
        isOverview = true;
        currentPage = WeaponPage.Overview;
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && currentPage == WeaponPage.Detailview)
            MoveToOverviewPage();
    }*/


    //============================= Button Methods =============================
    private Vector3 initialSelectedPartPosition;
    private GameObject selectedPartObject;

    public MenuNavigationManager rangeNavMgr;
    public MenuNavigationManager_Melee meleeNavMgr;

    private void MoveToDetailviewPage(GameObject selectedPartObjectPass)
    {
        isDetailedView = true;
        isOverview = false;
        currentPage = WeaponPage.Detailview;

        // Swipes the page up
        overviewPage.transform.DOLocalMoveY(450f, 0.5f, true).SetUpdate(true);
        detailviewPage.transform.DOLocalMoveY(0f, 0.5f, true).SetUpdate(true);

        // Scales down all the parts
        foreach (GameObject weaponObject in allWeaponParts)
            weaponObject.transform.DOScale(Vector3.zero, 0.2f).SetUpdate(true);

        // Saves data of selected part into memory
        selectedPartObject = selectedPartObjectPass;
        initialSelectedPartPosition = selectedPartObject.transform.localPosition;

        // Scales up the selected part and moves it to center
        if (selectedPartObjectPass.name == "Blade")
        {
            selectedPartObject.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f).SetUpdate(true);
            selectedPartObject.transform.DOLocalMove(new Vector3(0f, 60f, 0f), 0.2f, true).SetUpdate(true);
        }
        else
        {
            selectedPartObject.transform.DOScale(Vector3.one, 0.2f).SetUpdate(true);
            selectedPartObject.transform.DOLocalMove(new Vector3(0f, 60f, 0f), 0.2f, true).SetUpdate(true);
        }

    }

    public void MoveToOverviewPage()
    {
        isOverview = true;
        isDetailedView = false;
        currentPage = WeaponPage.Overview;

        // Swipes the page down
        overviewPage.transform.DOLocalMoveY(0f, 0.5f, true).SetUpdate(true);
        detailviewPage.transform.DOLocalMoveY(-450f, 0.5f, true).SetUpdate(true);

        // Resets scale and position of the parts
        foreach (GameObject weaponObject in allWeaponParts)
            weaponObject.transform.DOScale(Vector3.one, 0.2f).SetUpdate(true);
        selectedPartObject.transform.DOLocalMove(initialSelectedPartPosition, 0.2f, true).SetUpdate(true);

        if (rangeNavMgr)
        {
            rangeNavMgr.OnBackPress_WeaponPart();
            rangeNavMgr.EnableButtons();
        }

        if (meleeNavMgr)
        {
            meleeNavMgr.OnBackPress_WeaponPart();
            meleeNavMgr.EnableButtons();
        }
    }

    public void OverViewInstant() //when exit from menu, reset back to overview
    {
        currentPage = WeaponPage.Overview;

        // Swipes the page down
        //overviewPage.transform.DOLocalMoveY(0f, 0.5f, true).SetUpdate(true);
        //detailviewPage.transform.DOLocalMoveY(-450f, 0.5f, true).SetUpdate(true);
        overviewPage.transform.localPosition = new Vector3(overviewPage.transform.localPosition.x, 0, overviewPage.transform.localPosition.z);
        detailviewPage.transform.localPosition = new Vector3(detailviewPage.transform.localPosition.x, -450, detailviewPage.transform.localPosition.z);

        // Resets scale and position of the parts
        foreach (GameObject weaponObject in allWeaponParts)
            weaponObject.transform.localScale = new Vector3(1, 1, 1);
        if (selectedPartObject)
            selectedPartObject.transform.localPosition = initialSelectedPartPosition;


        if (rangeNavMgr)
            rangeNavMgr.EnableButtons();

        if (meleeNavMgr)
            meleeNavMgr.EnableButtons();
    }

    //============================= Button Methods =============================

    public void highlightWeaponPart(string weaponPartName)
    {
        if (currentPage == WeaponPage.Overview)
        {
            selectedPartOverviewText.text = weaponPartName;
            selectedPartDetailviewText.text = weaponPartName;
        }
    }

    public void clickedWeaponPart(GameObject selectedPartObject)
    {
        if (currentPage == WeaponPage.Overview)
            MoveToDetailviewPage(selectedPartObject);
    }


}
