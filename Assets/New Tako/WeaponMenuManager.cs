using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class WeaponMenuManager : MonoBehaviour
{
    public enum WeaponPage { Overview, Detailview }

    [SerializeField] private WeaponPage currentPage;

    [Header("Editor Visual References")]
    [SerializeField] private GameObject overviewPage;
    [SerializeField] private GameObject detailviewPage;
    [SerializeField] private GameObject[] allWeaponParts;


    [Header("Overview Visual References")]
    [SerializeField] private TextMeshProUGUI selectedPartOverviewText;

    [Header("Detailview Visual References")]
    [SerializeField] private TextMeshProUGUI selectedPartDetailviewText;

    private void Start()
    {
        currentPage = WeaponPage.Overview;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && currentPage == WeaponPage.Detailview)
            MoveToOverviewPage();
    }

    //============================= Button Methods =============================
    private Vector3 initialSelectedPartPosition;
    private GameObject selectedPartObject;


    private void MoveToDetailviewPage(GameObject selectedPartObjectPass)
    {
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

    private void MoveToOverviewPage()
    {
        currentPage = WeaponPage.Overview;

        // Swipes the page down
        overviewPage.transform.DOLocalMoveY(0f, 0.5f, true).SetUpdate(true);
        detailviewPage.transform.DOLocalMoveY(-450f, 0.5f, true).SetUpdate(true);

        // Resets scale and position of the parts
        foreach (GameObject weaponObject in allWeaponParts)
            weaponObject.transform.DOScale(Vector3.one, 0.2f).SetUpdate(true);
        selectedPartObject.transform.DOLocalMove(initialSelectedPartPosition, 0.2f, true).SetUpdate(true);
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
