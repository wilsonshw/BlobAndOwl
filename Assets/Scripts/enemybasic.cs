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
    public bool isKO;
    public bool isNearEnough;
    public float atkCd;
    public float atkTimer;
    public Animator selfAnim;

    public float impactDist; //knockback distance
    public bool isKB; //isknocked back

    public int selfHP;
    public int maxHP;
    public CapsuleCollider myColl;

    public SkinnedMeshRenderer[] myMeshes;

    public spawncontroller spawnCont;

    public ParticleSystem poof;
    // Start is called before the first frame update
    void Start()
    {
        poof.Play();
        selfHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if(myTarget && !isKO)
        {
            if(!isAtk)
            {
                if (cdToDestCalc >= 0)
                    cdToDestCalc -= Time.deltaTime;
                else
                {
                    CalculateMyDest();
                    cdToDestCalc = 0.05f;
                }
            }       

            if (!isAtk && !selfAnim.GetCurrentAnimatorStateInfo(0).IsName("gunfire"))
            {
                if (atkCd >= 0)
                    atkCd -= Time.deltaTime;
                else
                {
                    if (isNearEnough && !isKB)
                    {
                        DoAttack();
                        atkCd = atkTimer;
                    }
                }
            }
        
            if(CanSlerp())
                LookAtTarget();
        }
    }

    void CalculateMyDest()
    {
        if(Vector3.Distance(transform.position,myTarget.position) > myDist)
        {
            Vector3 dir = transform.position - myTarget.position;
            dir.y = transform.position.y;
            dir.Normalize();
            myDest = myTarget.position + dir * myDist;

            if (Vector3.Distance(transform.position, myDest) >= 0.05f)
            {
                if (!selfAnim.GetCurrentAnimatorStateInfo(0).IsName("move"))
                {
                    ResetAnims();
                    selfAnim.SetInteger("move", 1);
                }
               
                isNearEnough = false;
            }
            else
            {
                if (!isAtk)
                {
                    ResetAnims();
                    selfAnim.SetInteger("idle", 1);
                }
                myDest = transform.position;
                isNearEnough = true;
            }

            myNav.SetDestination(myDest);

        }
        else
        {
            if (!isAtk)
            {
                ResetAnims();
                selfAnim.SetInteger("idle", 1);
            }
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

    bool CanSlerp()
    {
        return !isAtk && !isKB;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "atk" || other.tag == "bullet")
        {
            if(!isKO)
            {
                damagestats sc = other.GetComponent<damagestats>();
                Color theColor = Color.yellow;
                Vector3 theSize = new Vector3(1, 1, 1);
                int dmg = Mathf.RoundToInt(sc.effectiveDMG);
                float crit = sc.effectiveCRIT;
                crit = Mathf.RoundToInt(crit * 100); //scale to %

                int rando = Random.Range(0, 100); //roll 0 ~ 99
                if (rando < crit)
                {
                    dmg *= 2;
                    theColor = Color.red;
                    theSize *= 2;
                }

                Vector3 popupPos = transform.position + Vector3.up * (myColl.height * transform.localScale.x);
                sc.myParent.DmgPopUp(dmg, popupPos, theColor, theSize);

                selfHP -= dmg;
                if (selfHP <= 0)
                {
                    DoKoStuff();
                }

                if(other.tag == "bullet")
                {
                    bullet sc1 = other.GetComponent<bullet>();
                    sc1.PerformCollision();
                }

            }
            
        }
    }

    void DoKoStuff()
    {
        selfHP = 0;
        poof.Play();
        isKO = true;
        myNav.SetDestination(transform.position);
        for (int i = 0; i < myMeshes.Length; i++)
            myMeshes[i].enabled = false;
        StartCoroutine(DestroySelf());
        if (spawnCont)
            spawnCont.enemyCounter--;

    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(1);
        Destroy(transform.gameObject);
    }
}
