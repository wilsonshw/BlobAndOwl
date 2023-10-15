using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class tweenme : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        RectTransform myRect = transform.GetComponent<RectTransform>();
        Vector3 myScale = myRect.localScale;
        myRect.localScale = Vector3.zero;
        myRect.DOScale(myScale, 0.2f).onComplete = DisableSelf;
    }

    void DisableSelf()
    {
        transform.gameObject.SetActive(false);
    }
}
