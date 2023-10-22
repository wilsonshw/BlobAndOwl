using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemybasic : MonoBehaviour
{
    public NavMeshAgent myNav;
    public Transform myTarget;
    public float myDist; //how far from target do I stop
    Vector3 myDest;
    float cdToDestCalc;
    Quaternion selfRot;
    Vector3 lookPos;
    Vector3 kbPos;

    public bool isAtk;
    public bool isKO;
    public bool isNearEnough;
    public float atkCd;
    public float atkTimer;
    public Animator selfAnim;

    public bool isKB; //isknocked back

    public float selfHP;
    public float maxHP;
    public CapsuleCollider myColl;

    public SkinnedMeshRenderer[] myMeshes;

    public spawncontroller spawnCont;

    public ParticleSystem poof;
    IEnumerator StopKBCo;

    public float imp;
    public Image lifeBar;
    // Start is called before the first frame update
    void Start()
    {
        poof.Play();
        selfHP = maxHP;
        lifeBar.fillAmount = selfHP / maxHP;
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

        /*if(isKB)
        {
            if (transform.position == kbPos)
                isKB = false;
        }*/
       
    }

    void CalculateMyDest()
    {
        if(!isKB)
        {
            if (Vector3.Distance(transform.position, myTarget.position) > myDist)
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
        /*else
        {
            myNav.SetDestination(kbPos);
        }*/
       
       
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
                float acc = sc.effectiveACC; //0 ~ 15
                acc = Mathf.RoundToInt(75 + ((25 / 15) * acc));
                crit = Mathf.RoundToInt(crit * 100); //scale to %

                int rando = Random.Range(0, 100); //roll 0 ~ 99
                if (rando < crit)
                {
                    dmg *= 2;
                    theColor = Color.red;
                    theSize *= 2;
                }

                int rando1 = Random.Range(0, 100);
                if(other.tag == "bullet")
                {
                    imp = sc.effectiveIMP; //1~3
                    if (rando1 < acc) //if less than 75, hit. 25% chance miss
                    {
                        Vector3 popupPos = transform.position + Vector3.up * (myColl.height * transform.localScale.x);
                        sc.myParent.DmgPopUp(dmg, popupPos, theColor, theSize);

                        selfHP -= dmg;
                        lifeBar.fillAmount = selfHP / maxHP;
                        if (selfHP > 0)
                        {
                            isKB = true;
                            myNav.SetDestination(transform.position);
                            ResetAnims();
                            selfAnim.SetInteger("idle", 1);
                            if (StopKBCo != null)
                                StopCoroutine(StopKBCo);

                            StopKBCo = StopKB();
                            StartCoroutine(StopKBCo);
                            /*Vector3 dir = transform.position - other.transform.position;
                            dir.y = transform.position.y;
                            dir.Normalize();
                            kbPos = transform.position + dir * imp;
                            myNav.SetDestination(kbPos);*/
                        }                        
                        else if (selfHP <= 0)
                        {
                            DoKoStuff();
                            if (spawnCont)
                            {
                                spawnCont.killCount+=3;
                                spawnCont.currentScore.text = spawnCont.killCount.ToString();
                            }
                        }

                    }
                    else
                    {
                        //handle miss
                        Vector3 popupPos = transform.position + Vector3.up * (myColl.height * transform.localScale.x);
                        sc.myParent.DmgPopUp(0, popupPos, theColor, theSize);
                    }
                }
                else
                {
                    Vector3 popupPos = transform.position + Vector3.up * (myColl.height * transform.localScale.x);
                    sc.myParent.DmgPopUp(dmg, popupPos, theColor, theSize);

                    selfHP -= dmg;
                    lifeBar.fillAmount = selfHP / maxHP;
                    if (selfHP <= 0)
                    {
                        DoKoStuff();
                        if (spawnCont)
                        {
                            spawnCont.killCount+=3;
                            spawnCont.currentScore.text = spawnCont.killCount.ToString();
                        }
                    }
                }
               

                if (other.tag == "bullet")
                {
                    bullet sc1 = other.GetComponent<bullet>();
                    sc1.PerformCollision();
                }

            }
            
        }
    }

    IEnumerator StopKB()
    {
        yield return new WaitForSeconds(imp);
        isKB = false;
    }

    public void DoKoStuff()
    {
        if (StopKBCo != null)
            StopCoroutine(StopKBCo);
        ResetAnims();
        myColl.enabled = false;
        selfHP = 0;
        lifeBar.fillAmount = selfHP / maxHP;
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
