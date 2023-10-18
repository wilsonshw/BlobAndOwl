using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSFX : MonoBehaviour
{

    [SerializeField] private AudioSource meleeSwing;
    [SerializeField] private AudioSource meleeHit;

    [SerializeField] private AudioSource rangedShot;
    [SerializeField] private AudioSource rangedHit;

    public void PlayRangedShot()
    {
        rangedShot.Play();
    }

    public void PlayMeleeSwing()
    {
        meleeSwing.Play();
    }






}
