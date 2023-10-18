using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class tweenme : MonoBehaviour
{
    public bool hitEffect;
    public bool popupNo;
    public TextMeshProUGUI myText;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (hitEffect)
        {
            RectTransform myRect = transform.GetComponent<RectTransform>();
            Vector3 myScale = myRect.localScale;
            myRect.localScale = Vector3.zero;
            myRect.DOScale(myScale, 0.2f).onComplete = DisableSelf;
        }
        else if(popupNo)
        {
            Vector3 moveTo = transform.position + Vector3.up * 0.2f;
            Vector3 myScale = transform.localScale;
            transform.localScale = Vector3.zero;
            transform.DOScale(myScale, 0.2f).onComplete = DisableSelf;
            transform.DOMove(moveTo, 0.2f);
        }
    }

    void DisableSelf()
    {
        transform.gameObject.SetActive(false);
    }
}
