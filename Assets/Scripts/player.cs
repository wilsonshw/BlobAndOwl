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
        NormalMoveStuff();
        NormalSlerpStuff();
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
       
    }

    public void OnPause(InputAction.CallbackContext value)
    {

    }

    public void ResetAnims()
    {
        selfAnim.SetInteger("idle", 0);
        selfAnim.SetInteger("move", 0);
        selfAnim.SetInteger("gunfire", 0);
    }

}
