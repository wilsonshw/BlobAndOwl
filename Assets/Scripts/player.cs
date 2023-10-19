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
    public float dodgeDuration;
    public float dodgeSpeed;
    public float invincibleDuration;
    public float selfSpeed;

    public bool isDodge;
    public bool isInvincible;
    public bool isMoving; //i.e. input detected
    public bool isAtk;

    public Vector2 inputVec;

    public Vector3 direction;
    public Vector3 dodgeDir;
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
    IEnumerator InvincibleCo;

    IEnumerator DodgeCo;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    public ParticleSystem dodgeDust;
    public ParticleSystem gunSmoke;

    public RuntimeAnimatorController rangeCont;
    public RuntimeAnimatorController meleeCont;

    public UI_manager ourMenu;
    public GameObject defaultMenuButton;
    public JAR_StatController statCont;

    public damagestats meleeStats;


    public GameObject popupParent;

    public MeshRenderer[] meshes;
    public SkinnedMeshRenderer[] skinnedMeshes;

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

        if (isDodge)
        {
            DoDodge();
        }

    }

    private void LateUpdate()
    {
        //camFollow.transform.position = transform.position + camFollowOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            if (!isInvincible)
            {

                for (int i = 0; i < meshes.Length; i++)
                    meshes[i].enabled = false;

                for (int i = 0; i < skinnedMeshes.Length; i++)
                    skinnedMeshes[i].enabled = false;

                DoBlinkRevert();

                isInvincible = true;
                if (InvincibleCo != null)
                    StopCoroutine(InvincibleCo);
                InvincibleCo = StopInvincible();
                StartCoroutine(InvincibleCo);
            }

        }


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

    public void DoBlinkRevert()
    {
        if (BlinkRevertCo != null)
            StopCoroutine(BlinkRevertCo);
        BlinkRevertCo = BlinkRevert();
        StartCoroutine(BlinkRevertCo);
    }

    IEnumerator Blink()
    {
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < meshes.Length; i++)
            meshes[i].enabled = false;

        for (int i = 0; i < skinnedMeshes.Length; i++)
            skinnedMeshes[i].enabled = false;

        if (BlinkRevertCo != null)
            StopCoroutine(BlinkRevertCo);
        BlinkRevertCo = BlinkRevert();
        StartCoroutine(BlinkRevertCo);
    }

    IEnumerator BlinkRevert()
    {
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < meshes.Length; i++)
            meshes[i].enabled = true;

        for (int i = 0; i < skinnedMeshes.Length; i++)
            skinnedMeshes[i].enabled = true;

        if (BlinkCo != null)
            StopCoroutine(BlinkCo);
        BlinkCo = Blink();
        StartCoroutine(BlinkCo);
    }

    IEnumerator StopInvincible()
    {
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
        if (BlinkRevertCo != null)
            StopCoroutine(BlinkRevertCo);
        if (BlinkCo != null)
            StopCoroutine(BlinkCo);

        for (int i = 0; i < meshes.Length; i++)
            meshes[i].enabled = true;

        for (int i = 0; i < skinnedMeshes.Length; i++)
            skinnedMeshes[i].enabled = true;

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
        targetPos.y = transform.position.y;
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
        if (value.performed)
        {
            if (CanAttack())
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

            //JAR: set up default button placement on menu open;
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultMenuButton);

            Time.timeScale = 0;
        }
        else
        {
            ourMenu.menuOpen = false;
            ourMenu.myMenu.GetComponent<MenuNavigationManager>().OnBackPress_WeaponPart();
            ourMenu.myMenu.SetActive(false);

            //JAR: undo cursor locks;
            //Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 1;
        }
    }

    public void OnDodge(InputAction.CallbackContext value)
    {
        if (value.performed && Time.timeScale!=0)
        {
            if (!isDodge)
            {
                FalsifyAtk();
                isDodge = true;
                dodgeDust.Play();
                if (inputVec != Vector2.zero)
                    dodgeDir = new Vector3(inputVec.x, 0, inputVec.y);
                else
                {
                    dodgeDir = transform.forward;
                    var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, -45, 0));
                    var skewedDir = matrix.MultiplyPoint3x4(dodgeDir);
                    dodgeDir = skewedDir;
                }
                ResetAnims();
                selfAnim.SetInteger("dodge", 1);

                if (DodgeCo != null)
                    StopCoroutine(DodgeCo);
                DodgeCo = StopDodge();
                StartCoroutine(DodgeCo);
            }

        }
    }

    void DoDodge()
    {
        selfRigid.velocity = Vector3.zero;

        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var skewedDir = matrix.MultiplyPoint3x4(dodgeDir);

        skewedDir = Vector3.ProjectOnPlane(skewedDir, Vector3.up);
        skewedDir.Normalize();
        float distance = dodgeSpeed * Time.deltaTime;
        skewedDir *= distance;
        targetPos = transform.position + skewedDir;
        targetPos.y = transform.position.y;
        selfRigid.MovePosition(targetPos);
        transform.LookAt(targetPos);
    }

    IEnumerator StopDodge()
    {
        yield return new WaitForSeconds(dodgeDuration);
        isDodge = false;
        dodgeDust.Stop();
        ResetAnims();
    }

    public void ResetAnims()
    {
        selfAnim.SetInteger("idle", 0);
        selfAnim.SetInteger("move", 0);
        selfAnim.SetInteger("gunfire", 0);
        selfAnim.SetInteger("dodge", 0);
    }

    public bool AllowMove()
    {
        return !isAtk && !isDodge;
    }

    public bool CanAttack()
    {
        return !isAtk && !selfAnim.GetCurrentAnimatorStateInfo(0).IsName("gunfire") && !isDodge && Time.timeScale != 0;
    }

    public void SpawnBullet()
    {
        var inst = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity) as GameObject;
        if (GetComponent<playerraycast>().targetObj)
            inst.GetComponent<bullet>().targetObj = GetComponent<playerraycast>().targetObj;
        inst.GetComponent<bullet>().moveMe = true;
        inst.transform.LookAt(inst.transform.position + bulletSpawnPoint.transform.forward);

        damagestats sc = inst.GetComponent<damagestats>();
        sc.effectiveDMG = statCont.GetEffectiveDMG();
        sc.effectiveCRIT = statCont.GetEffectiveCritRate();
        sc.myParent = this;
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
