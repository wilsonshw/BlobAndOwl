using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemybasic : MonoBehaviour
{
    public NavMeshAgent myNav;
    public Transform myTarget;
    public float myDist; //how far from target do I stop
    Vector3 myDest;
    float cdToDestCalc;
    Quaternion selfRot;
    Vector3 lookPos;

    public bool isAtk;
    public bool isNearEnough;
    public float atkCd;
    public float atkTimer;
    public Animator selfAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(myTarget)
        {
            if(!isAtk)
            {
                if (cdToDestCalc >= 0)
                    cdToDestCalc -= Time.deltaTime;
                else
                {
                    CalculateMyDest();
                    cdToDestCalc = 0.2f;
                }
            }       

            if(isNearEnough)
            {
                if (!isAtk && !selfAnim.GetCurrentAnimatorStateInfo(0).IsName("gunfire"))
                {
                    if (atkCd >= 0)
                        atkCd -= Time.deltaTime;
                    else
                    {
                        DoAttack();
                        atkCd = atkTimer;
                    }
                }
            }          
            LookAtTarget();
           // myNav.SetDestination(myTarget.position);
        }
    }

    void CalculateMyDest()
    {
        if(Vector3.Distance(transform.position,myTarget.position) > myDist)
        {
            if (!selfAnim.GetCurrentAnimatorStateInfo(0).IsName("move"))
            {
                ResetAnims();
                selfAnim.SetInteger("move", 1);
            }

            Vector3 dir = myTarget.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            myDest = myTarget.position - dir * myDist;
            myNav.SetDestination(myDest);
            isNearEnough = false;                    
        }
        else
        {
            myNav.SetDestination(transform.position);
            isNearEnough = true;
        }
       
    }

    void DoAttack()
    {
        myNav.SetDestination(transform.position);
        isAtk = true;
        ResetAnims();
        selfAnim.SetInteger("gunfire", 1);
    }

    void FalsifyAtk()
    {
        isAtk = false;
        ResetAnims();
    }

    void LookAtTarget()
    {
        lookPos = myTarget.position - transform.position;
        lookPos.y = transform.position.y;
        if (lookPos != Vector3.zero)
            selfRot = Quaternion.LookRotation(lookPos);
        else
            selfRot = Quaternion.LookRotation(transform.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, selfRot, 20 * Time.deltaTime);
    }

    public void ResetAnims()
    {
        selfAnim.SetInteger("idle", 0);
        selfAnim.SetInteger("move", 0);
        selfAnim.SetInteger("gunfire", 0);
    }
}
