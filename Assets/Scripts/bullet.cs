using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameObject targetObj;
    public GameObject hitEffect;
    public MeshRenderer myMesh;
    public Vector3 direction;
    public float myLifetime;
    public float selfSpeed;
    public bool moveMe;

    private void Start()
    {
        if (targetObj)
        {
            direction = targetObj.transform.position - transform.position;
            direction.y = transform.position.y;
            direction.Normalize();
        }
        else
            direction = transform.forward;

    }

    void Update()
    {
        if (moveMe)
        {
            if (myLifetime >= 0)
                myLifetime -= Time.deltaTime;
            else
            {
                PerformCollision();
            }

            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, selfSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "enemy")
        {
            PerformCollision();
        }
    }

    void PerformCollision()
    {
        moveMe = false;
        myMesh.enabled = false;
        hitEffect.SetActive(true);
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(transform.gameObject);
    }
}
