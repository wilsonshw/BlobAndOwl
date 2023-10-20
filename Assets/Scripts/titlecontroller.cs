using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class titlecontroller : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject subMenu;
    public Image blackOut;

    public bool isTransition;
    public string nextSceneName;

    public GameObject mainSel; //in menu, default selection
    public GameObject subSel; //in submenu, default selection

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainSel);
    }

    public void ClickStart()
    {
        if(!isTransition)
        {
            mainMenu.SetActive(false);
            subMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(subSel);
        }
    
    }

    public void ClickQuit()
    {
        Application.Quit();
    }

    public void ClickStage(string myName)
    {
        if (!isTransition)
        {
            isTransition = true;
            nextSceneName = myName;
            StartCoroutine(DoBlackOut());
        }
    }

    public void OnCancel(InputAction.CallbackContext value)
    {
        if(value.performed)
        {
            if (!isTransition)
            {
                if (subMenu.activeSelf)
                {
                    subMenu.SetActive(false);
                    mainMenu.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(mainSel);
                }
            }
        }
       
    }

    IEnumerator DoBlackOut()
    {
        CanvasGroup cvGroup = blackOut.GetComponent<CanvasGroup>();
        while (cvGroup.alpha < 1)
        {
            cvGroup.alpha += Time.unscaledDeltaTime;
            yield return null;
        }
        SceneManager.LoadScene(nextSceneName);
    }
}
