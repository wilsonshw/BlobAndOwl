using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;


//this is Dennis;
//this is continuation of main;
//this is scary - Dennis 11:12pm 14/10/3000
//this is Wilson's comment

// this is Najib 

public class player : MonoBehaviour
{
    public float selfSpeed;

    public bool isMoving; //i.e. input detected
    public bool isAtk;

    public Vector2 inputVec;

    public Vector3 direction;
    public Vector3 lookPos;
    public Vector3 targetPos;

    //public Image breathBar;

    public Quaternion selfRot;

    public Rigidbody selfRigid;

    public CapsuleCollider selfCapsuleColl;

    public Animator selfAnim;

    Vector3 camFollowOffset;

    public float selfHP;
    public float maxHP;
    //public Image lifeBar;

    IEnumerator BlinkCo;
    IEnumerator BlinkRevertCo;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    public ParticleSystem gunSmoke;

    public RuntimeAnimatorController rangeCont;
    public RuntimeAnimatorController meleeCont;

    public UI_manager ourMenu;
    public JAR_StatController statCont;

    public damagestats meleeStats;
    public damagestats rangeStats;

    public GameObject popupParent;
    // Start is called before the first frame update
    void Start()
    {       
        selfHP = maxHP;
        //lifeBar.fillAmount = selfHP / maxHP;
        //camFollowOffset = camFollow.transform.position - transform.position;
        //breathBar.fillAmount = 0;
    }

    private void Update()
    {
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (AllowMove())
        {
            NormalMoveStuff();
            NormalSlerpStuff();
        }
    }

    private void LateUpdate()
    {
        //camFollow.transform.position = transform.position + camFollowOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
       
    }

    private void OnTriggerStay(Collider other)
    {
      
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "fakewall")
        {
            Vector3 theNormal = collision.contacts[0].normal;
            theNormal.Normalize();
            Vector3 myVel = selfRigid.velocity;
            selfRigid.velocity = Vector3.zero;
            myVel = Vector3.ProjectOnPlane(myVel, theNormal);
            selfRigid.velocity = myVel;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "fakewall")
        {
            Vector3 theNormal = collision.contacts[0].normal;
            theNormal.Normalize();
            Vector3 myVel = selfRigid.velocity;
            selfRigid.velocity = Vector3.zero;
            myVel = Vector3.ProjectOnPlane(myVel, theNormal);
            selfRigid.velocity = myVel;
        }
    }

    public void DoKoStuff()
    {
       
    }

    public void DoBlink()
    {
        if (BlinkCo != null)
            StopCoroutine(BlinkCo);
        BlinkCo = Blink();
        StartCoroutine(BlinkCo);
    }

    IEnumerator Blink()
    {
        yield return new WaitForSeconds(0.1f);

        if (BlinkRevertCo != null)
            StopCoroutine(BlinkRevertCo);
        BlinkRevertCo = BlinkRevert();
        StartCoroutine(BlinkRevertCo);
    }

    IEnumerator BlinkRevert()
    {
        yield return new WaitForSeconds(0.1f);
        if (BlinkCo != null)
            StopCoroutine(BlinkCo);
        BlinkCo = Blink();
    }

    public void NormalMoveStuff()
    {
        selfRigid.velocity = Vector3.zero;
        direction = new Vector3(inputVec.x, 0, inputVec.y);
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var skewedDir = matrix.MultiplyPoint3x4(direction);

        skewedDir = Vector3.ProjectOnPlane(skewedDir, Vector3.up);
        skewedDir.Normalize();
        //direction = Camera.main.transform.TransformDirection(direction);       
        float distance = selfSpeed * Time.deltaTime;
        skewedDir *= distance;
        targetPos = transform.position + skewedDir;
        selfRigid.MovePosition(targetPos);

        if (inputVec == Vector2.zero)
        {
            if (isMoving) //meaning running animation on ground, or jumping animation in air
            {
                selfRigid.velocity = Vector3.zero;
                ResetAnims();
                selfAnim.SetInteger("idle", 1);
                isMoving = false;
            }
            else
            {
                selfRigid.velocity = Vector3.zero;
                if (selfAnim.GetInteger("idle") != 1)
                {
                    ResetAnims();
                    selfAnim.SetInteger("idle", 1);
                }
            }
        }
        else
        {
            if (!isMoving)
            {
                ResetAnims();
                selfAnim.SetInteger("move", 1);
                isMoving = true;
            }
            else
            {
                if (selfAnim.GetInteger("move") != 1)
                {
                    ResetAnims();
                    selfAnim.SetInteger("move", 1);
                }
            }
        }
    }

    public void NormalSlerpStuff()
    {
        lookPos = targetPos - transform.position;
        if (lookPos != Vector3.zero)
            selfRot = Quaternion.LookRotation(lookPos);
        else
            selfRot = Quaternion.LookRotation(transform.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, selfRot, 20 * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        ReadInputVec(value);
    }

    void ReadInputVec(InputAction.CallbackContext value)
    {
        inputVec = Vector2.zero;
        inputVec = value.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext value)
    {
       if(value.performed)
        {
            if (!isAtk && !selfAnim.GetCurrentAnimatorStateInfo(0).IsName("gunfire"))
                DoAtk();
        }
    }

    void DoAtk()
    {
        if (selfAnim.runtimeAnimatorController == meleeCont)
        {
            meleeStats.effectiveDMG = statCont.GetEffectiveDMG();
            meleeStats.effectiveCRIT = statCont.GetEffectiveCritRate();
            meleeStats.effectiveDEF = statCont.GetEffectiveDEF();
            meleeStats.effectiveAOE = statCont.GetEffectiveAoE();
            meleeStats.effectiveCD = statCont.GetEffectiveCooldown();
        }

        isAtk = true;
        ResetAnims();
        selfAnim.SetInteger("gunfire", 1);
        if (selfAnim.runtimeAnimatorController == rangeCont)
        {
            SpawnBullet();
            gunSmoke.Play();
        }
    }

    void FalsifyAtk()
    {
        isAtk = false;
        ResetAnims();
    }

    public void OnMenu(InputAction.CallbackContext value)
    {
        if(!ourMenu.menuOpen)
        {
            ourMenu.menuOpen = true;
            ourMenu.myMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            ourMenu.menuOpen = false;
            ourMenu.myMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void ResetAnims()
    {
        selfAnim.SetInteger("idle", 0);
        selfAnim.SetInteger("move", 0);
        selfAnim.SetInteger("gunfire", 0);
    }

    public bool AllowMove()
    {
        return !isAtk;
    }

    public void SpawnBullet()
    {
        var inst = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity) as GameObject;
        if (GetComponent<playerraycast>().targetObj)
            inst.GetComponent<bullet>().targetObj = GetComponent<playerraycast>().targetObj;
        inst.GetComponent<bullet>().moveMe = true;
        inst.transform.LookAt(inst.transform.position + bulletSpawnPoint.transform.forward);
    }

    public void DmgPopUp(int theDmg, Vector3 thePos, Color theColor, Vector3 theSize)
    {
        for (int i = 0; i < popupParent.transform.childCount; i++)
        {
            GameObject myChild = popupParent.transform.GetChild(i).gameObject;
            if (!myChild.activeSelf)
            {
                myChild.transform.position = thePos;
                tweenme sc = myChild.GetComponent<tweenme>();
                sc.myText.text = theDmg.ToString();
                sc.myText.color = theColor;
                myChild.transform.localScale = theSize;
                myChild.SetActive(true);
                break;
            }
        }
    }
}
