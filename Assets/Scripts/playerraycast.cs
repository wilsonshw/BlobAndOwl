using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerraycast : MonoBehaviour
{
    public float sphereCastRadius;
    public CapsuleCollider selfCapsuleColl;
    public GameObject myMarker;
    public GameObject targetObj;
    RaycastHit rayHit;
    public LayerMask theMask;

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        if (Physics.SphereCast(transform.TransformPoint(selfCapsuleColl.center), sphereCastRadius, transform.forward, out rayHit, Mathf.Infinity, theMask))
        {
            if(rayHit.transform.tag == "enemy")
            {
                myMarker.SetActive(true);
                CapsuleCollider theColl = rayHit.transform.GetComponent<CapsuleCollider>();
                myMarker.transform.position = theColl.transform.TransformPoint(theColl.center);
                targetObj = rayHit.transform.gameObject;
            }
            else
            {
                if (myMarker.activeSelf)
                    myMarker.SetActive(false);

                if(targetObj)
                    targetObj = null;
            }
        }
        else
        {
            if (myMarker.activeSelf)
                myMarker.SetActive(false);
            if(targetObj)
                targetObj = null;
        }
    }
}
